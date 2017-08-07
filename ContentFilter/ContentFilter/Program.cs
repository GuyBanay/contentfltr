using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContentFilter.Contracts;

namespace ContentFilter
{
    internal static class Program
    {
        private const string RuleFilesAssembleyPattern = "ContentFilter.*.Rules.dll";
        private static readonly string CurrentDir = Directory.GetCurrentDirectory();
        private static readonly string ArticleFile = $@"{CurrentDir}\article.txt";
        private static readonly string RulesFile = $@"{CurrentDir}\rules.txt";
        private static readonly string ResultFile = $@"{CurrentDir}\result.txt";

        private static void Main()
        {
            MakeSureFileExists(RulesFile);
            MakeSureFileExists(ArticleFile);
            var rulesFromFile = GetRulesFromTextFile();
            var rules = new RuleParser().Parse(rulesFromFile).ToList();
            var canExit = false;
            while (!canExit)
            {
                MakeSureResultFileNotExists();
                var rulesDllPath = AskForAssembley();
                PrintRules(rules);

                using (var ruleManager = new RuleManager.RuleManager(rulesDllPath))
                {
                    foreach (var rule in rules)
                    {
                        ruleManager.AddRule(rule.Name, rule.Rule, rule.Content);
                    }
                    var ruleToApply = GetRuleToApplyFromUser(ruleManager);

                    var processArticle = new ProcessArticle(ArticleFile, ResultFile, ruleToApply);
                    processArticle.StartProcess().Wait();

                    Console.WriteLine("");
                    Console.WriteLine($"Done, please check {ResultFile}");
                }
                canExit = AskIfExit();
            }
        }

        private static bool AskIfExit()
        {
            Console.WriteLine("");
            Console.WriteLine("Press enter to continue");
            Console.WriteLine("type exit to end");
            Console.WriteLine("");
            var answer = Console.ReadLine();
            return !string.IsNullOrEmpty(answer) && answer.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }

        private static void MakeSureResultFileNotExists()
        {
            while (true)
            {
                if (!File.Exists(ResultFile)) return;
                Console.WriteLine($"{ResultFile} already exists");
                Console.WriteLine("Please remove the file and press enter to continue.");
                var answer = Console.ReadLine();
                CheckExit(answer);
            }
        }

        private static void MakeSureFileExists(string fileName)
        {
            while (true)
            {
                if (File.Exists(fileName)) return;
                Console.WriteLine($"Cannot find {fileName}");
                Console.WriteLine("Please add the file and press enter to continue.");
                var answer = Console.ReadLine();
                CheckExit(answer);
            }
        }

        
        private static BaseRule GetRuleToApplyFromUser(IRuleManager ruleManager)
        {
            while (true)
            {
                var applyRule = GetApplyRuleFromUser();
                var ruleToApply = ruleManager.GetRule(applyRule);
                if (ruleToApply == null) continue;
                return ruleToApply;
            }
        }

        private static string GetApplyRuleFromUser()
        {
            while (true)
            {
                Console.WriteLine("Please choose rule by APPLY [ruleName]");
                Console.WriteLine("");
                var answer = Console.ReadLine();
                CheckExit(answer);
                if (answer == null || !answer.StartsWith("Apply ", StringComparison.OrdinalIgnoreCase)) continue;
                var rule = answer.Split(' ');
                if (rule.Length != 2) continue;
                return rule[1];
            }
        }

        private static void PrintRules(IEnumerable<RuleDefinition> rulesFromFile)
        {
            Console.WriteLine("Available Rules:");
            Console.WriteLine("");
            foreach (var rule in rulesFromFile)
            {
                Console.WriteLine($"{rule.Name}: {rule.Rule} {rule.Content}");
            }
            Console.WriteLine("");
        }

        private static void CheckExit(string answer)
        {
            if (string.IsNullOrEmpty(answer)) return;
            if (answer.Equals("exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);
        }
        
        private static Dictionary<int, string> GetRulesAssemblies()
        {
            while (true)
            {
                var contentRulesAssemblies = GetFilesFromPatterns(CurrentDir, RuleFilesAssembleyPattern);
                if (!contentRulesAssemblies.Any())
                {
                    Console.WriteLine("Couln't find any rule assemblies");
                    Console.WriteLine($"Please copy file with pattern {RuleFilesAssembleyPattern} into {CurrentDir}");
                    Console.WriteLine("And press Enter");
                    var answer = Console.ReadLine();
                    CheckExit(answer);
                }
                else
                {
                    return contentRulesAssemblies;
                }
            }
        }

        private static string AskForAssembley()
        {
            var assemblies = GetRulesAssemblies();

            while (true)
            {
                Console.WriteLine("Please choose assembly...");
                Console.WriteLine("");

                foreach (var assembly in assemblies)
                {
                    var fileName = Path.GetFileName(assembly.Value);
                    Console.WriteLine($"{assembly.Key}. {fileName}");
                }

                Console.WriteLine("");
                var answer = Console.ReadLine();
                CheckExit(answer);
                int.TryParse(answer, out int intAnswer);

                if (intAnswer == 0) continue;
                assemblies.TryGetValue(intAnswer, out string assemblyFile);
                return assemblyFile;
            }
        }

        private static Dictionary<int, string> GetFilesFromPatterns(string path, string pattern)
        {
            var assemblies = new Dictionary<int, string>();
            var files = Directory.GetFiles(path, pattern).ToList();
            var index = 1;
            foreach (var file in files)
            {
                assemblies.Add(index, file);
                index++;
            }
            return assemblies;
        }

        private static IEnumerable<string> GetRulesFromTextFile()
        {
            return File.ReadAllLines(RulesFile).Where(l=>l.Length> 0).ToList();
        }
    }
}
