using System.Linq;


namespace ContentFilter.General.Rules.Rules
{
    public class OrRule : BaseRule
    {
        public override bool IsMatch(string text)
        {
            var rules = ContentToRules(Content);
            var b = rules.Any(r => r.IsMatch(text));
            return b;
        }
    }
}
