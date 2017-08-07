using System;
using Xunit;

namespace ContentFilter.Tests
{
    public class RulesShould : RuleTestBase
    {
        [Theory]
        [InlineData("R1", "Hello bla bla", true)]
        [InlineData("R1", "bla bla Hello bla bla", false)]
        [InlineData("R2", "Hello bla bla bye", true)]
        [InlineData("R2", "bla bla Hello bla bla", false)]
        [InlineData("R3", "bla worlD bla bye", true)]
        [InlineData("R3", "bla bla Hello bla bla", false)]
        [InlineData("R4", "Hello bla worlD bla bye", true)]
        [InlineData("R4", "Hello bla bla Hello bla bla", false)]
        [InlineData("R4", "bla bla Hello bla bla bye", false)]
        [InlineData("R5", "Hello bla wrlD bla bye", true)]
        [InlineData("R5", "Hello bla worlD bla Hello bla bla", false)]
        [InlineData("R6", "Hello bla worlD bla bye", true)]
        [InlineData("R6", "Hello bla wolD bla", true)]
        [InlineData("R6", "bla bla worlD bla bla bye", false)]
        [InlineData("R7", "bla bla worlD bla bla bye jhg kjhg kjhg kjhg", true)]
        [InlineData("R7", "bla bla worlD bla bla bye jhg kjhg kjhg kjhg jkhgkj", false)]
        public void TestExistsRules(string ruleName, string line, bool expectedResult)
        {
            using (var ruleManager = GetRuleManagerWithRules())
            {
                var isMatch = ruleManager.GetRule(ruleName).IsMatch(line);
                Assert.Equal(expectedResult, isMatch);
            }
        }

        [Fact]
        public void TestNotExistsRules()
        {
            using (var ruleManager = GetRuleManagerWithRules())
            {
                const string notExistsRuleName = "R17";
                var expectedException = new Exception($"Cannot find rule {notExistsRuleName}");
                var actuaException = new Exception();
                try
                {
                    ruleManager.GetRule(notExistsRuleName).IsMatch("line");
                }
                catch (Exception e)
                {
                    actuaException = e;
                }
                Assert.Equal(expectedException.Message, actuaException.Message);
            }
        }


        [Fact]
        public void AddSameRuleNameTwice()
        {
            const string existsRuleName = "R2";
            using (var ruleManager = GetRuleManagerWithRules())
            {
                var expectedException = new Exception($"Rule already exists {existsRuleName}");
                var actuaException = new Exception();
                try
                {
                    ruleManager.AddRule(existsRuleName, "XOR", "R1 R3");
                }
                catch (Exception e)
                {
                    actuaException = e;
                }
                Assert.Equal(expectedException.Message, actuaException.Message);
            }
        }
    }
}
