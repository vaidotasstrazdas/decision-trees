#region Usings
using System;
using System.IO;
using Shared.DecisionTrees.DataStructure;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Shared.DecisionTrees
{

    public class RuleBuilder : IRuleBuilder
    {

        #region Implemented Interfaces

        #region IRuleBuilder
        public Rule Read(string line)
        {

            var parts = line.Split(new[] { " :" }, StringSplitOptions.None);

            var rule = SetRule(parts[0]);
            rule.Action = ReadAction(parts[1]);

            return rule;
        }

        public void MapRules(Rule previousRule, Rule currentRule)
        {
            switch (currentRule.Relation)
            {
                case RelationType.LessOrEqual:
                    previousRule.LessOrEqualRule = currentRule;
                    break;
                case RelationType.Greater:
                    previousRule.GreaterRule = currentRule;
                    break;
                default:
                    throw new ArgumentException("Relation Type unknown.");
            }
        }
        #endregion

        #endregion

        #region Methods
        private static Rule SetRule(string mainRulePart)
        {
            var rule = new Rule { Level = mainRulePart.Split(new[] { "   " }, StringSplitOptions.None).Length - 1 };

            var properties = mainRulePart.Replace("   ", string.Empty).Split(new[] { " " }, StringSplitOptions.None);
            var relation = properties[1];

            rule.Property = properties[0];
            rule.Value = double.Parse(properties[2]);

            if (relation == "<=")
            {
                rule.Relation = RelationType.LessOrEqual;
            }
            else if (relation == ">")
            {
                rule.Relation = RelationType.Greater;
            }
            else
            {
                throw new InvalidDataException(string.Format("Operation {0} is unknown.", relation));
            }

            return rule;
        }

        private static MarketAction ReadAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                return default(MarketAction);
            }

            var parts = action.Trim().Split(new[] { " " }, StringSplitOptions.None);

            return (MarketAction)Enum.Parse(typeof(MarketAction), parts[0]);

        }

        #endregion

    }

}
