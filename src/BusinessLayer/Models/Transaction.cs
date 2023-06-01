namespace Theonorg.DotnetTraining.BusinessLayer.Models;

public record Transaction (
    string Description, 
    Currency Currency, 
    decimal Amount, 
    decimal AmountInAccountCurrency, 
    DateTime TransactionDate
);

