# swagger-client
# NGIS-OpenAPI  Grov oversikt over funksjonalitet:   - Hente liste over tilgjengelige datasett    - Hente metadata for et bestemt datasett   - Hente data fra et bestemt datasett     - Med lesetilgang eller skrivetilgang (medfører låsing)       - områdebegrensning       - egenskapsspørring (begrenset i første versjon til bygningsnummer eller lokalid)   - Lagre data til et bestemt datasett     - Operasjoner som håndteres: nytt objekt, endre objekt og slett objekt  ## Generelle prinsipper for systemet  ### Versjonering og bakoverkompatibilitet  #### Versjonsnummer i URL  Vi har et versjonsnummer `v1` i URL for å gjøre det mulig å gjøre store endringer i APIet hvis det blir nødvendig, men i utgangspunktet ønsker vi å unngå å endre dette versjonsnummeret.  #### Versjonsnummer i media types (\"media type versioning\", content negotiation)  APIet baserer seg på standard [HTTP content negotiation](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept) ved utveksling av data med headerne `Accept` og `Content-Type`. Dette gjør det veldig enkelt å introdusere nye dataformater i APIet uten endringer for eksisterende klienter. I tillegg til dette inneholder også alle dataformater et versjonsparameter, eks. `version=1.0`, der klienten kan styre hvilket  eller hvilke dataformater klienten kan håndtere. Dataformater angitt uten versjonsparameter vil tolkes som å be om siste versjon.  `Accept: application/vnd.kartverket.sosi+json; version=1.0` Klienten ønsker svar med versjon 1 av dataformatet      `Accept: application/vnd.kartverket.sosi+json; version=2.0` Klienten ønsker svar med versjon 2 av dataformatet  `Accept: application/vnd.kartverket.sosi+json` Klienten ønsker svar med siste versjon av dataformatet  `Accept: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0` Klienten håndterer både versjon 1.0 og 2.0 av dataformatet, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes  `Accept: */*` Klienten håndterer alle dataformater, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes.  Der man kan velge mellom flere helt ulike dataformater som f.eks GML og JSON, må man faktisk håndtere begge.  ### Delt geometri  Flater består av avgrensningslinjer som ligger lagret som egne objekter. På den måten kan en linje avgrense ingen, én eller flere flater. Det er likevel slik at flater hentes ut og lagres med egen geometri for å gjøre det enklere å tegne opp datene, men ved endring av (delte) linjer og flater må det tas hensyn til delt geometri. Forsøk på endring av linje eller flate uten tilsvarende endring av evt. delt geometri vil bli avvist av systemet.  ### Låsing  Dette er nærmere beskrevet i de aktuelle kallene.  Foreløpig er det kun `user_lock` som er støttet. Det betyr at data må hentes ut med `user_lock` før de kan sendes inn med endringer.  ### Porsjonering  All uthenting av feature-objekter vil kunne bli porsjonert av serveren, se `limit`-parameteret.   ### Koordinatsystemer og transformasjon  Dersom annet koordinatsystem enn det som ligger i dataset skal brukes (se `GET /datasets/{datasetId}`) må koordinatsystem angis med `crs_EPSG`-parameteret. Dette styrer data som sendes inn, data som hentes ut og koordinatsystemet i `bbox`-parameteret i kallet. For å bytte rekkefølge på aksene brukes `crs_normalized_for_visualization`-parameteret.  ### Historikk og historiske endringer  Det er mulig å hente ut data for et gitt tidspunkt (for hele datasettet eller begrenset til et område, et objekt etc.). Se etter parameteret `dataset_at`.  Det er også mulig å hente ut historikken som endringer, f.eks som endringer fra et tidligere uthentet område, objekt eller helt dataset. Se etter parameteret `dataset_modified`.  ### FKB5 og QMS13  I forbindelse med FKB5 har det blitt gjort endringer i GeoJSON-formatet som benyttes i NGIS-OpenAPI. Endringene gjelder ved bruk av Versjon 2 av formatet,  som er påkrevd versjon for FKB 5.  Endringer i GeoJSON-formatet, Versjon 2:   - Noden `geometry_properties` er flyttet under `properties`   - Formatet støtter assosiasjoner mellom objekter og geometri-assosiasjoner ved flater med delt geometri   - Formatet støtter heleid geometri  #### Assosiasjoner mellom objekter Et objekt kan ha en assosiasjon til ett eller flere andre objekter. Assosiasjonene av samme type ligger i en array med `lokalId` og `featuretype` til det assosierte objektet.  Flater kan være modellert med krav om delt geometri for flateavgrensningen. Dette angis via geometri-assosiasjoner fra flate-objektet til linjene-objektene som avgrenser flata.  I tillegg til assosiasjons-informasjonen beskrevet over, har geometri-assosiasjonene følgende egenskaper for hvert assosiert objekt:      `reverse` er en bool som forteller om linjas retning skal snus eller ikke for å danne en sammenhengende flateavgrensing med de andre avgrensningslinjene.  `idx` er ett array med tre indekser som gir informasjon om avgrensningslinja:    1. angir hvilken flate linja tilhører (aktuell ved framtidig bruk av MultiSurface), skal for enkeltflater være 0    2. angir om linja tilhører ytre eller en indre avgrensning (hull) og eventuelt hvilken indre avgrensning linja tilhører       - Ytre avgrensning: 0       - Indre avgrensning/hull: 1..n    3. angir hvilken rekkefølge linja har i avgrensningen av flata som dannes av alle avgrensninslinjene (starter på 0) 

