namespace FoodDelivery.Modules.Identity.Identity.Features.VerifyEmail;

public class VerifyEmailRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}
