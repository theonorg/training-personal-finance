namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services.DTO;

public record TransactionDTO (
    int Id,
    string Description, 
    string Currency, 
    decimal Amount, 
    decimal AmountInAccountCurrency, 
    DateTime TransactionDate
);