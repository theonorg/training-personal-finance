namespace Theonorg.DotnetTraining.ExchangeRateLib;

public abstract class ExchangeRateCalculatorBase : IExchangeRateCalculator
{
    protected Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>();
    public string Currency { get; }
    public ExchangeRateCalculatorBase(string currency)
    {
        Currency = currency;
        GenerateExchangeRates();
    }

    protected abstract void GenerateExchangeRates();

    public decimal Convert(decimal amount, string currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        if (currency == Currency)
        {
            return amount;
        }

        if (!_exchangeRates.ContainsKey(currency))
        {
            throw new ArgumentException($"Currency {currency} is not supported.");
        }

        return amount * _exchangeRates[currency];
    }
}
