using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Bossolani.Products;
using System.Reflection.Metadata;

namespace Bossolani.Products.Tests;

public class FunctionsTest
{
    [Fact]
    public void TestGetMethod()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        context = new TestLambdaContext();
        //response = Handler.GetFunctionHandler(request, context);
        //Assert.Equal(200, response.StatusCode);
        //Assert.Equal("Hello AWS Serverless", response.Body);
    }
}