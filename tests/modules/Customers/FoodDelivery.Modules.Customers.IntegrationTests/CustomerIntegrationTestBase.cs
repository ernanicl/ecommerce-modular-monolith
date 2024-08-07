using FoodDelivery.Api;
using FoodDelivery.Modules.Customers.Shared.Data;
using Tests.Shared.Fixtures;
using Xunit.Abstractions;

namespace FoodDelivery.Modules.Customers.IntegrationTests;

public class CustomerModuleTestIntegrationTestBase : ModuleTestBase<Program, CustomersModuleConfiguration, CustomersDbContext, CustomersReadDbContext>
{
    public CustomerModuleTestIntegrationTestBase(IntegrationTestFixture<Program> integrationTestFixture,
        ITestOutputHelper outputHelper) : base(integrationTestFixture, outputHelper)
    {
    }
}
