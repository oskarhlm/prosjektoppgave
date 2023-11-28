/* 
 * Oppdateringsgrensesnitt for SFKB
 *
 * # NGIS-OpenAPI  Grov oversikt over funksjonalitet:   - Hente liste over tilgjengelige datasett    - Hente metadata for et bestemt datasett   - Hente data fra et bestemt datasett     - Med lesetilgang eller skrivetilgang (medfører låsing)       - områdebegrensning       - egenskapsspørring (begrenset i første versjon til bygningsnummer eller lokalid)   - Lagre data til et bestemt datasett     - Operasjoner som håndteres: nytt objekt, endre objekt og slett objekt  ## Generelle prinsipper for systemet  ### Versjonering og bakoverkompatibilitet  #### Versjonsnummer i URL  Vi har et versjonsnummer `v1` i URL for å gjøre det mulig å gjøre store endringer i APIet hvis det blir nødvendig, men i utgangspunktet ønsker vi å unngå å endre dette versjonsnummeret.  #### Versjonsnummer i media types (\"media type versioning\", content negotiation)  APIet baserer seg på standard [HTTP content negotiation](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept) ved utveksling av data med headerne `Accept` og `Content-Type`. Dette gjør det veldig enkelt å introdusere nye dataformater i APIet uten endringer for eksisterende klienter. I tillegg til dette inneholder også alle dataformater et versjonsparameter, eks. `version=1.0`, der klienten kan styre hvilket  eller hvilke dataformater klienten kan håndtere. Dataformater angitt uten versjonsparameter vil tolkes som å be om siste versjon.  `Accept: application/vnd.kartverket.sosi+json; version=1.0` Klienten ønsker svar med versjon 1 av dataformatet      `Accept: application/vnd.kartverket.sosi+json; version=2.0` Klienten ønsker svar med versjon 2 av dataformatet  `Accept: application/vnd.kartverket.sosi+json` Klienten ønsker svar med siste versjon av dataformatet  `Accept: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0` Klienten håndterer både versjon 1.0 og 2.0 av dataformatet, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes  `Accept: *_/_*` Klienten håndterer alle dataformater, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes.  Der man kan velge mellom flere helt ulike dataformater som f.eks GML og JSON, må man faktisk håndtere begge.  ### Delt geometri  Flater består av avgrensningslinjer som ligger lagret som egne objekter. På den måten kan en linje avgrense ingen, én eller flere flater. Det er likevel slik at flater hentes ut og lagres med egen geometri for å gjøre det enklere å tegne opp datene, men ved endring av (delte) linjer og flater må det tas hensyn til delt geometri. Forsøk på endring av linje eller flate uten tilsvarende endring av evt. delt geometri vil bli avvist av systemet.  ### Låsing  Dette er nærmere beskrevet i de aktuelle kallene.  Foreløpig er det kun `user_lock` som er støttet. Det betyr at data må hentes ut med `user_lock` før de kan sendes inn med endringer.  ### Porsjonering  All uthenting av feature-objekter vil kunne bli porsjonert av serveren, se `limit`-parameteret.   ### Koordinatsystemer og transformasjon  Dersom annet koordinatsystem enn det som ligger i dataset skal brukes (se `GET /datasets/{datasetId}`) må koordinatsystem angis med `crs_EPSG`-parameteret. Dette styrer data som sendes inn, data som hentes ut og koordinatsystemet i `bbox`-parameteret i kallet. For å bytte rekkefølge på aksene brukes `crs_normalized_for_visualization`-parameteret.  ### Historikk og historiske endringer  Det er mulig å hente ut data for et gitt tidspunkt (for hele datasettet eller begrenset til et område, et objekt etc.). Se etter parameteret `dataset_at`.  Det er også mulig å hente ut historikken som endringer, f.eks som endringer fra et tidligere uthentet område, objekt eller helt dataset. Se etter parameteret `dataset_modified`.  ### FKB5 og QMS13  I forbindelse med FKB5 har det blitt gjort endringer i GeoJSON-formatet som benyttes i NGIS-OpenAPI. Endringene gjelder ved bruk av Versjon 2 av formatet,  som er påkrevd versjon for FKB 5.  Endringer i GeoJSON-formatet, Versjon 2:   - Noden `geometry_properties` er flyttet under `properties`   - Formatet støtter assosiasjoner mellom objekter og geometri-assosiasjoner ved flater med delt geometri   - Formatet støtter heleid geometri  #### Assosiasjoner mellom objekter Et objekt kan ha en assosiasjon til ett eller flere andre objekter. Assosiasjonene av samme type ligger i en array med `lokalId` og `featuretype` til det assosierte objektet.  Flater kan være modellert med krav om delt geometri for flateavgrensningen. Dette angis via geometri-assosiasjoner fra flate-objektet til linjene-objektene som avgrenser flata.  I tillegg til assosiasjons-informasjonen beskrevet over, har geometri-assosiasjonene følgende egenskaper for hvert assosiert objekt:      `reverse` er en bool som forteller om linjas retning skal snus eller ikke for å danne en sammenhengende flateavgrensing med de andre avgrensningslinjene.  `idx` er ett array med tre indekser som gir informasjon om avgrensningslinja:    1. angir hvilken flate linja tilhører (aktuell ved framtidig bruk av MultiSurface), skal for enkeltflater være 0    2. angir om linja tilhører ytre eller en indre avgrensning (hull) og eventuelt hvilken indre avgrensning linja tilhører       - Ytre avgrensning: 0       - Indre avgrensning/hull: 1..n    3. angir hvilken rekkefølge linja har i avgrensningen av flata som dannes av alle avgrensninslinjene (starter på 0) 
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
        public interface ILocksApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset
        /// </summary>
        /// <remarks>
        /// Fjerne alle låser brukeren har i et bestemt dataset 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns></returns>
        void DeleteDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null);

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset
        /// </summary>
        /// <remarks>
        /// Fjerne alle låser brukeren har i et bestemt dataset 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteDatasetLocksWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null);
        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.
        /// </summary>
        /// <remarks>
        /// Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Locks</returns>
        Locks GetDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null);

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.
        /// </summary>
        /// <remarks>
        /// Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>ApiResponse of Locks</returns>
        ApiResponse<Locks> GetDatasetLocksWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset
        /// </summary>
        /// <remarks>
        /// Fjerne alle låser brukeren har i et bestemt dataset 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteDatasetLocksAsync (string xClientProductVersion, Guid? datasetId, string lockingType = null);

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset
        /// </summary>
        /// <remarks>
        /// Fjerne alle låser brukeren har i et bestemt dataset 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteDatasetLocksAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null);
        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.
        /// </summary>
        /// <remarks>
        /// Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of Locks</returns>
        System.Threading.Tasks.Task<Locks> GetDatasetLocksAsync (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null);

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.
        /// </summary>
        /// <remarks>
        /// Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of ApiResponse (Locks)</returns>
        System.Threading.Tasks.Task<ApiResponse<Locks>> GetDatasetLocksAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
        public partial class LocksApi : ILocksApi
    {
        private IO.Swagger.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocksApi"/> class.
        /// </summary>
        /// <returns></returns>
        public LocksApi(String basePath)
        {
            this.Configuration = new IO.Swagger.Client.Configuration { BasePath = basePath };

            ExceptionFactory = IO.Swagger.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocksApi"/> class
        /// </summary>
        /// <returns></returns>
        public LocksApi()
        {
            this.Configuration = IO.Swagger.Client.Configuration.Default;

            ExceptionFactory = IO.Swagger.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocksApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public LocksApi(IO.Swagger.Client.Configuration configuration = null)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = IO.Swagger.Client.Configuration.Default;
            else
                this.Configuration = configuration;

            ExceptionFactory = IO.Swagger.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public IO.Swagger.Client.Configuration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public IO.Swagger.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<String, String> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(this.Configuration.DefaultHeader);
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset Fjerne alle låser brukeren har i et bestemt dataset 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns></returns>
        public void DeleteDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null)
        {
             DeleteDatasetLocksWithHttpInfo(xClientProductVersion, datasetId, lockingType);
        }

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset Fjerne alle låser brukeren har i et bestemt dataset 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DeleteDatasetLocksWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling LocksApi->DeleteDatasetLocks");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling LocksApi->DeleteDatasetLocks");

            var localVarPath = "/datasets/{datasetId}/locks";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteDatasetLocks", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset Fjerne alle låser brukeren har i et bestemt dataset 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteDatasetLocksAsync (string xClientProductVersion, Guid? datasetId, string lockingType = null)
        {
             await DeleteDatasetLocksAsyncWithHttpInfo(xClientProductVersion, datasetId, lockingType);

        }

        /// <summary>
        /// Fjerne alle låser brukeren har i et bestemt dataset Fjerne alle låser brukeren har i et bestemt dataset 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DeleteDatasetLocksAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling LocksApi->DeleteDatasetLocks");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling LocksApi->DeleteDatasetLocks");

            var localVarPath = "/datasets/{datasetId}/locks";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteDatasetLocks", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser. Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Locks</returns>
        public Locks GetDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
             ApiResponse<Locks> localVarResponse = GetDatasetLocksWithHttpInfo(xClientProductVersion, datasetId, lockingType, bbox, crsEPSG, normalizedForVisualization);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser. Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>ApiResponse of Locks</returns>
        public ApiResponse< Locks > GetDatasetLocksWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling LocksApi->GetDatasetLocks");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling LocksApi->GetDatasetLocks");

            var localVarPath = "/datasets/{datasetId}/locks";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.locks+json",
                "application/vnd.kartverket.ngis.all_locks+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (bbox != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "bbox", bbox)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetDatasetLocks", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Locks>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Locks) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Locks)));
        }

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser. Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of Locks</returns>
        public async System.Threading.Tasks.Task<Locks> GetDatasetLocksAsync (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
             ApiResponse<Locks> localVarResponse = await GetDatasetLocksAsyncWithHttpInfo(xClientProductVersion, datasetId, lockingType, bbox, crsEPSG, normalizedForVisualization);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser. Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of ApiResponse (Locks)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Locks>> GetDatasetLocksAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling LocksApi->GetDatasetLocks");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling LocksApi->GetDatasetLocks");

            var localVarPath = "/datasets/{datasetId}/locks";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.locks+json",
                "application/vnd.kartverket.ngis.all_locks+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (bbox != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "bbox", bbox)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetDatasetLocks", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Locks>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Locks) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Locks)));
        }

    }
}
