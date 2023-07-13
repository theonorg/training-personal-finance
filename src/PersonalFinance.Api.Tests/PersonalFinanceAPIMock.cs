namespace PersonalFinance.Api.Tests;

class PersonalFinanceAPIMock : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();
 
        builder.ConfigureServices(services => 
        {
            services.AddScoped(sp =>
            {
                // Replace PostgreSQL with the in memory provider for tests
                return new DbContextOptionsBuilder<PersonalFinanceDbContext>()
                            .UseInMemoryDatabase("PersonalFinanceDbContext", root)
                            .UseApplicationServiceProvider(sp)
                            .Options;
            });
        });
 
        return base.CreateHost(builder);
    }
}
