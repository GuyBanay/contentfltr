using System;
using ContentFilter.Contracts;

namespace ContentFilter.General.Rules.Rules
{
    public class BeginWithRule : BaseRule
    {
        public override bool IsMatch(string text)
        {
            var b = text.StartsWith(Content, StringComparison.OrdinalIgnoreCase);
            return b;
        }
    }
}