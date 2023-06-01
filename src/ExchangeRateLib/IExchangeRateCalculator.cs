namespace Theonorg.DotnetTraining.ExchangeRateLib;
public interface IExchangeRateCalculator
{
    public string Currency { get; }
    public decimal Convert(decimal amount, string currency);
}
