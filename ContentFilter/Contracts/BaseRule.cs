using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentFilter.Contracts
{
    public abstract class BaseRule :IDisposable
    {
        private static readonly List<BaseRule> Rules = new List<BaseRule>();

        protected BaseRule()
        {
            Rules.Add(this);
        }

        public string Name { get; set; }
        protected string Content { get; private set; }

        public abstract bool IsMatch(string text);
        public void SetContent(string content)
        {
            Content = content;
        }

        protected static List<BaseRule> ContentToRules(string content)
        {
            var rules = new List<BaseRule>();
            var ruleNames = content.Split(' ');
            foreach (var ruleName in ruleNames)
            {
                var rule = Rules.FirstOrDefault(r => r.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase));
                if (rule != null)
                {
                    rules.Add(rule);
                }
                else
                {
                    throw new Exception($"Cannot find rule {ruleName}");
                }
            }
            return rules;
        }

        public void Dispose()
        {
            Rules.Remove(this);
        }
    }
}
