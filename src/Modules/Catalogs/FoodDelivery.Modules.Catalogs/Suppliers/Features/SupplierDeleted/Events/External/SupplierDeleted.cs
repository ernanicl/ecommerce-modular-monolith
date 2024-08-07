using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Core.Messaging;

namespace FoodDelivery.Modules.Catalogs.Suppliers.Features.SupplierDeleted.Events.External;

public record SupplierDeleted(long Id) : IntegrationEvent;

public class SupplierDeletedConsumer : IEventHandler<SupplierDeleted>
{
    public Task Handle(SupplierDeleted notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
