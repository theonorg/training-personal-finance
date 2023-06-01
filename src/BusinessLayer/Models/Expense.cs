namespace Theonorg.DotnetTraining.BusinessLayer.Models;

public record Expense(
    string description, 
    Currency currency, 
    decimal amount, 
    decimal amountInAccountCurrency, 
    DateTime transactionDate) : 
    Transaction (
        description, 
        currency, 
        amount, 
        amountInAccountCurrency, 
        transactionDate);
