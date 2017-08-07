using System;
using ContentFilter.Contracts;

namespace ContentFilter.General.Rules.Rules
{

    public class EndWithRule : BaseRule
    {

        public override bool IsMatch(string text)
        {
            var b = text.EndsWith(Content, StringComparison.OrdinalIgnoreCase);
            return b;
        }
    }
}