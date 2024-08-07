using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace FoodDelivery.Modules.Catalogs.Products.Features.UpdatingProduct;

// PUT api/v1/catalog/products/{id}
public static class UpdateProductEndpoint
{
    internal static IEndpointRouteBuilder MapCreateProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{id}}",
                UpdateProducts)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(ProductsConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute("Updating Product", "Updating Product"))
            .WithName("UpdateProduct")
            .WithDisplayName("Update a product.")
            .WithApiVersionSet(ProductsConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> UpdateProducts(
        long id,
        UpdateProductRequest request,
        IGatewayProcessor<CatalogModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateProduct(
                id,
                request.Name,
                request.Price,
                request.RestockThreshold,
                request.MaxStockThreshold,
                request.Status,
                request.Width,
                request.Height,
                request.Depth,
                request.Size,
                request.CategoryId,
                request.SupplierId,
                request.BrandId,
                request.Description);

            await commandProcessor.SendAsync(command, cancellationToken);
            return Results.NoContent();
        });
    }
}
