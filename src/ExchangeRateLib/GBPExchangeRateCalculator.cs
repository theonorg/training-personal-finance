namespace Theonorg.DotnetTraining.ExchangeRateLib;

public class GBPExchangeRateCalculator : ExchangeRateCalculatorBase
{
    const string MyCurrency = "GBP";

    public GBPExchangeRateCalculator() : base(MyCurrency)
    {
    }

    protected override void GenerateExchangeRates()
    {
        _exchangeRates.Add("EUR", 1.18m);
        _exchangeRates.Add("USD", 1.38m);
    }
}
