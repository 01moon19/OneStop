using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace ECommerce.UserService.OpenApi
{ 
    public sealed class BearerSecuritySchemeTransformer
        : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes ??=
                new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] =
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT Bearer token."
                };

            return Task.CompletedTask;
        }
    }

}
