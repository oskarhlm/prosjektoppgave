using System.Text.Json;
using Azure.AI.OpenAI;

namespace Client.GeonorgeFunctions;

public class GetGeonorgeDatasetFunction
{
    public const string Name = "get_geonorge_datasets";

    public static FunctionDefinition GetFunctionDefinition()
    {
        return new FunctionDefinition()
        {
            Name = Name,
            Description = "Get a list of datasets from the GeoNorge API",
            Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new 
                    {
                        SearchTerm = new
                        {
                            Type = "string",
                            Description = "The search term about which we want to find datasets from the API." +
                                          "The search term should be in Norwegian."
                        }
                    },
                    Required = new[] { "searchTerm" }
                }, 
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
        };
    }
    
    public static async Task<IEnumerable<string?>> GetDatasets(Uri baseUrl, string searchString)
    {
        HttpClient client = new();
        client.BaseAddress = baseUrl;
        var response = await client.GetAsync($"{baseUrl}/search?text={searchString}");
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<GeonorgeSearchResponse>(content);
        return data?.Results.Select(res => res.Title)!;
    }
}

public class GeonorgeSearchResponse
{
    public int NumFound { get; set; }
    public GeonorgeSearchResult[] Results { get; set; } = Array.Empty<GeonorgeSearchResult>();
}

public class GeonorgeSearchResult
{
    public Guid Uuid { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
}

public class GeonorgeDatasetSearchInput
{
    public string SearchTerm { get; set; } = string.Empty;
}