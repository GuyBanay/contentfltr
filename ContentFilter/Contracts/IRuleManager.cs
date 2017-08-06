namespace ContentFilter.Contracts
{
    public interface IRuleManager
    {
        IBaseRule GetRule(string ruleName);
     }
}