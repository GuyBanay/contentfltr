using System;

public class Rule
{
    public string RuleId { get; set; }
    public Enum<RuleType> RuleType { get; set; }
    public string RuleContext { get; set; }
}

public Enum RuleType
{
    BeginWith,
    EndWith,


}
