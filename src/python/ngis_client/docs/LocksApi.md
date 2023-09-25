# swagger_client.LocksApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**delete_dataset_locks**](LocksApi.md#delete_dataset_locks) | **DELETE** /datasets/{datasetId}/locks | Fjerne alle låser brukeren har i et bestemt dataset
[**get_dataset_locks**](LocksApi.md#get_dataset_locks) | **GET** /datasets/{datasetId}/locks | Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.

# **delete_dataset_locks**
> delete_dataset_locks(x_client_product_version, dataset_id, locking_type=locking_type)

Fjerne alle låser brukeren har i et bestemt dataset

Fjerne alle låser brukeren har i et bestemt dataset 

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
api_instance = swagger_client.LocksApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
locking_type = 'locking_type_example' # str | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional)

try:
    # Fjerne alle låser brukeren har i et bestemt dataset
    api_instance.delete_dataset_locks(x_client_product_version, dataset_id, locking_type=locking_type)
except ApiException as e:
    print("Exception when calling LocksApi->delete_dataset_locks: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **locking_type** | **str**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 

### Return type

void (empty response body)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_dataset_locks**
> Locks get_dataset_locks(x_client_product_version, dataset_id, locking_type=locking_type, bbox=bbox, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)

Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.

Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 

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
api_instance = swagger_client.LocksApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
locking_type = 'locking_type_example' # str | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional)
bbox = swagger_client.BoundingBox() # BoundingBox | Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`   (optional)
crs_epsg = 56 # int | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)
normalized_for_visualization = true # bool | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)

try:
    # Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.
    api_response = api_instance.get_dataset_locks(x_client_product_version, dataset_id, locking_type=locking_type, bbox=bbox, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling LocksApi->get_dataset_locks: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **locking_type** | **str**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **bbox** | [**BoundingBox**](.md)| Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   | [optional] 
 **crs_epsg** | **int**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalized_for_visualization** | **bool**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

[**Locks**](Locks.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.locks+json, application/vnd.kartverket.ngis.all_locks+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

