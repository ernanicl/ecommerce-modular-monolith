using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Query;
using FoodDelivery.Modules.Customers.RestockSubscriptions.Dtos;
using FoodDelivery.Modules.Customers.Shared.Data;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FoodDelivery.Modules.Customers.RestockSubscriptions.Features.GettingRestockSubscriptionsByEmails;

public record GetRestockSubscriptionsByEmails(IList<string> Emails) : IStreamQuery<RestockSubscriptionDto>;

internal class GetRestockSubscriptionsByEmailsValidator : AbstractValidator<GetRestockSubscriptionsByEmails>
{
    public GetRestockSubscriptionsByEmailsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Emails)
            .NotNull()
            .NotEmpty();
    }
}

internal class GetRestockSubscriptionsByEmailsHandler
    : IStreamQueryHandler<GetRestockSubscriptionsByEmails, RestockSubscriptionDto>
{
    private readonly CustomersReadDbContext _customersReadDbContext;
    private readonly IMapper _mapper;

    public GetRestockSubscriptionsByEmailsHandler(CustomersReadDbContext customersReadDbContext, IMapper mapper)
    {
        _customersReadDbContext = customersReadDbContext;
        _mapper = mapper;
    }

    public IAsyncEnumerable<RestockSubscriptionDto> Handle(
        GetRestockSubscriptionsByEmails query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        var result = _customersReadDbContext.RestockSubscriptions.AsQueryable()
            .Where(x => !x.IsDeleted)
            .Where(x => query.Emails.Contains(x.Email!))
            .ProjectTo<RestockSubscriptionDto>(_mapper.ConfigurationProvider)
            .ToAsyncEnumerable();

        return result;
    }
}
