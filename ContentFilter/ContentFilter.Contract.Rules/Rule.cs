namespace ContentFilter
{
    public class Rule
    {
        public string RuleId { get; set; }
        public RuleType RuleType { get; set; }
        public string RuleContext { get; set; }
        public Condition Condition { get; set; }
    }

    public enum Condition
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
