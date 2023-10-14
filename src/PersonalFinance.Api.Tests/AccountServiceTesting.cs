namespace PersonalFinance.Api.Tests;

public class AccountServiceTesting
{
    private readonly PersonalFinanceAPIMock _app;
    public AccountServiceTesting() {
        _app = new PersonalFinanceAPIMock();

        using (var scope = _app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var dbContext = provider.GetRequiredService<PersonalFinanceDbContext>())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }

    [Fact]
    public async Task GetAccounts()
    {
        var client = _app.CreateClient();
        var accounts = await client.GetFromJsonAsync<List<AccountDTO>>("/account");

        Assert.Empty(accounts);
    }

    [InlineData("My Account", "USD", 300.0)]
    [InlineData("My Second Account", "EUR", 100.0)]
    [Theory]
    public async Task AddAccount(string accountName, string currency, decimal initialBalance)
    {
        NewAccountDTO newAccountDTO = new NewAccountDTO(accountName, currency, initialBalance);

        var client = _app.CreateClient();
        var response = await client.PostAsJsonAsync("/account", newAccountDTO);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
 
        var accounts = await client.GetFromJsonAsync<List<AccountDTO>>("/account");
    
        Assert.Single(accounts);
        Assert.Equal(accountName, accounts?[0].Name);
        Assert.Equal(currency, accounts?[0].Currency);
        Assert.Equal(initialBalance, accounts?[0].Balance);
    }

    // [InlineData("My Account", "BRL", 300.0)]
    // [Theory]
    // public async Task AddAccountNoCurrency(string accountName, string currency, decimal initialBalance)
    // {
    //     NewAccountDTO newAccountDTO = new NewAccountDTO(accountName, currency, initialBalance);

        var client = _app.CreateClient();
    //     var response = await client.PostAsJsonAsync("/account", newAccountDTO);

    //     Assert.Equal(HttpStatusCode.Created, response.StatusCode);
 
    //     var accounts = await client.GetFromJsonAsync<List<AccountDTO>>("/account");
    
    //     Assert.Single(accounts);
    //     Assert.Equal(accountName, accounts?[0].Name);
    //     Assert.Equal(currency, accounts?[0].Currency);
    //     Assert.Equal(initialBalance, accounts?[0].Balance);
    // }

    [Fact]
    public async Task GetOneAccount()
    {
        NewAccountDTO newAccountDTO = new NewAccountDTO("My Account", "USD", 300.0m);

        var client = _app.CreateClient();
        var response = await client.PostAsJsonAsync("/account", newAccountDTO);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var addedAccount = await response.Content.ReadFromJsonAsync<AccountDTO>();
 
        var account = await client.GetFromJsonAsync<AccountDTO>("/account/" + addedAccount?.Id);
    
        Assert.Equal("My Account", account?.Name);
    }

    [Fact]
    public async Task GetNotFoundAccount()
    {
        var client = _app.CreateClient();
        var response = await client.GetAsync("/account/1000");
    
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [InlineData("My Account", "USD", 300.0, "My Updated Account")]
    [InlineData("My Second Account", "EUR", 100.0, "My Updated Second Account")]
    [Theory]
    public async Task UpdateAccountName(string accountName, string currency, decimal initialBalance, string updatedAccountName)
    {
        NewAccountDTO newAccountDTO = new NewAccountDTO(accountName, currency, initialBalance);

        var client = _app.CreateClient();
        var response = await client.PostAsJsonAsync("/account", newAccountDTO);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
 
        var addedAccount = await response.Content.ReadFromJsonAsync<AccountDTO>();
        var toUpdate = new UpdateAccountDTO(updatedAccountName);

        var updateResponse = await client.PutAsJsonAsync("/account/" + addedAccount.Id, toUpdate);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedAccount = await updateResponse.Content.ReadFromJsonAsync<AccountDTO>();
    
        Assert.Equal(updatedAccountName, updatedAccount!.Name);
    }

    [Fact]
    public async Task UpdateAccountBadRequest()
    {
        UpdateAccountDTO updatedAccount = new UpdateAccountDTO("Update Bad Request");

        var client = _app.CreateClient();
        var updatedResponse = await client.PutAsJsonAsync("/account/1000", updatedAccount);
    
        Assert.Equal(HttpStatusCode.NotFound, updatedResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo()
    {
        NewAccountDTO newAccountDTO = new NewAccountDTO("My Account", "USD", 300.0m);

        var client = _app.CreateClient();
        var response = await client.PostAsJsonAsync("/account", newAccountDTO);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var addedAccount = await response.Content.ReadFromJsonAsync<AccountDTO>();
 
        var deletedResponse = await client.DeleteAsync("/account/" + addedAccount?.Id);
    
        Assert.Equal(HttpStatusCode.NoContent, deletedResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteNotFoundAccount()
    {
        var client = _app.CreateClient();
        var deletedResponse = await client.DeleteAsync("/account/1000");
    
        Assert.Equal(HttpStatusCode.NotFound, deletedResponse.StatusCode);
    }
}