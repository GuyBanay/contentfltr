using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContentFilter.Contracts;

namespace ContentFilter.RuleManager
{
    public class RuleManager: IRuleManager, IDisposable
    {
        private readonly List<BaseRule> _rules = new List<BaseRule>();
        private List<Type> _derivedTypes = new List<Type>();
        
        public RuleManager(string rulesDllPath)
        {
            try
            {
                GetRulesTypes(rulesDllPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load {rulesDllPath}",ex);
            }
            
        }

        public BaseRule GetRule(string ruleName)
        {
            var rule = _rules.FirstOrDefault(r => r.Name?.Equals(ruleName, StringComparison.OrdinalIgnoreCase) == true);
            if (rule != null)
            {
                return rule;
            }
            throw new Exception($"Cannot find rule {ruleName}");
        }

        private static IEnumerable<Type> FindDerivedTypes(Assembly assembly)
        {
            return assembly.ExportedTypes.Where(t => t != typeof(BaseRule) &&
                                                  t.IsAssignableFrom(t) && !t.IsAbstract && t.BaseType == typeof(BaseRule));
        }

        private void GetRulesTypes(string rulesDllPath)
        {
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(rulesDllPath));
            _derivedTypes = FindDerivedTypes(assembly).ToList();
            if (!_derivedTypes.Any())
                throw new Exception($"The assembly {rulesDllPath} is not contains clasess that inherit from BaseRule");
        }

        public void AddRule(string ruleName, string rule, string content)
        {
            if (_rules.Any(r => r.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Rule already exists {ruleName}");
            }
            var derivedType =
                _derivedTypes.FirstOrDefault(t => t.Name.Equals($"{rule}Rule", StringComparison.OrdinalIgnoreCase));
            if (derivedType != null)
            {
                var ruleInstance = (BaseRule)Activator.CreateInstance(derivedType);
                if (ruleInstance != null)
                {
                    ruleInstance.SetContent(content);
                    ruleInstance.Name = ruleName;
                    _rules.Add(ruleInstance);
                }
                else
                {
                    throw new Exception($"Faield to create rule instance {rule}");
                }
            }
            else
            {
                throw new Exception($"Cannot find rule class {rule}");
            }
        }

        public void Dispose()
        {
            foreach (var rule in _rules)
            {
                rule.Dispose();
            }
        }
    }
}
