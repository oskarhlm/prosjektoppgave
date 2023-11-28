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
        public interface IFeaturesApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Hent ut en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Object</returns>
        Object GetDatasetFeature (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null);

        /// <summary>
        /// Hent ut en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> GetDatasetFeatureWithHttpInfo (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null);
        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut alle egenskapene til en bestemt feature.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Object</returns>
        Object GetDatasetFeatureAttributes (Guid? datasetId, Guid? lokalId);

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut alle egenskapene til en bestemt feature.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> GetDatasetFeatureAttributesWithHttpInfo (Guid? datasetId, Guid? lokalId);
        /// <summary>
        /// Hent ut features fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Object</returns>
        Object GetDatasetFeatures (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null);

        /// <summary>
        /// Hent ut features fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> GetDatasetFeaturesWithHttpInfo (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null);
        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset
        /// </summary>
        /// <remarks>
        /// Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Object</returns>
        Object UpdateDatasetFeatureAttributes (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId);

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset
        /// </summary>
        /// <remarks>
        /// Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> UpdateDatasetFeatureAttributesWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId);
        /// <summary>
        /// Endre features i et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>InlineResponse200</returns>
        InlineResponse200 UpdateDatasetFeatures (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null);

        /// <summary>
        /// Endre features i et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>ApiResponse of InlineResponse200</returns>
        ApiResponse<InlineResponse200> UpdateDatasetFeaturesWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Hent ut en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> GetDatasetFeatureAsync (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null);

        /// <summary>
        /// Hent ut en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeatureAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null);
        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut alle egenskapene til en bestemt feature.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> GetDatasetFeatureAttributesAsync (Guid? datasetId, Guid? lokalId);

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter ut alle egenskapene til en bestemt feature.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeatureAttributesAsyncWithHttpInfo (Guid? datasetId, Guid? lokalId);
        /// <summary>
        /// Hent ut features fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> GetDatasetFeaturesAsync (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null);

        /// <summary>
        /// Hent ut features fra et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeaturesAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null);
        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset
        /// </summary>
        /// <remarks>
        /// Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> UpdateDatasetFeatureAttributesAsync (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId);

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset
        /// </summary>
        /// <remarks>
        /// Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> UpdateDatasetFeatureAttributesAsyncWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId);
        /// <summary>
        /// Endre features i et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>Task of InlineResponse200</returns>
        System.Threading.Tasks.Task<InlineResponse200> UpdateDatasetFeaturesAsync (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null);

        /// <summary>
        /// Endre features i et dataset
        /// </summary>
        /// <remarks>
        /// Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>Task of ApiResponse (InlineResponse200)</returns>
        System.Threading.Tasks.Task<ApiResponse<InlineResponse200>> UpdateDatasetFeaturesAsyncWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
        public partial class FeaturesApi : IFeaturesApi
    {
        private IO.Swagger.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FeaturesApi(String basePath)
        {
            this.Configuration = new IO.Swagger.Client.Configuration { BasePath = basePath };

            ExceptionFactory = IO.Swagger.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesApi"/> class
        /// </summary>
        /// <returns></returns>
        public FeaturesApi()
        {
            this.Configuration = IO.Swagger.Client.Configuration.Default;

            ExceptionFactory = IO.Swagger.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public FeaturesApi(IO.Swagger.Client.Configuration configuration = null)
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
        /// Hent ut en bestemt feature fra et dataset Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Object</returns>
        public Object GetDatasetFeature (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
             ApiResponse<Object> localVarResponse = GetDatasetFeatureWithHttpInfo(xClientProductVersion, datasetId, lokalId, references, lockingType, limit, crsEPSG, normalizedForVisualization);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Hent ut en bestemt feature fra et dataset Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>ApiResponse of Object</returns>
        public ApiResponse< Object > GetDatasetFeatureWithHttpInfo (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'references' is set
            if (references == null)
                throw new ApiException(400, "Missing required parameter 'references' when calling FeaturesApi->GetDatasetFeature");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}";
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
                "application/vnd.kartverket.sosi+json; version=1.0",
                "application/vnd.kartverket.sosi+gml; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (references != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "references", references)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeature", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Hent ut en bestemt feature fra et dataset Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> GetDatasetFeatureAsync (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
             ApiResponse<Object> localVarResponse = await GetDatasetFeatureAsyncWithHttpInfo(xClientProductVersion, datasetId, lokalId, references, lockingType, limit, crsEPSG, normalizedForVisualization);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Hent ut en bestemt feature fra et dataset Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeatureAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->GetDatasetFeature");
            // verify the required parameter 'references' is set
            if (references == null)
                throw new ApiException(400, "Missing required parameter 'references' when calling FeaturesApi->GetDatasetFeature");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}";
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
                "application/vnd.kartverket.sosi+json; version=1.0",
                "application/vnd.kartverket.sosi+gml; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (references != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "references", references)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeature", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset Henter ut alle egenskapene til en bestemt feature.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Object</returns>
        public Object GetDatasetFeatureAttributes (Guid? datasetId, Guid? lokalId)
        {
             ApiResponse<Object> localVarResponse = GetDatasetFeatureAttributesWithHttpInfo(datasetId, lokalId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset Henter ut alle egenskapene til en bestemt feature.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>ApiResponse of Object</returns>
        public ApiResponse< Object > GetDatasetFeatureAttributesWithHttpInfo (Guid? datasetId, Guid? lokalId)
        {
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeatureAttributes");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->GetDatasetFeatureAttributes");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}/attributes";
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
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeatureAttributes", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset Henter ut alle egenskapene til en bestemt feature.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> GetDatasetFeatureAttributesAsync (Guid? datasetId, Guid? lokalId)
        {
             ApiResponse<Object> localVarResponse = await GetDatasetFeatureAttributesAsyncWithHttpInfo(datasetId, lokalId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Hent ut egenskapene til en bestemt feature fra et dataset Henter ut alle egenskapene til en bestemt feature.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeatureAttributesAsyncWithHttpInfo (Guid? datasetId, Guid? lokalId)
        {
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeatureAttributes");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->GetDatasetFeatureAttributes");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}/attributes";
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
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeatureAttributes", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Hent ut features fra et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Object</returns>
        public Object GetDatasetFeatures (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null)
        {
             ApiResponse<Object> localVarResponse = GetDatasetFeaturesWithHttpInfo(xClientProductVersion, datasetId, references, lockingType, bbox, crsEPSG, normalizedForVisualization, limit, cursor, query, datasetAt, datasetModified);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Hent ut features fra et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>ApiResponse of Object</returns>
        public ApiResponse< Object > GetDatasetFeaturesWithHttpInfo (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->GetDatasetFeatures");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeatures");
            // verify the required parameter 'references' is set
            if (references == null)
                throw new ApiException(400, "Missing required parameter 'references' when calling FeaturesApi->GetDatasetFeatures");

            var localVarPath = "/datasets/{datasetId}/features";
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
                "application/vnd.kartverket.sosi+json; version=1.0",
                "application/vnd.kartverket.sosi+json; version=2.0",
                "application/vnd.kartverket.sosi+gml; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (bbox != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "bbox", bbox)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (references != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "references", references)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
            if (cursor != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "cursor", cursor)); // query parameter
            if (query != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "query", query)); // query parameter
            if (datasetAt != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "dataset_at", datasetAt)); // query parameter
            if (datasetModified != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "dataset_modified", datasetModified)); // query parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeatures", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Hent ut features fra et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> GetDatasetFeaturesAsync (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null)
        {
             ApiResponse<Object> localVarResponse = await GetDatasetFeaturesAsyncWithHttpInfo(xClientProductVersion, datasetId, references, lockingType, bbox, crsEPSG, normalizedForVisualization, limit, cursor, query, datasetAt, datasetModified);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Hent ut features fra et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="references">Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses. </param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="bbox">Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="limit">Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  (optional)</param>
        /// <param name="cursor">Brukes til porsjonering av data (optional)</param>
        /// <param name="query">### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)</param>
        /// <param name="datasetAt">Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  (optional)</param>
        /// <param name="datasetModified">### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  (optional)</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> GetDatasetFeaturesAsyncWithHttpInfo (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null)
        {
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->GetDatasetFeatures");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->GetDatasetFeatures");
            // verify the required parameter 'references' is set
            if (references == null)
                throw new ApiException(400, "Missing required parameter 'references' when calling FeaturesApi->GetDatasetFeatures");

            var localVarPath = "/datasets/{datasetId}/features";
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
                "application/vnd.kartverket.sosi+json; version=1.0",
                "application/vnd.kartverket.sosi+json; version=2.0",
                "application/vnd.kartverket.sosi+gml; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (bbox != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "bbox", bbox)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (references != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "references", references)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
            if (cursor != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "cursor", cursor)); // query parameter
            if (query != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "query", query)); // query parameter
            if (datasetAt != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "dataset_at", datasetAt)); // query parameter
            if (datasetModified != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "dataset_modified", datasetModified)); // query parameter
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
                Exception exception = ExceptionFactory("GetDatasetFeatures", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Object</returns>
        public Object UpdateDatasetFeatureAttributes (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId)
        {
             ApiResponse<Object> localVarResponse = UpdateDatasetFeatureAttributesWithHttpInfo(body, xClientProductVersion, datasetId, lokalId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>ApiResponse of Object</returns>
        public ApiResponse< Object > UpdateDatasetFeatureAttributesWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId)
        {
            // verify the required parameter 'body' is set
            if (body == null)
                throw new ApiException(400, "Missing required parameter 'body' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->UpdateDatasetFeatureAttributes");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}/attributes";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("UpdateDatasetFeatureAttributes", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> UpdateDatasetFeatureAttributesAsync (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId)
        {
             ApiResponse<Object> localVarResponse = await UpdateDatasetFeatureAttributesAsyncWithHttpInfo(body, xClientProductVersion, datasetId, lokalId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Endre egenskapene til en bestemt feature i et dataset Oppdaterer alle egenskapene til en bestemt feature i et dataset.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"></param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lokalId">Identifikasjon.Lokalid til objektet</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> UpdateDatasetFeatureAttributesAsyncWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId)
        {
            // verify the required parameter 'body' is set
            if (body == null)
                throw new ApiException(400, "Missing required parameter 'body' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->UpdateDatasetFeatureAttributes");
            // verify the required parameter 'lokalId' is set
            if (lokalId == null)
                throw new ApiException(400, "Missing required parameter 'lokalId' when calling FeaturesApi->UpdateDatasetFeatureAttributes");

            var localVarPath = "/datasets/{datasetId}/features/{lokalId}/attributes";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.attributes+json; version=1.0"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lokalId != null) localVarPathParams.Add("lokalId", this.Configuration.ApiClient.ParameterToString(lokalId)); // path parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("UpdateDatasetFeatureAttributes", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Object) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Object)));
        }

        /// <summary>
        /// Endre features i et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>InlineResponse200</returns>
        public InlineResponse200 UpdateDatasetFeatures (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null)
        {
             ApiResponse<InlineResponse200> localVarResponse = UpdateDatasetFeaturesWithHttpInfo(body, xClientProductVersion, datasetId, lockingType, crsEPSG, normalizedForVisualization, async, copyTransactionToken);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Endre features i et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>ApiResponse of InlineResponse200</returns>
        public ApiResponse< InlineResponse200 > UpdateDatasetFeaturesWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null)
        {
            // verify the required parameter 'body' is set
            if (body == null)
                throw new ApiException(400, "Missing required parameter 'body' when calling FeaturesApi->UpdateDatasetFeatures");
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->UpdateDatasetFeatures");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->UpdateDatasetFeatures");

            var localVarPath = "/datasets/{datasetId}/features";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.kartverket.sosi+wfs-t; version=1.0", 
                "application/vnd.kartverket.geosynkronisering+zip; version=1.0", 
                "application/vnd.kartverket.sosi+json; version=1.0", 
                "application/vnd.kartverket.sosi+json; version=2.0"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.edit_features_summary+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (async != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "async", async)); // query parameter
            if (copyTransactionToken != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "copy_transaction_token", copyTransactionToken)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("UpdateDatasetFeatures", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InlineResponse200>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InlineResponse200) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InlineResponse200)));
        }

        /// <summary>
        /// Endre features i et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>Task of InlineResponse200</returns>
        public async System.Threading.Tasks.Task<InlineResponse200> UpdateDatasetFeaturesAsync (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null)
        {
             ApiResponse<InlineResponse200> localVarResponse = await UpdateDatasetFeaturesAsyncWithHttpInfo(body, xClientProductVersion, datasetId, lockingType, crsEPSG, normalizedForVisualization, async, copyTransactionToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Endre features i et dataset Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Optional description in *Markdown*</param>
        /// <param name="xClientProductVersion">Brukes for å kunne identifisere klienten som er brukt</param>
        /// <param name="datasetId">UUID of the dataset to get</param>
        /// <param name="lockingType">Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  (optional)</param>
        /// <param name="crsEPSG">Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)</param>
        /// <param name="normalizedForVisualization">Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)</param>
        /// <param name="async">Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)</param>
        /// <param name="copyTransactionToken">Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)</param>
        /// <returns>Task of ApiResponse (InlineResponse200)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InlineResponse200>> UpdateDatasetFeaturesAsyncWithHttpInfo (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null)
        {
            // verify the required parameter 'body' is set
            if (body == null)
                throw new ApiException(400, "Missing required parameter 'body' when calling FeaturesApi->UpdateDatasetFeatures");
            // verify the required parameter 'xClientProductVersion' is set
            if (xClientProductVersion == null)
                throw new ApiException(400, "Missing required parameter 'xClientProductVersion' when calling FeaturesApi->UpdateDatasetFeatures");
            // verify the required parameter 'datasetId' is set
            if (datasetId == null)
                throw new ApiException(400, "Missing required parameter 'datasetId' when calling FeaturesApi->UpdateDatasetFeatures");

            var localVarPath = "/datasets/{datasetId}/features";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.kartverket.sosi+wfs-t; version=1.0", 
                "application/vnd.kartverket.geosynkronisering+zip; version=1.0", 
                "application/vnd.kartverket.sosi+json; version=1.0", 
                "application/vnd.kartverket.sosi+json; version=2.0"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.kartverket.ngis.edit_features_summary+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (datasetId != null) localVarPathParams.Add("datasetId", this.Configuration.ApiClient.ParameterToString(datasetId)); // path parameter
            if (lockingType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "locking_type", lockingType)); // query parameter
            if (crsEPSG != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "crs_EPSG", crsEPSG)); // query parameter
            if (normalizedForVisualization != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "normalized_for_visualization", normalizedForVisualization)); // query parameter
            if (async != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "async", async)); // query parameter
            if (copyTransactionToken != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "copy_transaction_token", copyTransactionToken)); // query parameter
            if (xClientProductVersion != null) localVarHeaderParams.Add("X-Client-Product-Version", this.Configuration.ApiClient.ParameterToString(xClientProductVersion)); // header parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }
            // authentication (basicAuth) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("UpdateDatasetFeatures", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InlineResponse200>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InlineResponse200) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InlineResponse200)));
        }

    }
}
