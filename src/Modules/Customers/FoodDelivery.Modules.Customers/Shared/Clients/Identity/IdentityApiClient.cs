using System.Net.Http.Json;
using Ardalis.GuardClauses;
using BuildingBlocks.Core.Exception;
using FoodDelivery.Modules.Customers.Shared.Clients.Identity.Dtos;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Modules.Customers.Shared.Clients.Identity;

// Ref: http://www.kamilgrzybek.com/design/modular-monolith-integration-styles/
public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IdentityApiClientOptions _options;

    public IdentityApiClient(HttpClient httpClient, IOptions<IdentityApiClientOptions> options)
    {
        _httpClient = Guard.Against.Null(httpClient, nameof(httpClient));
        _options = Guard.Against.Null(options.Value, nameof(options));

        if (string.IsNullOrEmpty(_options.BaseApiAddress) == false)
            _httpClient.BaseAddress = new Uri(_options.BaseApiAddress);
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task<GetUserByEmailResponse?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(email, nameof(email));
        Guard.Against.InvalidEmail(email);

        var userIdentity = await _httpClient.GetFromJsonAsync<GetUserByEmailResponse>(
            $"{_options.UsersEndpoint}/by-email/{email}",
            cancellationToken);

        return userIdentity;
    }

    public async Task<CreateUserResponse?> CreateUserIdentityAsync(
        CreateUserRequest createUserRequest,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(createUserRequest, nameof(createUserRequest));

        var response = await _httpClient.PostAsJsonAsync(
            _options.UsersEndpoint,
            createUserRequest,
            cancellationToken);

        // throws if not 200-299
        response.EnsureSuccessStatusCode();

        var createdUser =
            await response.Content.ReadFromJsonAsync<CreateUserResponse?>(cancellationToken: cancellationToken);

        return createdUser;
    }
}
