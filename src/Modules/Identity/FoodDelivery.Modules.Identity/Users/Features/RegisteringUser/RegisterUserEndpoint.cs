using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;

namespace FoodDelivery.Modules.Identity.Users.Features.RegisteringUser;

public static class RegisterUserEndpoint
{
    internal static IEndpointRouteBuilder MapRegisterNewUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{UsersConfigs.UsersPrefixUri}", RegisterUser)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser")
            .WithDisplayName("Register New user.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return endpoints;
    }

    private static Task<IResult> RegisterUser(
        RegisterUserRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new RegisterUser(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email,
                request.Password,
                request.ConfirmPassword,
                request.Roles?.ToList());

            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Created($"{UsersConfigs.UsersPrefixUri}/{result.UserIdentity?.Id}", result);
        });
    }
}
