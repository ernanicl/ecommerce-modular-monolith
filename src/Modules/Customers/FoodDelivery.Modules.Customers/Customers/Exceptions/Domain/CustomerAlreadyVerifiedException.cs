using BuildingBlocks.Core.Exception.Types;

namespace FoodDelivery.Modules.Customers.Customers.Exceptions.Domain;

internal class CustomerAlreadyVerifiedException : AppException
{
    public long CustomerId { get; }

    public CustomerAlreadyVerifiedException(long customerId)
        : base($"Customer with Id: '{customerId}' already verified.")
    {
        CustomerId = customerId;
    }
}
