namespace Theonorg.DotnetTraining.ExchangeRateLib;

public class EuroExchangeRateCalculator : ExchangeRateCalculatorBase
{
    const string MyCurrency = "EUR";

    public EuroExchangeRateCalculator() : base(MyCurrency)
    {
    }

    protected override void GenerateExchangeRates()
    {
        _exchangeRates.Add("USD", 0.85m);
        _exchangeRates.Add("GBP", 1.16m);
    }
}
