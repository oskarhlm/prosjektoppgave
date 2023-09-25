# IO.Swagger.Api.MetadataApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetDatasetJobStatus**](MetadataApi.md#getdatasetjobstatus) | **GET** /datasets/{datasetId}/job_status/{jobId} | Hent fremdriftsstatus for en bestemt prosess
[**GetDatasetMetadata**](MetadataApi.md#getdatasetmetadata) | **GET** /datasets/{datasetId} | Hent metadata for et bestemt dataset
[**GetDatasets**](MetadataApi.md#getdatasets) | **GET** /datasets | Hent liste over tilgjengelige dataset
[**SendDatasetJobStatus**](MetadataApi.md#senddatasetjobstatus) | **POST** /datasets/{datasetId}/job_status | Send fremdriftsinformasjon

<a name="getdatasetjobstatus"></a>
# **GetDatasetJobStatus**
> Object GetDatasetJobStatus (string xClientProductVersion, Guid? datasetId, Guid? jobId)

Hent fremdriftsstatus for en bestemt prosess

Mulighet for å følge med på fremdriften til f.eks en asynkron oppdatering 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetJobStatusExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new MetadataApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var jobId = new Guid?(); // Guid? | UUID of the job

            try
            {
                // Hent fremdriftsstatus for en bestemt prosess
                Object result = apiInstance.GetDatasetJobStatus(xClientProductVersion, datasetId, jobId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MetadataApi.GetDatasetJobStatus: " + e.Message );
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
 **jobId** | [**Guid?**](Guid?.md)| UUID of the job | 

### Return type

**Object**

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="getdatasetmetadata"></a>
# **GetDatasetMetadata**
> Dataset GetDatasetMetadata (string xClientProductVersion, Guid? datasetId, int? crsEPSG = null, bool? normalizedForVisualization = null)

Hent metadata for et bestemt dataset

Henter utvidet informasjon om et bestemt dataset.  Her ligger all informasjon som kommer ut i listen over dataset, og i tillegg en del ekstra informasjon som enten er unødvendig eller for tung å ha med i listen over dataset. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetMetadataExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new MetadataApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var crsEPSG = 56;  // int? | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional) 
            var normalizedForVisualization = true;  // bool? | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional) 

            try
            {
                // Hent metadata for et bestemt dataset
                Dataset result = apiInstance.GetDatasetMetadata(xClientProductVersion, datasetId, crsEPSG, normalizedForVisualization);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MetadataApi.GetDatasetMetadata: " + e.Message );
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
 **crsEPSG** | **int?**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalizedForVisualization** | **bool?**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

[**Dataset**](Dataset.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.dataset+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="getdatasets"></a>
# **GetDatasets**
> DatasetList GetDatasets (string xClientProductVersion)

Hent liste over tilgjengelige dataset

Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  Dersom brukeren ikke har tilgang til noen dataset, returneres en tom liste. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetsExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new MetadataApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt

            try
            {
                // Hent liste over tilgjengelige dataset
                DatasetList result = apiInstance.GetDatasets(xClientProductVersion);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MetadataApi.GetDatasets: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 

### Return type

[**DatasetList**](DatasetList.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.datasets+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="senddatasetjobstatus"></a>
# **SendDatasetJobStatus**
> void SendDatasetJobStatus (Object body, string xClientProductVersion, Guid? datasetId)

Send fremdriftsinformasjon

Gir klienten mulighet til å sende inn fremdriftsinformasjon slik at serveren f.eks kan følge med på status for klienten, valideringsfeil o.l. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SendDatasetJobStatusExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new MetadataApi();
            var body = new Object(); // Object | Klienten kan f.eks sende informasjon underveis i oppdateringen
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get

            try
            {
                // Send fremdriftsinformasjon
                apiInstance.SendDatasetJobStatus(body, xClientProductVersion, datasetId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MetadataApi.SendDatasetJobStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Object**](Object.md)| Klienten kan f.eks sende informasjon underveis i oppdateringen | 
 **xClientProductVersion** | **string**| Brukes for å kunne identifisere klienten som er brukt | 
 **datasetId** | [**Guid?**](Guid?.md)| UUID of the dataset to get | 

### Return type

void (empty response body)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
