
namespace Theonorg.DotnetTraining.PersonalFinanceAPI;

public static class AccountsEndpoints
{
    public static RouteGroupBuilder MapAccountsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllAccounts)
                .Produces<List<AccountDTO>>(StatusCodes.Status200OK)
                .WithOpenApi();

        group.MapGet("/{id}", GetAccount)
                .Produces<AccountDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapPost("/", CreateAccount)
                .Produces<AccountDTO>(StatusCodes.Status201Created)
                .WithOpenApi();
        
        group.MapPut("/{id}", UpdateAccount)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapDelete("/{id}", DeleteAccount)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        return group;
    }

    public static async Task<IResult> GetAllAccounts([FromServices] IAccountService accountService)
    {
        return Results.Ok(await accountService.GetAllAsync());
    }

    public static async Task<IResult> GetAccount([FromServices] IAccountService accountService, int id)
    {
        try
        {
            return Results.Ok(await accountService.GetByIdAsync(id));
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }
    
    public static async Task<IResult> CreateAccount([FromServices] IAccountService accountService, [FromBody] AccountDTO Account)
    {
        var newAccount = await accountService.AddAsync(Account.Name, Account.Currency, Account.Balance);
        return Results.Created($"/{newAccount.Id}", newAccount);
    }

    public static async Task<IResult> UpdateAccount([FromServices] IAccountService accountService, int id, [FromBody] AccountDTO Account)
    {
        try
        {
            var updated = await accountService.UpdateByIdAsync(id, Account.Name);
            return Results.Ok(updated);
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

    public static async Task<IResult> DeleteAccount([FromServices] IAccountService accountService, int id)
    {
        try
        {
            _ = await accountService.DeleteByIdAsync(id);
            return Results.NoContent();
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

}
