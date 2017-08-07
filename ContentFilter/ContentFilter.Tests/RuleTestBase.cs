using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContentFilter.Tests
{
    public class RuleTestBase
    {
        protected const string RuleFilesAssembleyPattern = "ContentFilter.*.Rules.dll";
        private static IEnumerable<RuleDefinition> GetRules()
        {
            var rulesLines = GetRulesLines();
            return new RuleParser().Parse(rulesLines);
        }

        protected static List<string> GetFilesByPattern(string pattern)
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory(), pattern).ToList();
        }

        protected static RuleManager.RuleManager GetRuleManagerWithRules()
        {
            var files = GetFilesByPattern(RuleFilesAssembleyPattern);
            var rules = GetRules().ToList();

            var ruleManager = new RuleManager.RuleManager(files[0]);
            foreach (var rule in rules)
            {
                ruleManager.AddRule(rule.Name, rule.Rule, rule.Content);
            }
            return ruleManager;
        }

        protected static IEnumerable<string> GetRulesLines()
        {
            var lines = new List<string>
            {
                "R1: BeginWith \"hello\"",
                "R2: EndWith \"bye\"",
                "R3: Contain \"world\"",
                "R4: AND R1 R2",
                "R5: NOT R3",
                "R6: OR R4 R5",
                "R7: WordCount 10"
            };
            return lines;
        }
    }
}
