using BuildingBlocks.Core.Exception.Types;

namespace FoodDelivery.Modules.Identity.Identity.Exceptions;

public class UserLockedException : BadRequestException
{
    public UserLockedException(string userId) : base($"userId '{userId}' has been locked.")
    {
    }
}
