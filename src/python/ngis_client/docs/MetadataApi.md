# swagger_client.MetadataApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**get_dataset_job_status**](MetadataApi.md#get_dataset_job_status) | **GET** /datasets/{datasetId}/job_status/{jobId} | Hent fremdriftsstatus for en bestemt prosess
[**get_dataset_metadata**](MetadataApi.md#get_dataset_metadata) | **GET** /datasets/{datasetId} | Hent metadata for et bestemt dataset
[**get_datasets**](MetadataApi.md#get_datasets) | **GET** /datasets | Hent liste over tilgjengelige dataset
[**send_dataset_job_status**](MetadataApi.md#send_dataset_job_status) | **POST** /datasets/{datasetId}/job_status | Send fremdriftsinformasjon

# **get_dataset_job_status**
> object get_dataset_job_status(x_client_product_version, dataset_id, job_id)

Hent fremdriftsstatus for en bestemt prosess

Mulighet for å følge med på fremdriften til f.eks en asynkron oppdatering 

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
api_instance = swagger_client.MetadataApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
job_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the job

try:
    # Hent fremdriftsstatus for en bestemt prosess
    api_response = api_instance.get_dataset_job_status(x_client_product_version, dataset_id, job_id)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling MetadataApi->get_dataset_job_status: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **job_id** | [**str**](.md)| UUID of the job | 

### Return type

**object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_dataset_metadata**
> Dataset get_dataset_metadata(x_client_product_version, dataset_id, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)

Hent metadata for et bestemt dataset

Henter utvidet informasjon om et bestemt dataset.  Her ligger all informasjon som kommer ut i listen over dataset, og i tillegg en del ekstra informasjon som enten er unødvendig eller for tung å ha med i listen over dataset. 

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
api_instance = swagger_client.MetadataApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get
crs_epsg = 56 # int | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional)
normalized_for_visualization = true # bool | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional)

try:
    # Hent metadata for et bestemt dataset
    api_response = api_instance.get_dataset_metadata(x_client_product_version, dataset_id, crs_epsg=crs_epsg, normalized_for_visualization=normalized_for_visualization)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling MetadataApi->get_dataset_metadata: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 
 **crs_epsg** | **int**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalized_for_visualization** | **bool**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

[**Dataset**](Dataset.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.dataset+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_datasets**
> DatasetList get_datasets(x_client_product_version)

Hent liste over tilgjengelige dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  Dersom brukeren ikke har tilgang til noen dataset, returneres en tom liste. 

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
api_instance = swagger_client.MetadataApi(swagger_client.ApiClient(configuration))
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt

try:
    # Hent liste over tilgjengelige dataset
    api_response = api_instance.get_datasets(x_client_product_version)
    pprint(api_response)
except ApiException as e:
    print("Exception when calling MetadataApi->get_datasets: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 

### Return type

[**DatasetList**](DatasetList.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.datasets+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **send_dataset_job_status**
> send_dataset_job_status(body, x_client_product_version, dataset_id)

Send fremdriftsinformasjon

Gir klienten mulighet til å sende inn fremdriftsinformasjon slik at serveren f.eks kan følge med på status for klienten, valideringsfeil o.l. 

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
api_instance = swagger_client.MetadataApi(swagger_client.ApiClient(configuration))
body = {
  "id" : "07b59e3d-a4b6-4bae-ac4c-664d3dc3d778",
  "status" : "SUCCEEDED",
  "metadata" : {
    "copy_transaction_token" : 1000,
    "source_copy_transaction_token" : 1100,
    "current_operation" : "getDatasetMetadata"
  }
} # object | Klienten kan f.eks sende informasjon underveis i oppdateringen
x_client_product_version = 'x_client_product_version_example' # str | Brukes for å kunne identifisere klienten som er brukt
dataset_id = '38400000-8cf0-11bd-b23e-10b96e4ef00d' # str | UUID of the dataset to get

try:
    # Send fremdriftsinformasjon
    api_instance.send_dataset_job_status(body, x_client_product_version, dataset_id)
except ApiException as e:
    print("Exception when calling MetadataApi->send_dataset_job_status: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**object**](object.md)| Klienten kan f.eks sende informasjon underveis i oppdateringen | 
 **x_client_product_version** | **str**| Brukes for å kunne identifisere klienten som er brukt | 
 **dataset_id** | [**str**](.md)| UUID of the dataset to get | 

### Return type

void (empty response body)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

