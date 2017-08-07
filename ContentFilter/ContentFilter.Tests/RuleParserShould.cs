using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ContentFilter.Tests
{
    public class RuleParserShould:RuleTestBase
    {
        [Fact]
        public void GetRulesBasic()
        {
            var ruleParser = new RuleParser();
            var rules = ruleParser.Parse(GetRulesLines()).ToList();
            Assert.Equal(7,rules.Count);
            CheckAllRulesHaveValues(rules);
        }

        [Fact]
        public void GetRulesAdvanced()
        {
            var ruleParser = new RuleParser();
            var rules = ruleParser.Parse(GetRulesAdvancedLines()).ToList();
            Assert.Equal(7, rules.Count);
            CheckAllRulesHaveValues(rules);
        }

        
        private static void CheckAllRulesHaveValues(IEnumerable<RuleDefinition> rules)
        {
            foreach (var rule in rules)
            {
                Assert.NotEmpty(rule.Name);
                Assert.NotEmpty(rule.Content);
                Assert.NotEmpty(rule.Rule);
            }
        }


        private static List<string> GetRulesAdvancedLines()
        {
            var lines = new List<string>
            {
                "R1: BeginWith \"hello\"",
                "R2: EndWith \"bye\"",

                "",
                "R3: Contain \"world\"",
                "R9: Contain",
                "R4: AND R1 R2",
                "",
                "R2: End \"bye1\"",
                "R5: NOT R3",
                "R6: OR R4 R5",
                "R7: WordCount 10",
                "R8: Xor",
                "R2 EndWith \"bye\""
            };
            return lines;
        }
    }
}