This Python package is automatically generated by the [Swagger Codegen](https://github.com/swagger-api/swagger-codegen) project:

- API version: 1.0.0
- Package version: 1.0.0
- Build package: io.swagger.codegen.v3.generators.python.PythonClientCodegen
For more information, please visit [https://www.kartverket.no/Prosjekter/Sentral-felles-kartdatabase/brukerstotte/](https://www.kartverket.no/Prosjekter/Sentral-felles-kartdatabase/brukerstotte/)

## Requirements.

Python 2.7 and 3.4+

## Installation & Usage
### pip install

If the python package is hosted on Github, you can install directly from Github

```sh
pip install git+https://github.com/GIT_USER_ID/GIT_REPO_ID.git
```
(you may need to run `pip` with root permission: `sudo pip install git+https://github.com/GIT_USER_ID/GIT_REPO_ID.git`)

Then import the package:
```python
import swagger_client 
```

### Setuptools

Install via [Setuptools](http://pypi.python.org/pypi/setuptools).

```sh
python setup.py install --user
```
(or `sudo python setup.py install` to install the package for all users)

Then import the package:
```python
import swagger_client
```

## Getting Started

Please follow the [installation procedure](#installation--usage) and then run the following:

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

## Documentation for API Endpoints

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*FeaturesApi* | [**get_dataset_feature**](docs/FeaturesApi.md#get_dataset_feature) | **GET** /datasets/{datasetId}/features/{lokalId} | Hent ut en bestemt feature fra et dataset
*FeaturesApi* | [**get_dataset_feature_attributes**](docs/FeaturesApi.md#get_dataset_feature_attributes) | **GET** /datasets/{datasetId}/features/{lokalId}/attributes | Hent ut egenskapene til en bestemt feature fra et dataset
*FeaturesApi* | [**get_dataset_features**](docs/FeaturesApi.md#get_dataset_features) | **GET** /datasets/{datasetId}/features | Hent ut features fra et dataset
*FeaturesApi* | [**update_dataset_feature_attributes**](docs/FeaturesApi.md#update_dataset_feature_attributes) | **PUT** /datasets/{datasetId}/features/{lokalId}/attributes | Endre egenskapene til en bestemt feature i et dataset
*FeaturesApi* | [**update_dataset_features**](docs/FeaturesApi.md#update_dataset_features) | **POST** /datasets/{datasetId}/features | Endre features i et dataset
*LocksApi* | [**delete_dataset_locks**](docs/LocksApi.md#delete_dataset_locks) | **DELETE** /datasets/{datasetId}/locks | Fjerne alle låser brukeren har i et bestemt dataset
*LocksApi* | [**get_dataset_locks**](docs/LocksApi.md#get_dataset_locks) | **GET** /datasets/{datasetId}/locks | Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.
*MetadataApi* | [**get_dataset_job_status**](docs/MetadataApi.md#get_dataset_job_status) | **GET** /datasets/{datasetId}/job_status/{jobId} | Hent fremdriftsstatus for en bestemt prosess
*MetadataApi* | [**get_dataset_metadata**](docs/MetadataApi.md#get_dataset_metadata) | **GET** /datasets/{datasetId} | Hent metadata for et bestemt dataset
*MetadataApi* | [**get_datasets**](docs/MetadataApi.md#get_datasets) | **GET** /datasets | Hent liste over tilgjengelige dataset
*MetadataApi* | [**send_dataset_job_status**](docs/MetadataApi.md#send_dataset_job_status) | **POST** /datasets/{datasetId}/job_status | Send fremdriftsinformasjon

## Documentation For Models

 - [BoundingBox](docs/BoundingBox.md)
 - [Dataset](docs/Dataset.md)
 - [DatasetList](docs/DatasetList.md)
 - [DatasetListInner](docs/DatasetListInner.md)
 - [Error](docs/Error.md)
 - [InlineResponse200](docs/InlineResponse200.md)
 - [Locks](docs/Locks.md)
 - [LocksInner](docs/LocksInner.md)

## Documentation For Authorization


## basicAuth

- **Type**: HTTP basic authentication


## Author


