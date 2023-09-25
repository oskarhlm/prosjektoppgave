# swagger_client.FeaturesApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**get_dataset_feature**](FeaturesApi.md#get_dataset_feature) | **GET** /datasets/{datasetId}/features/{lokalId} | Hent ut en bestemt feature fra et dataset
[**get_dataset_feature_attributes**](FeaturesApi.md#get_dataset_feature_attributes) | **GET** /datasets/{datasetId}/features/{lokalId}/attributes | Hent ut egenskapene til en bestemt feature fra et dataset
[**get_dataset_features**](FeaturesApi.md#get_dataset_features) | **GET** /datasets/{datasetId}/features | Hent ut features fra et dataset
[**update_dataset_feature_attributes**](FeaturesApi.md#update_dataset_feature_attributes) | **PUT** /datasets/{datasetId}/features/{lokalId}/attributes | Endre egenskapene til en bestemt feature i et dataset
[**update_dataset_features**](FeaturesApi.md#update_dataset_features) | **POST** /datasets/{datasetId}/features | Endre features i et dataset

# **get_dataset_feature**
> object get_dataset_feature(x_client_product_version, dataset_id, lokal_id, references, locking_type=locking_type, limit=limit, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)

Hent ut en bestemt feature fra et dataset

Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.

### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint
# Configure HTTP basic authorization: basicAuth
configuration = swagger_client.Configuration()
configuration.username = 'YOUR_USERNAME'
configuration.password = 'YOUR_PASSWORD'

# create an instance of the API class
api_instance = swagger_client.FeaturesApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
lokal_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | Identifikasjon.Lokalid til objektet
references = 'references_example' # str | Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses. 
locking_type = 'locking_type_example' # str | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional)
limit = 56 # int | Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`.  (optional)
crs_epsg = 56 # int | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)
normalized_for_visualization = true # bool | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)

try:
    # Hent ut en bestemt feature fra et dataset
    api_response = api_instance.get_dataset_feature(x_client_product_version, dataset_id, lokal_id, references, locking_type=locking_type, limit=limit, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling FeaturesApi->get_dataset_feature: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **lokal_id** | [**str**](.md)| Identifikasjon.Lokalid til objektet | 
 **references** | **str**| Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses.  | 
 **locking_type** | **str**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **limit** | **int**| Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  | [optional] 
 **crs_epsg** | **int**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalized_for_visualization** | **bool**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

**object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+gml; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_dataset_feature_attributes**
> object get_dataset_feature_attributes(dataset_id, lokal_id)

Hent ut egenskapene til en bestemt feature fra et dataset

Henter ut alle egenskapene til en bestemt feature.

### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint
# Configure HTTP basic authorization: basicAuth
configuration = swagger_client.Configuration()
configuration.username = 'YOUR_USERNAME'
configuration.password = 'YOUR_PASSWORD'

# create an instance of the API class
api_instance = swagger_client.FeaturesApi(swagger_client.ApiClient(configuration))
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
lokal_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | Identifikasjon.Lokalid til objektet

try:
    # Hent ut egenskapene til en bestemt feature fra et dataset
    api_response = api_instance.get_dataset_feature_attributes(dataset_id, lokal_id)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling FeaturesApi->get_dataset_feature_attributes: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **lokal_id** | [**str**](.md)| Identifikasjon.Lokalid til objektet | 

### Return type

**object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.attributes+json; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_dataset_features**
> object get_dataset_features(x_client_product_version, dataset_id, references, locking_type=locking_type, bbox=bbox, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization, limit=limit, cursor=cursor, query=query, dataset_at=dataset_at, dataset_modified=dataset_modified)

Hent ut features fra et dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.

### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint
# Configure HTTP basic authorization: basicAuth
configuration = swagger_client.Configuration()
configuration.username = 'YOUR_USERNAME'
configuration.password = 'YOUR_PASSWORD'

# create an instance of the API class
api_instance = swagger_client.FeaturesApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
references = 'references_example' # str | Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses. 
locking_type = 'locking_type_example' # str | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional)
bbox = swagger_client.BoundingBox() # BoundingBox | Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`   (optional)
crs_epsg = 56 # int | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)
normalized_for_visualization = true # bool | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)
limit = 56 # int | Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`.  (optional)
cursor = 'cursor_example' # str | Brukes til porsjonering av data (optional)
query = 'query_example' # str | ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun `eq` og `in` for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  (optional)
dataset_at = '2013-10-20T19:20:30+01:00' # datetime | Angir hvilket tidspunkt (i henhold til ISO 8601, eks. `2020-01-30T12:13:14`) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  (optional)
dataset_modified = 'dataset_modified_example' # str | ### Intervall for uthenting av historiske endringer  Må kombineres med `Accept`-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  Formatet er <fra>/<til> hvor `..` er det samme som hhv. \"fra og med første endring\" eller \"til og med siste endring\".  Hvis man ikke angir `..` må man angi et tidspunkt i henhold til ISO 8601, eks. `2020-01-30T12:13:14`. Man kan også angi høyere oppløsning enn sekund og tidssone med `Z` (betyr UTC) eller offset f.eks `+02:00`.  (optional)

try:
    # Hent ut features fra et dataset
    api_response = api_instance.get_dataset_features(x_client_product_version, dataset_id, references, locking_type=locking_type, bbox=bbox, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization, limit=limit, cursor=cursor, query=query, dataset_at=dataset_at, dataset_modified=dataset_modified)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling FeaturesApi->get_dataset_features: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **references** | **str**| Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  &#x60;none&#x60; - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  &#x60;direct&#x60; - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  &#x60;all&#x60; - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra &#x60;direct&#x60; også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i &#x60;direct&#x60;. Både flaten og alle avgrensningskurvene låses.  | 
 **locking_type** | **str**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **bbox** | [**BoundingBox**](.md)| Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   | [optional] 
 **crs_epsg** | **int**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalized_for_visualization** | **bool**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 
 **limit** | **int**| Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med &#x60;rel&#x3D;next&#x60; i [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [&#x60;Link&#x60;](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med &#x60;rel&#x3D;next&#x60;.  | [optional] 
 **cursor** | **str**| Brukes til porsjonering av data | [optional] 
 **query** | **str**| ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun &#x60;eq&#x60; og &#x60;in&#x60; for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer.  | [optional] 
 **dataset_at** | **datetime**| Angir hvilket tidspunkt (i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  | [optional] 
 **dataset_modified** | **str**| ### Intervall for uthenting av historiske endringer  Må kombineres med &#x60;Accept&#x60;-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks &#x60;crs_EPSG&#x60; osv. Kan foreløpig ikke kombineres med &#x60;query&#x60;, og ved bruk av &#x60;bbox&#x60; kan ikke &#x60;references&#x60; være annet enn &#x60;none&#x60; (hvis ikke angitt blir det satt til &#x60;none&#x60;).  Formatet er &lt;fra&gt;/&lt;til&gt; hvor &#x60;..&#x60; er det samme som hhv. \&quot;fra og med første endring\&quot; eller \&quot;til og med siste endring\&quot;.  Hvis man ikke angir &#x60;..&#x60; må man angi et tidspunkt i henhold til ISO 8601, eks. &#x60;2020-01-30T12:13:14&#x60;. Man kan også angi høyere oppløsning enn sekund og tidssone med &#x60;Z&#x60; (betyr UTC) eller offset f.eks &#x60;+02:00&#x60;.  | [optional] 

### Return type

**object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0, application/vnd.kartverket.sosi+gml; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **update_dataset_feature_attributes**
> object update_dataset_feature_attributes(body, x_client_product_version, dataset_id, lokal_id)

Endre egenskapene til en bestemt feature i et dataset

Oppdaterer alle egenskapene til en bestemt feature i et dataset.

### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint
# Configure HTTP basic authorization: basicAuth
configuration = swagger_client.Configuration()
configuration.username = 'YOUR_USERNAME'
configuration.password = 'YOUR_PASSWORD'

# create an instance of the API class
api_instance = swagger_client.FeaturesApi(swagger_client.ApiClient(configuration))
body = {
  "summary" : "Eksempel på egenskaper til linje",
  "externalValue" : "https://raw.githubusercontent.com/kartverket/SFKB-API/master/spec/examples/linestring/Mønelinje.attributes.json"
} # object | 
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
lokal_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | Identifikasjon.Lokalid til objektet

try:
    # Endre egenskapene til en bestemt feature i et dataset
    api_response = api_instance.update_dataset_feature_attributes(body, x_client_product_version, dataset_id, lokal_id)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling FeaturesApi->update_dataset_feature_attributes: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**object**](object.md)|  | 
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **lokal_id** | [**str**](.md)| Identifikasjon.Lokalid til objektet | 

### Return type

**object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/vnd.kartverket.ngis.attributes+json; version=1.0
 - **Accept**: application/vnd.kartverket.ngis.attributes+json; version=1.0

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **update_dataset_features**
> InlineResponse200 update_dataset_features(body, x_client_product_version, dataset_id, locking_type=locking_type, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization, _async=_async, copy_transaction_token=copy_transaction_token)

Endre features i et dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.

### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint
# Configure HTTP basic authorization: basicAuth
configuration = swagger_client.Configuration()
configuration.username = 'YOUR_USERNAME'
configuration.password = 'YOUR_PASSWORD'

# create an instance of the API class
api_instance = swagger_client.FeaturesApi(swagger_client.ApiClient(configuration))
body = NULL # object | Optional description in *Markdown*
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
locking_type = 'locking_type_example' # str | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional)
crs_epsg = 56 # int | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)
normalized_for_visualization = true # bool | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)
_async = true # bool | Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  (optional)
copy_transaction_token = 'copy_transaction_token_example' # str | Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  (optional)

try:
    # Endre features i et dataset
    api_response = api_instance.update_dataset_features(body, x_client_product_version, dataset_id, locking_type=locking_type, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization, _async=_async, copy_transaction_token=copy_transaction_token)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling FeaturesApi->update_dataset_features: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**object**](object.md)| Optional description in *Markdown* | 
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **locking_type** | **str**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **crs_epsg** | **int**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalized_for_visualization** | **bool**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 
 **_async** | **bool**| Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder.  | [optional] 
 **copy_transaction_token** | **str**| Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet  | [optional] 

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/vnd.kartverket.sosi+wfs-t; version=1.0, application/vnd.kartverket.geosynkronisering+zip; version=1.0, application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0
 - **Accept**: application/vnd.kartverket.ngis.edit_features_summary+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

