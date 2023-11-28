# IO.Swagger.Api.LocksApi

All URIs are relative to *https://qmsrest.westeurope.cloudapp.azure.com:8080/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteDatasetLocks**](LocksApi.md#deletedatasetlocks) | **DELETE** /datasets/{datasetId}/locks | Fjerne alle låser brukeren har i et bestemt dataset
[**GetDatasetLocks**](LocksApi.md#getdatasetlocks) | **GET** /datasets/{datasetId}/locks | Hent informasjon om brukerens låste features i et bestemt dataset. Bruk &#x60;application/vnd.kartverket.ngis.locks+json&#x60; for kun å se egne låser, og &#x60;application/vnd.kartverket.ngis.all_locks+json&#x60; for også å se andres låser.

<a name="deletedatasetlocks"></a>
# **DeleteDatasetLocks**
> void DeleteDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null)

Fjerne alle låser brukeren har i et bestemt dataset

Fjerne alle låser brukeren har i et bestemt dataset 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteDatasetLocksExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new LocksApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lockingType = lockingType_example;  // string | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional) 

            try
            {
                // Fjerne alle låser brukeren har i et bestemt dataset
                apiInstance.DeleteDatasetLocks(xClientProductVersion, datasetId, lockingType);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling LocksApi.DeleteDatasetLocks: " + e.Message );
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
 **lockingType** | **string**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 

### Return type

void (empty response body)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="getdatasetlocks"></a>
# **GetDatasetLocks**
> Locks GetDatasetLocks (string xClientProductVersion, Guid? datasetId, string lockingType = null, BoundingBox bbox = null, int? crsEPSG = null, bool? normalizedForVisualization = null)

Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.

Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset. 

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDatasetLocksExample
    {
        public void main()
        {
            // Configure HTTP basic authorization: basicAuth
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";

            var apiInstance = new LocksApi();
            var xClientProductVersion = xClientProductVersion_example;  // string | Brukes for å kunne identifisere klienten som er brukt
            var datasetId = new Guid?(); // Guid? | UUID of the dataset to get
            var lockingType = lockingType_example;  // string | Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes.  (optional) 
            var bbox = new BoundingBox(); // BoundingBox | Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`   (optional) 
            var crsEPSG = 56;  // int? | Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  (optional) 
            var normalizedForVisualization = true;  // bool? | Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  (optional) 

            try
            {
                // Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.
                Locks result = apiInstance.GetDatasetLocks(xClientProductVersion, datasetId, lockingType, bbox, crsEPSG, normalizedForVisualization);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling LocksApi.GetDatasetLocks: " + e.Message );
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
 **lockingType** | **string**| Angir låsetype som skal brukes (foreløpig er kun &#x60;user_lock&#x60; støttet). Krever at brukeren har skrivetilgang mot dataset&#x27;et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med &#x60;user_lock&#x60; legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset&#x27;et.  Låsen vil fjernes neste gang brukeren skriver data til dataset&#x27;et med &#x60;user_lock&#x60;, eller dersom låsen slettes.  | [optional] 
 **bbox** | [**BoundingBox**](BoundingBox.md)| Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom &#x60;references&#x60; ikke er angitt blir det satt til &#x60;none&#x60;   | [optional] 
 **crsEPSG** | **int?**| Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes.  | [optional] 
 **normalizedForVisualization** | **bool?**| Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden.  | [optional] 

### Return type

[**Locks**](Locks.md)

### Authorization

[basicAuth](../README.md#basicAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.kartverket.ngis.locks+json, application/vnd.kartverket.ngis.all_locks+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
