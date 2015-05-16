#region Usings
using System;
using System.Collections.Generic;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Shared.DecisionTrees.DataStructure
{
    public class DecisionTree<TRecord> : IDecisionTree<TRecord>
    {

        #region Private Fields
        private readonly IDecisionTreeReader _decisionTreeReader;
        private readonly IRuleBuilder _ruleBuilder;
        private readonly IClassifier<TRecord> _classifier;
        #endregion

        #region Constructors and Destructors
        public DecisionTree(
            IDecisionTreeReader decisionTreeReader,
            IRuleBuilder ruleBuilder,
            IClassifier<TRecord> classifier)
        {
            _decisionTreeReader = decisionTreeReader;
            _ruleBuilder = ruleBuilder;
            _classifier = classifier;
            Root = new Rule();
        }
        #endregion

        #region Implemented Interfaces

        #region IDecisionTree
        public Rule Root { get; private set; }

        public MarketAction ClassifyRecord(TRecord record)
        {
            return _classifier.Classify(record, Root);
        }

        public void SaveDecisionTree(string rawTree)
        {

            _decisionTreeReader.ReadSubTrees(rawTree);
            rawTree = _decisionTreeReader.NormalizeTreeSource(rawTree);
            rawTree = _decisionTreeReader.NormalizeTree(rawTree);

            var lines = rawTree.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var parents = new Dictionary<int, Rule> { {0, Root} };

            var to = lines.Length;
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
        #endregion

        #endregion

    }
}
