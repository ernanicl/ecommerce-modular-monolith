using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using FoodDelivery.Modules.Catalogs.Categories;
using FoodDelivery.Modules.Catalogs.Shared.Contracts;
using FoodDelivery.Modules.Catalogs.Shared.Extensions;

namespace FoodDelivery.Modules.Catalogs.Products.Features.ChangingProductCategory.Events;

public record ChangingProductCategory(CategoryId CategoryId) : DomainEvent;

internal class ChangingProductCategoryValidationHandler :
    IDomainEventHandler<ChangingProductCategory>
{
    private readonly ICatalogDbContext _catalogDbContext;

    public ChangingProductCategoryValidationHandler(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task Handle(ChangingProductCategory notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));
        Guard.Against.NegativeOrZero(notification.CategoryId, nameof(notification.CategoryId));
        Guard.Against.ExistsCategory(
            await _catalogDbContext.CategoryExistsAsync(notification.CategoryId, cancellationToken: cancellationToken),
            notification.CategoryId);
    }
}
