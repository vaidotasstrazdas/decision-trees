using System;
using System.Collections.Generic;
using Shared.DecisionTrees.Interfaces;

namespace Shared.DecisionTrees.DataStructure
{
    public class DecisionTree<TRecord>
    {
        private readonly IDecisionTreeReader _decisionTreeReader;
        private readonly IRuleBuilder _ruleBuilder;

        private Dictionary<string, double> _records;
        public Rule Root { get; private set; }

        public DecisionTree(
            IDecisionTreeReader decisionTreeReader,
            IRuleBuilder ruleBuilder)
        {
            _decisionTreeReader = decisionTreeReader;
            _ruleBuilder = ruleBuilder;
            Root = new Rule();
            _records = new Dictionary<string, double>();
            SetRecord();
        }

        public void SaveDecisionTree(string rawTree)
        {

            var parts = rawTree.Split(new[] { "Subtree " }, StringSplitOptions.None);
            _decisionTreeReader.ReadSubTrees(parts);
            rawTree = _decisionTreeReader.NormalizeTree(parts[0]);

            var lines = rawTree.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var parents = new Dictionary<int, Rule> { {0, Root} };

            var to = lines.Length - 1;
            for (var i = 0; i < to; i++)
            {
                var line = lines[i];
                var rule = _ruleBuilder.Read(line);
                
                var childLevel = rule.Level + 1;
                if (!parents.ContainsKey(childLevel))
                {
                    parents.Add(childLevel, rule);
                }
                else
                {
                    parents[childLevel] = rule;
                }
                _ruleBuilder.MapRules(parents[rule.Level], rule);
            }

        }

        private void PrepareRecord(TRecord record)
        {
            foreach (var key in _records.Keys)
            {
                _records[key] = Convert.ToDouble(typeof(TRecord).GetProperty(key).GetValue(record, null));
            }
        }

        private void SetRecord()
        {
            var type = typeof (TRecord);
            foreach (var property in type.GetProperties())
            {
                _records[property.Name] = 0.0;
            }
        }

    }
}
