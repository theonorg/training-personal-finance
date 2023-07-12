namespace Theonorg.DotnetTraining.PersonalFinanceAPI.Models;

public class Currency
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
}
