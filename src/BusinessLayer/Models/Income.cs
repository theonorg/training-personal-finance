namespace Theonorg.DotnetTraining.BusinessLayer.Models;

public record Income(
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