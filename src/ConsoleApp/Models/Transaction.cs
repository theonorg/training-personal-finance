using Theonorg.DotnetTraining.BusinessLayer.Models;

namespace Theonorg.DotnetTraining.ConsoleApp.Models;

public record TransactionDTO (
    string Description, 
    Currency Currency, 
    decimal Amount, 
    DateTime TransactionDate
);