using BuildingBlocks.Core.Exception.Types;

namespace FoodDelivery.Modules.Identity.Identity.Exceptions;

public class EmailNotConfirmedException : BadRequestException
{
    public EmailNotConfirmedException(string email) : base($"Email not confirmed for email address `{email}`")
    {
        Email = email;
    }

    public string Email { get; }
}
