


namespace Theonorg.DotnetTraining.PersonalFinanceAPI;

public static class TransactionEndpoints
{
    public static RouteGroupBuilder MapTransactionsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTransactions)
                .Produces<List<TransactionDTO>>(StatusCodes.Status200OK)
                .WithOpenApi();

        group.MapGet("/{id}", GetTransaction)
                .Produces<TransactionDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapPost("/", CreateTransaction)
                .Produces<TransactionDTO>(StatusCodes.Status201Created)
                .WithOpenApi();
        
        group.MapPut("/{id}", UpdateTransaction)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapDelete("/{id}", DeleteTransaction)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapGet("/byMonth/{year}/{month}", GetTransactionsByMonth)
                .Produces<List<TransactionDTO>>(StatusCodes.Status200OK)
                .WithOpenApi();

        return group;
    }

    public static async Task<IResult> GetAllTransactions([FromServices] ITransactionService transactionService)
    {
        return Results.Ok(await transactionService.GetTransactionsAsync());
    }

    public static async Task<IResult> GetTransaction([FromServices] ITransactionService transactionService, int id)
    {
        try
        {
            return Results.Ok(await transactionService.GetTransactionByIdAsync(id));
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }
    
    public static async Task<IResult> CreateTransaction([FromServices] ITransactionService transactionService, [FromBody] TransactionDTO transaction, [FromQuery] int accountId)
    {
        var newTransaction = await transactionService.AddTransactionAsync(accountId, transaction.Description, transaction.Currency, transaction.Amount, transaction.TransactionDate);
        return Results.Created($"/{newTransaction.Id}", newTransaction);
    }

    public static async Task<IResult> UpdateTransaction([FromServices] ITransactionService transactionService, int id, [FromBody] TransactionDTO transaction)
    {
        try
        {
            var updatedTransaction = await transactionService.UpdateTransactionByIdAsync(id, transaction.Description, transaction.Currency, transaction.Amount, transaction.TransactionDate);
            return Results.NoContent();
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

    public static async Task<IResult> DeleteTransaction([FromServices] ITransactionService transactionService, int id)
    {
        try
        {
            var updatedTransaction = await transactionService.DeleteTransactionByIdAsync(id);
            return Results.NoContent();
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

    public static async Task<IResult> GetTransactionsByMonth([FromServices] ITransactionService transactionService, [FromRoute] int year, [FromRoute] int month)
    {
        return Results.Ok(await transactionService.GetTransactionsByMonthAsync(year, month));
    }
}
