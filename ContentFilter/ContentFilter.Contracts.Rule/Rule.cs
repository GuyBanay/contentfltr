namespace ContentFilter.Contracts.Rule
{
    public class Rule
    {
        public string RuleId { get; set; }
        public RuleType RuleType { get; set; }
        public string RuleContext { get; set; }
        public Operator Operator { get; set; }
    }

    public enum Operator
    {
        And,
        Not,
        Or
    }

    public enum RuleType
    {
        BeginWith,
        EndWith,
        Contains
    }
}
