using Amazon.Lambda.Serialization.SystemTextJson;
using Bossolani.Products;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = ApiSerializerContext.Default;
});

builder.Services.AddSingleton<Database, Database>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi, options =>
{
    options.Serializer = new SourceGeneratorLambdaJsonSerializer<ApiSerializerContext>();
});
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "hh:mm:ss ";
});

var app = builder.Build();

Handlers.database = app.Services.GetRequiredService<Database>();
Handlers.Logger = app.Logger;

app.MapGet("/", Handlers.GetAllProducts);
app.MapGet("/{id}", Handlers.GetProduct);
app.MapDelete("/{id}", Handlers.DeleteProduct);
app.MapPut("/{id}", Handlers.PutProduct);
app.Run();

static class Handlers
{
    internal static Database database;
    internal static ILogger Logger;

    public static async Task GetAllProducts(HttpContext context)
    {
        Logger.LogInformation("Received request to list all products");
        var products = await database.GetAllProducts();
        Logger.LogInformation($"Found {products.Count} products(s)");
        await context.WriteResponse(HttpStatusCode.OK, products);
    }

    public static async Task DeleteProduct(HttpContext context)
    {
        var id = context.Request.RouteValues["id"].ToString();
        try
        {            
            Logger.LogInformation($"Received request to delete {id}");
            await database.DeleteProduct(id);
            Logger.LogInformation("Delete complete");
            await context.WriteResponse(HttpStatusCode.OK, $"Product with id {id} deleted");
        }
        catch (ProductNotFoundException)
        {
            Logger.LogWarning($"Id {id} not found.");
            await context.WriteResponse(HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failure deleting product");
            await context.WriteResponse(HttpStatusCode.InternalServerError);
        }
    }

    public static async Task GetProduct(HttpContext context)
    {
        var id = context.Request.RouteValues["id"].ToString();
        Logger.LogInformation($"Received request to get {id}");
        var product = await database.GetProduct(id);
        if (product == null)
        {
            Logger.LogWarning($"Id {id} not found.");
            await context.WriteResponse(HttpStatusCode.NotFound);
            return;
        }
        await context.WriteResponse(HttpStatusCode.OK, product);
    }

    public static async Task PutProduct(HttpContext context)
    {
        var id = context.Request.RouteValues["id"].ToString();
        var product = await JsonSerializer.DeserializeAsync<Product>(context.Request.Body, ApiSerializerContext.Default.Product);
        if (product == null || id != product.Id)
        {
            await context.WriteResponse(HttpStatusCode.BadRequest, "Product ID in the body does not match path parameter");
            return;
        }
        try
        {
            var updatedProduct = await database.PutProduct(product);
            await context.WriteResponse(HttpStatusCode.OK, $"Updated product with id {id}. Product details: {product}");
        }
        catch (ProductNotFoundException)
        {
            Logger.LogWarning($"Id {id} not found.");
            await context.WriteResponse(HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failure deleting product");
            await context.WriteResponse(HttpStatusCode.InternalServerError);
        }
    }
}

static class ResponseWriter
{
    public static async Task WriteResponse(this HttpContext context, HttpStatusCode statusCode)
    {
        await context.WriteResponse<string>(statusCode, "");
    }

    public static async Task WriteResponse<TResponseType>(this HttpContext context, HttpStatusCode statusCode, TResponseType body) where TResponseType : class
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(body, typeof(TResponseType), ApiSerializerContext.Default));
    }
}