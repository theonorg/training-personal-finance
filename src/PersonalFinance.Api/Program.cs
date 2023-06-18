
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(opts =>
        {
            opts.IncludeScopes = false;
            opts.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
            opts.ColorBehavior = LoggerColorBehavior.Disabled;
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"Personal Finance API",
                Version = "1.0"
            });
        });


builder.Services.AddSingleton<ITransactionService, TransactionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var transactionEndpoints = app.MapGroup("/transaction")
                              .MapTransactionsApi();

app.Run();
