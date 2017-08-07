using System;
using Xunit;

namespace ContentFilter.Tests
{
    public class RuleManagerShould: RuleTestBase
    {
        [Fact]
        public void LoadNotExistsDll()
        {
            const string filePath = @"c:\temp\notexistfile.dll";

            var expectedException = new Exception($"Failed to load {filePath}");
            var actuaException = new Exception();
            try
            {
                var ruleManager = new RuleManager.RuleManager(filePath);
                Assert.Null(ruleManager);
            }
            catch (Exception e)
            {
                actuaException = e;
            }
            Assert.Equal(expectedException.Message, actuaException.Message);
        }

        [Fact]
        public void LoadNotMeetDll()
        {
            const string pattern = "xunit.core.dll";
            var files = GetFilesByPattern(pattern);
            var expectedException = new Exception($"The assembly {files[0]} is not contains clasess that inherit from BaseRule");
            var actuaException = new Exception();
            try
            {
                var ruleManager = new RuleManager.RuleManager(files[0]);
                Assert.Null(ruleManager);
            }
            catch (Exception e)
            {
                actuaException = e;
            }
            Assert.Equal(expectedException.Message, actuaException.InnerException?.Message);
        }

        [Fact]
        public void LoadRulesDll()
        {
            var files = GetFilesByPattern(RuleFilesAssembleyPattern);
            RuleManager.RuleManager ruleManager = null;
            try
            {
                ruleManager = new RuleManager.RuleManager(files[0]);
            }
            catch
            {
                // ignored
            }
            Assert.NotNull(ruleManager);
        }
    }
}
