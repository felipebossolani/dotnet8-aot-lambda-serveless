using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;

namespace Bossolani.Products;

[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<Product>))]
[JsonSerializable(typeof(Product))]
public partial class ApiSerializerContext : JsonSerializerContext
{
}