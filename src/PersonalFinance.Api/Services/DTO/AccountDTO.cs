namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Services.DTO;

public record NewAccountDTO (
    string Name,
    string Currency,
    decimal Balance
);

public record UpdateAccountDTO (
    string Name
);

public record SimpleAccountDTO (
    int Id,
    string Name
);

public record AccountDTO (
    int Id,
    string Name, 
    string Currency, 
    decimal Balance
);

public record FullAccountDTO (
    int Id,
    string Name, 
    string Currency, 
    decimal Balance, 
    List<TransactionDTO> Transactions
);
