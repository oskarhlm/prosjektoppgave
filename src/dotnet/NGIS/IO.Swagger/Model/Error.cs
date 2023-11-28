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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;
namespace IO.Swagger.Model
{
    /// <summary>
    /// Error
    /// </summary>
    [DataContract]
        public partial class Error :  IEquatable<Error>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="title">title (required).</param>
        /// <param name="status">status.</param>
        /// <param name="detail">detail (required).</param>
        /// <param name="instance">instance.</param>
        public Error(string type = default(string), string title = default(string), int? status = default(int?), string detail = default(string), string instance = default(string))
        {
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new InvalidDataException("type is a required property for Error and cannot be null");
            }
            else
            {
                this.Type = type;
            }
            // to ensure "title" is required (not null)
            if (title == null)
            {
                throw new InvalidDataException("title is a required property for Error and cannot be null");
            }
            else
            {
                this.Title = title;
            }
            // to ensure "detail" is required (not null)
            if (detail == null)
            {
                throw new InvalidDataException("detail is a required property for Error and cannot be null");
            }
            else
            {
                this.Detail = detail;
            }
            this.Status = status;
            this.Instance = instance;
        }
        
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public int? Status { get; set; }

        /// <summary>
        /// Gets or Sets Detail
        /// </summary>
        [DataMember(Name="detail", EmitDefaultValue=false)]
        public string Detail { get; set; }

        /// <summary>
        /// Gets or Sets Instance
        /// </summary>
        [DataMember(Name="instance", EmitDefaultValue=false)]
        public string Instance { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Error {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Detail: ").Append(Detail).Append("\n");
            sb.Append("  Instance: ").Append(Instance).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Error);
        }

        /// <summary>
        /// Returns true if Error instances are equal
        /// </summary>
        /// <param name="input">Instance of Error to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Error input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Title == input.Title ||
                    (this.Title != null &&
                    this.Title.Equals(input.Title))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.Detail == input.Detail ||
                    (this.Detail != null &&
                    this.Detail.Equals(input.Detail))
                ) && 
                (
                    this.Instance == input.Instance ||
                    (this.Instance != null &&
                    this.Instance.Equals(input.Instance))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Title != null)
                    hashCode = hashCode * 59 + this.Title.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Detail != null)
                    hashCode = hashCode * 59 + this.Detail.GetHashCode();
                if (this.Instance != null)
                    hashCode = hashCode * 59 + this.Instance.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
