using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentFilter
{
    public class RuleParser
    {
        private List<RuleDefinition> _rules = new List<RuleDefinition>();

        public IEnumerable<RuleDefinition> Parse(IEnumerable<string> lines)
        {
            _rules = new List<RuleDefinition>();
            foreach (var line in lines)
            {
                ParseLine(line);
            }
            return _rules;
        }


        private void ParseLine(string line)
        {
            try
            {
                var nameAndDefinition = line.Split(':');
                var name = nameAndDefinition[0].Trim();
                var definition = nameAndDefinition[1].Trim();

                var ruleAndArguments = definition.Split(' ');
                var ruleId = ruleAndArguments[0];

                var rule = CreateRuleDefinition(name, ruleId, ruleAndArguments.Skip(1).ToArray());

                if (!_rules.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    _rules.Add(rule);
                }
                else
                {
                    Console.WriteLine($"Rule name already exist, ignoring \"{line}\"");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Rule line does not meet the definition \"{line}\"");
            }
        }
        private RuleDefinition CreateRuleDefinition(string name, string rule, string[] arguments)
        {
            var argument = arguments[0];
            var isValueArgument = argument.StartsWith("\"");
            var parsedArguments = isValueArgument ? argument.Replace("\"", string.Empty) : string.Join(" ", arguments);

            return new RuleDefinition
            {
                Name = name,
                Content = parsedArguments,
                Rule = rule,
            };
        }
    }
}
