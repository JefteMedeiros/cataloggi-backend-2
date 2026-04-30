using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace cataloggi_backend_2.Swagger;

public class SwaggerResponseExamplesOperationFilter : IOperationFilter
{
    private const string ApplicationJson = "application/json";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        SetValidationProblemExample(operation);
        RemoveEmptyResponseBody(operation, StatusCodes.Status401Unauthorized);
        RemoveEmptyResponseBody(operation, StatusCodes.Status429TooManyRequests);
    }

    private static void SetValidationProblemExample(OpenApiOperation operation)
    {
        if (operation.Responses is null
            || !operation.Responses.TryGetValue(StatusCodes.Status400BadRequest.ToString(), out var response)
            || response.Content is null
            || !response.Content.TryGetValue(ApplicationJson, out var content))
            return;

        content.Example = JsonNode.Parse(
            """
            {
              "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
              "title": "One or more validation errors occurred.",
              "status": 400,
              "errors": {
                "name": [
                  "Name is required"
                ]
              },
              "traceId": "00-00000000000000000000000000000000-0000000000000000-00"
            }
            """);
    }

    private static void RemoveEmptyResponseBody(OpenApiOperation operation, int statusCode)
    {
        if (operation.Responses is not null
            && operation.Responses.TryGetValue(statusCode.ToString(), out var response)
            && response.Content is not null)
        {
            response.Content.Clear();
        }
    }
}
