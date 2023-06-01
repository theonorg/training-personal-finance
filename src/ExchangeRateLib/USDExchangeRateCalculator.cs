namespace Theonorg.DotnetTraining.ExchangeRateLib;

public class USDExchangeRateCalculator : ExchangeRateCalculatorBase
{
    const string MyCurrency = "USD";

    public USDExchangeRateCalculator() : base(MyCurrency)
    {
    }

    protected override void GenerateExchangeRates()
    {
        _exchangeRates.Add("EUR", 1.15m);
        _exchangeRates.Add("GBP", 1.28m);
    }
}
