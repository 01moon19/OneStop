using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;


namespace ECommerce.UserService.OpenApi
{ 

    public sealed class BearerSecurityOperationTransformer
        : IOpenApiOperationTransformer
    {
        public Task TransformAsync(
            OpenApiOperation operation,
            OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            var metadata = context.Description
                .ActionDescriptor.EndpointMetadata;

            var allowAnonymous = metadata
                .OfType<IAllowAnonymous>()
                .Any();

            var requiresAuthorization = metadata
                .OfType<IAuthorizeData>()
                .Any();

            if (!allowAnonymous && requiresAuthorization)
            {
                operation.Security ??= [];

                operation.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
                    });



            }

            return Task.CompletedTask;
        }
    }

}
