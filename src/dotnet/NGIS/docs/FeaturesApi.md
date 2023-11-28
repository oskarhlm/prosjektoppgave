# IO.Swagger.Api.FeaturesApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetDatasetFeature**](FeaturesApi.md#getdatasetfeature) | **GET** /datasets/{datasetId}/features/{lokalId} | Hent ut en bestemt feature fra et dataset
[**GetDatasetFeatureAttributes**](FeaturesApi.md#getdatasetfeatureattributes) | **GET** /datasets/{datasetId}/features/{lokalId}/attributes | Hent ut egenskapene til en bestemt feature fra et dataset
[**GetDatasetFeatures**](FeaturesApi.md#getdatasetfeatures) | **GET** /datasets/{datasetId}/features | Hent ut features fra et dataset
[**UpdateDatasetFeatureAttributes**](FeaturesApi.md#updatedatasetfeatureattributes) | **PUT** /datasets/{datasetId}/features/{lokalId}/attributes | Endre egenskapene til en bestemt feature i et dataset
[**UpdateDatasetFeatures**](FeaturesApi.md#updatedatasetfeatures) | **POST** /datasets/{datasetId}/features | Endre features i et dataset

<a name="getdatasetfeature"></a>
# **GetDatasetFeature**
> Object GetDatasetFeature (string xClientProductVersion, Guid? datasetId, Guid? lokalId, string references, string lockingType = null, int? limit = null, int? crsEPSG = null, bool? normalizedForVisualization = null)

Hent ut en bestemt feature fra et dataset

Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetFeatureExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new FeaturesApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lokalId = new Guid?(); // Guid? | Identifikasjon.Lokalid til objektet
            var references = references_example;  // string | Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses. 
            var lockingType = lockingType_example;  // string | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional) 
            var limit = 56;  // int? | Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`.  (optional) 
            var crsEPSG = 56;  // int? | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional) 
            var normalizedForVisualization = true;  // bool? | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional) 

            try
            {
                // Hent ut en bestemt feature fra et dataset
                Object result = apiInstance.GetDatasetFeature(xClientProductVersion, datasetId, lokalId, references, lockingType, limit, crsEPSG, normalizedForVisualization);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeaturesApi.GetDatasetFeature: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 
 **lokalId** | [**Guid?**](Guid?.md)| Identifikasjon.Lokalid til objektet | 
 **references** | **string**| Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses.  | 
 **lockingType** | **string**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **limit** | **int?**| Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  | [optional] 
 **crsEPSG** | **int?**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalizedForVisualization** | **bool?**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

**Object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+gml; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="getdatasetfeatureattributes"></a>
# **GetDatasetFeatureAttributes**
> Object GetDatasetFeatureAttributes (Guid? datasetId, Guid? lokalId)

Hent ut egenskapene til en bestemt feature fra et dataset

Henter ut alle egenskapene til en bestemt feature.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetFeatureAttributesExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new FeaturesApi();
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lokalId = new Guid?(); // Guid? | Identifikasjon.Lokalid til objektet

            try
            {
                // Hent ut egenskapene til en bestemt feature fra et dataset
                Object result = apiInstance.GetDatasetFeatureAttributes(datasetId, lokalId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeaturesApi.GetDatasetFeatureAttributes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 
 **lokalId** | [**Guid?**](Guid?.md)| Identifikasjon.Lokalid til objektet | 

### Return type

**Object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.attributes+json; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="getdatasetfeatures"></a>
# **GetDatasetFeatures**
> Object GetDatasetFeatures (string xClientProductVersion, Guid? datasetId, string references, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null, int? limit = null, string cursor = null, string query = null, DateTime? datasetAt = null, string datasetModified = null)

Hent ut features fra et dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetFeaturesExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new FeaturesApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var references = references_example;  // string | Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses. 
            var lockingType = lockingType_example;  // string | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional) 
            var bbox = new BoundingBox(); // BoundingBox | Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`   (optional) 
            var crsEPSG = 56;  // int? | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional) 
            var normalizedForVisualization = true;  // bool? | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional) 
            var limit = 56;  // int? | Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`.  (optional) 
            var cursor = cursor_example;  // string | Brukes til porsjonering av data (optional) 
            var query = query_example;  // string | ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun `eq` og `in` for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional) 
            var datasetAt = 2013-10-20T19:20:30+01:00;  // DateTime? | Angir hvilket tidspunkt (i henhold til ISO 8601, eks. `2020-01-30T12:13:14`) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  (optional) 
            var datasetModified = datasetModified_example;  // string | ### Intervall for uthenting av historiske endringer  Må kombineres med `Accept`-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  Formatet er <fra>/<til> hvor `..` er det samme som hhv. \"fra og med første endring\" eller \"til og med siste endring\".  Hvis man ikke angir `..` må man angi et tidspunkt i henhold til ISO 8601, eks. `2020-01-30T12:13:14`. Man kan også angi høyere oppløsning enn sekund og tidssone med `Z` (betyr UTC) eller offset f.eks `+02:00`.  (optional) 

            try
            {
                // Hent ut features fra et dataset
                Object result = apiInstance.GetDatasetFeatures(xClientProductVersion, datasetId, references, lockingType, bbox, crsEPSG, normalizedForVisualization, limit, cursor, query, datasetAt, datasetModified);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeaturesApi.GetDatasetFeatures: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 
 **references** | **string**| Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses.  | 
 **lockingType** | **string**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **bbox** | [**BoundingBox**](BoundingBox.md)| Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   | [optional] 
 **crsEPSG** | **int?**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalizedForVisualization** | **bool?**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 
 **limit** | **int?**| Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  | [optional] 
 **cursor** | **string**| Brukes til porsjonering av data | [optional] 
 **query** | **string**| ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  | [optional] 
 **datasetAt** | **DateTime?**| Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  | [optional] 
 **datasetModified** | **string**| ### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  | [optional] 

### Return type

**Object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0, application/vnd.kartverket.sosi+gml; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="updatedatasetfeatureattributes"></a>
# **UpdateDatasetFeatureAttributes**
> Object UpdateDatasetFeatureAttributes (Object body, string xClientProductVersion, Guid? datasetId, Guid? lokalId)

Endre egenskapene til en bestemt feature i et dataset

Oppdaterer alle egenskapene til en bestemt feature i et dataset.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateDatasetFeatureAttributesExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new FeaturesApi();
            var body = new Object(); // Object | 
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lokalId = new Guid?(); // Guid? | Identifikasjon.Lokalid til objektet

            try
            {
                // Endre egenskapene til en bestemt feature i et dataset
                Object result = apiInstance.UpdateDatasetFeatureAttributes(body, xClientProductVersion, datasetId, lokalId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeaturesApi.UpdateDatasetFeatureAttributes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Object**](Object.md)|  | 
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 
 **lokalId** | [**Guid?**](Guid?.md)| Identifikasjon.Lokalid til objektet | 

### Return type

**Object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/vnd.kartverket.ngis.attributes+json; version=1.0
 - **Accept**: application/vnd.kartverket.ngis.attributes+json; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="updatedatasetfeatures"></a>
# **UpdateDatasetFeatures**
> InlineResponse200 UpdateDatasetFeatures (Object body, string xClientProductVersion, Guid? datasetId, string lockingType = null, int? crsEPSG = null, bool? normalizedForVisualization = null, bool? async = null, string copyTransactionToken = null)

Endre features i et dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateDatasetFeaturesExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new FeaturesApi();
            var body = new Object(); // Object | Optional description in *Markdown*
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lockingType = lockingType_example;  // string | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional) 
            var crsEPSG = 56;  // int? | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional) 
            var normalizedForVisualization = true;  // bool? | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional) 
            var async = true;  // bool? | Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional) 
            var copyTransactionToken = copyTransactionToken_example;  // string | Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional) 

            try
            {
                // Endre features i et dataset
                InlineResponse200 result = apiInstance.UpdateDatasetFeatures(body, xClientProductVersion, datasetId, lockingType, crsEPSG, normalizedForVisualization, async, copyTransactionToken);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeaturesApi.UpdateDatasetFeatures: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Object**](Object.md)| Optional description in *Markdown* | 
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 
 **lockingType** | **string**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **crsEPSG** | **int?**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalizedForVisualization** | **bool?**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 
 **async** | **bool?**| Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  | [optional] 
 **copyTransactionToken** | **string**| Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  | [optional] 

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/vnd.kartverket.sosi+wfs-t; version=1.0, application/vnd.kartverket.geosynkronisering+zip; version=1.0, application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0
 - **Accept**: application/vnd.kartverket.ngis.edit_features_summary+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
