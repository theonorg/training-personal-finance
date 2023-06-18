namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public record TransactionDTO(int id, 
    string Description, 
    string Currency, 
    decimal Amount, 
    decimal AmountInAccountCurrency, 
    DateTime TransactionDate);

