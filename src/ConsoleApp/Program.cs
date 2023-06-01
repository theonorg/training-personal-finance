using Theonorg.DotnetTraining.BusinessLayer.Models;
using Theonorg.DotnetTraining.BusinessLayer.Services;
using Theonorg.DotnetTraining.ConsoleApp.Models;

Console.WriteLine("Welcome to Personal Finance Console App!");
Console.WriteLine("Let me ask you details to create your Account");

Console.Write("Account Name: ");
string accountName = Console.ReadLine()!;

Currency? currency = null;

do {
    Console.Write("Account Currency (EUR, USD, GBP): ");
    string accountCurrency = Console.ReadLine()!;

    if (Enum.IsDefined(typeof(Currency), accountCurrency)) {
        currency = (Currency)Enum.Parse(typeof(Currency), accountCurrency);
        break;
    } else {
        Console.WriteLine("Invalid Currency");
        continue;
    }
} while (currency == null);

Console.Write("Initial Balance: ");
decimal initialBalance = decimal.Parse(Console.ReadLine()!);

AccountService accountService = new AccountService(accountName, currency.Value, initialBalance);

Console.WriteLine($"Account created: {accountService.Account}");

Console.WriteLine("Let's add some transactions to your account");

char? option = null;

do {

    Console.WriteLine("Select an option:");
    Console.WriteLine("1. Add Expense");
    Console.WriteLine("2. Add Income");
    Console.WriteLine("3. Get Balance");
    Console.WriteLine("4. Get Transactions by Month");
    Console.WriteLine("5. Get Transactions between Dates");
    Console.WriteLine("q. Quit");

    option = Console.ReadKey().KeyChar;

    Console.WriteLine();

    switch(option)  {
        case '1':
            var exp = GetTransactionData();
            accountService.AddExpense(exp.Description, exp.Currency, exp.Amount, exp.TransactionDate);
            Console.WriteLine($"Expense added with description {exp.Description} and amount {exp.Currency} {exp.Amount}");
            break;
        case '2':
            var inc = GetTransactionData();
            accountService.AddIncome(inc.Description, inc.Currency, inc.Amount, inc.TransactionDate);
            Console.WriteLine($"Expense added with description {inc.Description} and amount {inc.Currency} {inc.Amount}");
            break;
        case '3':
            Console.WriteLine($"Balance: {accountService.GetBalance()}");
            break;
        case '4':
            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine()!);
            Console.Write("Month: ");
            int month = int.Parse(Console.ReadLine()!);
            List<Transaction> transactionsByMonth = accountService.GetTransactionsByMonth(year, month);
            Console.WriteLine($"Transactions by Month {month} of {year}");
            foreach (Transaction transaction in transactionsByMonth) {
                Console.WriteLine($"{transaction.TransactionDate} - {transaction.Description} - {transaction.Amount} {transaction.Currency}");
            }
            break;
        case '5':
            Console.Write("Start Date (yyyy-MM-dd): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("End Date (yyyy-MM-dd): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine()!);

            List<Transaction> transactionsBetweenDates = accountService.GetTransactionsBetweenDates(startDate, endDate);

            Console.WriteLine($"Transactions between {startDate} and {endDate}");
            foreach (Transaction transaction in transactionsBetweenDates) {
                Console.WriteLine($"{transaction.TransactionDate} - {transaction.Description} - {transaction.Amount} {transaction.Currency}");
            }
            break;
    }

} while (option != 'q');

TransactionDTO GetTransactionData() {
    Console.Write("Description: ");
    string description = Console.ReadLine()!;
    Console.Write("Currency (EUR, USD, GBP): ");
    string currency = Console.ReadLine()!;
    Console.Write("Amount: ");
    decimal amount = decimal.Parse(Console.ReadLine()!);
    Console.Write("Transaction Date (yyyy-MM-dd): ");
    DateTime transactionDate = DateTime.Parse(Console.ReadLine()!);

    return new TransactionDTO(description, (Currency)Enum.Parse(typeof(Currency), currency), amount, transactionDate);
}



