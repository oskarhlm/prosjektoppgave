using System.Text.Json;
using Client.GeonorgeFunctions;
using Client.GPT4;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var serializingOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/ai", async Task<string?> (HttpRequest request, IConfiguration configuration) =>
{
    ChatGptClient client = new(configuration);
    var kartkatalogenBaseUrl = configuration["Kartkatalogen:BaseUrl"]!;
    
    string prompt = request.Query["prompt"]!;
    
    var responseChoice = await client.MakePrompt(prompt);

    if (responseChoice?.Message.FunctionCall.Name != GetGeonorgeDatasetFunction.Name) return "No function was called";
    
    string unvalidatedArguments = responseChoice.Message.FunctionCall.Arguments;
        
    var input = JsonSerializer.Deserialize<GeonorgeDatasetSearchInput>(
        unvalidatedArguments, serializingOptions)!;
        
    var functionResultData = await GetGeonorgeDatasetFunction.
        GetDatasets(new Uri(kartkatalogenBaseUrl), input.SearchTerm);
        
    return JsonSerializer.Serialize(functionResultData);;

});

app.MapGet("/hard-coded", async (IConfiguration configuration) =>
{
    var baseUrl = configuration["Kartkatalogen:BaseUrl"]!;
    var result = await GetGeonorgeDatasetFunction.GetDatasets(new Uri(baseUrl), "berggrunn");
    return JsonSerializer.Serialize(result);
});

app.Run();