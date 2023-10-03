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
    /// Dataset
    /// </summary>
    [DataContract]
        public partial class Dataset :  IEquatable<Dataset>, IValidatableObject
    {
        /// <summary>
        /// Defines Access
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
                public enum AccessEnum
        {
            /// <summary>
            /// Enum Only for value: read_only
            /// </summary>
            [EnumMember(Value = "read_only")]
            Only = 1,
            /// <summary>
            /// Enum Write for value: read_write
            /// </summary>
            [EnumMember(Value = "read_write")]
            Write = 2        }
        /// <summary>
        /// Gets or Sets Access
        /// </summary>
        [DataMember(Name="access", EmitDefaultValue=false)]
        public AccessEnum? Access { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Dataset" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="name">name.</param>
        /// <param name="description">description.</param>
        /// <param name="resolution">resolution.</param>
        /// <param name="crsEPSG">crsEPSG.</param>
        /// <param name="datasetLastModified">datasetLastModified.</param>
        /// <param name="schemaUrl">schemaUrl.</param>
        /// <param name="access">access.</param>
        /// <param name="bbox">bbox.</param>
        /// <param name="copyTransactionToken">copyTransactionToken.</param>
        public Dataset(Guid? id = default(Guid?), string name = default(string), string description = default(string), double? resolution = default(double?), int? crsEPSG = default(int?), string datasetLastModified = default(string), string schemaUrl = default(string), AccessEnum? access = default(AccessEnum?), BoundingBox bbox = default(BoundingBox), string copyTransactionToken = default(string))
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Resolution = resolution;
            this.CrsEPSG = crsEPSG;
            this.DatasetLastModified = datasetLastModified;
            this.SchemaUrl = schemaUrl;
            this.Access = access;
            this.Bbox = bbox;
            this.CopyTransactionToken = copyTransactionToken;
        }
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Resolution
        /// </summary>
        [DataMember(Name="resolution", EmitDefaultValue=false)]
        public double? Resolution { get; set; }

        /// <summary>
        /// Gets or Sets CrsEPSG
        /// </summary>
        [DataMember(Name="crs_EPSG", EmitDefaultValue=false)]
        public int? CrsEPSG { get; set; }

        /// <summary>
        /// Gets or Sets DatasetLastModified
        /// </summary>
        [DataMember(Name="dataset_last_modified", EmitDefaultValue=false)]
        public string DatasetLastModified { get; set; }

        /// <summary>
        /// Gets or Sets SchemaUrl
        /// </summary>
        [DataMember(Name="schema_url", EmitDefaultValue=false)]
        public string SchemaUrl { get; set; }


        /// <summary>
        /// Gets or Sets Bbox
        /// </summary>
        [DataMember(Name="bbox", EmitDefaultValue=false)]
        public BoundingBox Bbox { get; set; }

        /// <summary>
        /// Gets or Sets CopyTransactionToken
        /// </summary>
        [DataMember(Name="copy_transaction_token", EmitDefaultValue=false)]
        public string CopyTransactionToken { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Dataset {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Resolution: ").Append(Resolution).Append("\n");
            sb.Append("  CrsEPSG: ").Append(CrsEPSG).Append("\n");
            sb.Append("  DatasetLastModified: ").Append(DatasetLastModified).Append("\n");
            sb.Append("  SchemaUrl: ").Append(SchemaUrl).Append("\n");
            sb.Append("  Access: ").Append(Access).Append("\n");
            sb.Append("  Bbox: ").Append(Bbox).Append("\n");
            sb.Append("  CopyTransactionToken: ").Append(CopyTransactionToken).Append("\n");
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
            return this.Equals(input as Dataset);
        }

        /// <summary>
        /// Returns true if Dataset instances are equal
        /// </summary>
        /// <param name="input">Instance of Dataset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Dataset input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.Resolution == input.Resolution ||
                    (this.Resolution != null &&
                    this.Resolution.Equals(input.Resolution))
                ) && 
                (
                    this.CrsEPSG == input.CrsEPSG ||
                    (this.CrsEPSG != null &&
                    this.CrsEPSG.Equals(input.CrsEPSG))
                ) && 
                (
                    this.DatasetLastModified == input.DatasetLastModified ||
                    (this.DatasetLastModified != null &&
                    this.DatasetLastModified.Equals(input.DatasetLastModified))
                ) && 
                (
                    this.SchemaUrl == input.SchemaUrl ||
                    (this.SchemaUrl != null &&
                    this.SchemaUrl.Equals(input.SchemaUrl))
                ) && 
                (
                    this.Access == input.Access ||
                    (this.Access != null &&
                    this.Access.Equals(input.Access))
                ) && 
                (
                    this.Bbox == input.Bbox ||
                    (this.Bbox != null &&
                    this.Bbox.Equals(input.Bbox))
                ) && 
                (
                    this.CopyTransactionToken == input.CopyTransactionToken ||
                    (this.CopyTransactionToken != null &&
                    this.CopyTransactionToken.Equals(input.CopyTransactionToken))
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
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.Resolution != null)
                    hashCode = hashCode * 59 + this.Resolution.GetHashCode();
                if (this.CrsEPSG != null)
                    hashCode = hashCode * 59 + this.CrsEPSG.GetHashCode();
                if (this.DatasetLastModified != null)
                    hashCode = hashCode * 59 + this.DatasetLastModified.GetHashCode();
                if (this.SchemaUrl != null)
                    hashCode = hashCode * 59 + this.SchemaUrl.GetHashCode();
                if (this.Access != null)
                    hashCode = hashCode * 59 + this.Access.GetHashCode();
                if (this.Bbox != null)
                    hashCode = hashCode * 59 + this.Bbox.GetHashCode();
                if (this.CopyTransactionToken != null)
                    hashCode = hashCode * 59 + this.CopyTransactionToken.GetHashCode();
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
