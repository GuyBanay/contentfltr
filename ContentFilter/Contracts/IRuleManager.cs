namespace ContentFilter.Contracts
{
    public interface IRuleManager
    {
        BaseRule GetRule(string ruleName);
     }
}