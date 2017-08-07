

using ContentFilter.Contracts;

namespace ContentFilter.General.Rules.Rules
{
    public class NotRule : BaseRule
    {
        public override bool IsMatch(string text)
        {
            var rules = ContentToRules(Content);
            var b = rules.TrueForAll(r => !r.IsMatch(text));
            return b;
        }
    }
}
