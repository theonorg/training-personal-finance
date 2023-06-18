

using Microsoft.AspNetCore.Mvc;

namespace Theonorg.DotnetTraining.PersonalFinanceAPI;

public static class TransactionEndpoints
{
    public static RouteGroupBuilder MapTransactionsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTransactions)
                .Produces<List<Transaction>>(StatusCodes.Status200OK)
                .WithOpenApi();

        group.MapGet("/{id}", GetTransaction)
                .Produces<Transaction>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithOpenApi();

        group.MapPost("/", CreateTransaction)
                .Produces<Transaction>(StatusCodes.Status201Created)
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
                .Produces<List<Transaction>>(StatusCodes.Status200OK)
                .WithOpenApi();

        return group;
    }

    public static IResult GetAllTransactions([FromServices] ITransactionService transactionService)
    {
        return Results.Ok(transactionService.GetTransactions());
    }

    public static IResult GetTransaction([FromServices] ITransactionService transactionService, int id)
    {
        try
        {
            return Results.Ok(transactionService.GetTransactionById(id));
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }
    
    public static IResult CreateTransaction([FromServices] ITransactionService transactionService, [FromBody] TransactionDTO transaction)
    {
        var currency = Enum.Parse<Currency>(transaction.Currency);
        var newTransaction = transactionService.Add(transaction.Description, currency, transaction.Amount, transaction.TransactionDate);
        return Results.Created($"/{newTransaction.Id}", newTransaction);
    }

    public static IResult UpdateTransaction([FromServices] ITransactionService transactionService, int id, [FromBody] TransactionDTO transaction)
    {
        try
        {
            var myTransaction = transactionService.GetTransactionById(id);
            var currency = Enum.Parse<Currency>(transaction.Currency);
            transactionService.UpdateTransactionById(id, transaction.Description, currency, transaction.Amount, transaction.TransactionDate);
            return Results.NoContent();
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

    public static IResult DeleteTransaction([FromServices] ITransactionService transactionService, int id)
    {
        try
        {
            var myTransaction = transactionService.GetTransactionById(id);
            transactionService.DeleteTransactionById(id);
            return Results.NoContent();
        }
        catch (ArgumentException)
        {
            return Results.NotFound();
        }
    }

    public static IResult GetTransactionsByMonth([FromServices] ITransactionService transactionService, [FromRoute] int year, [FromRoute] int month)
    {
        return Results.Ok(transactionService.GetTransactionsByMonth(year, month));
    }
}
