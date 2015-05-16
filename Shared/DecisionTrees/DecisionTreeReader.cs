#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Shared.DecisionTrees
{
    public class DecisionTreeReader : IDecisionTreeReader
    {

        #region Private Fields
        private Dictionary<string, string> _subtrees;
        private StringBuilder _treeBuilder;
        #endregion

        #region Implemented Interfaces

        #region IDecisionTreeReader
        public string NormalizeTree(string treeSource)
        {
            _treeBuilder = new StringBuilder();
            var lines = treeSource.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                ReadLine(line, 0);
            }

            return _treeBuilder.ToString();
        }

        public void ReadSubTrees(string[] parts)
        {
            _subtrees = new Dictionary<string, string>();

            for (var i = 1; i < parts.Length; i++)
            {
                var builder = new StringBuilder();
                var subtreeParts = parts[i].Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (var j = 1; j < subtreeParts.Length; j++)
                {
                    builder.AppendLine(subtreeParts[j]);
                }
                _subtrees.Add(subtreeParts[0], builder.ToString());
            }

        }
        #endregion

        #endregion

        #region Methods

        private void ReadLine(string line, int addLevels)
        {
            var level = line.Split(new[] { "|   " }, StringSplitOptions.None).Length - 1;
            var parts = line.Split(new[] { " :" }, StringSplitOptions.None);
            var subTreeKey = parts[1];
            if (_subtrees.ContainsKey(subTreeKey))
            {
                line = line.Replace(subTreeKey, string.Empty);
                _treeBuilder.AppendLine(line);
                var subTreeLines = _subtrees[subTreeKey].Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subtreeLine in subTreeLines)
                {
                    ReadLine(subtreeLine, level + 1);
                }
            }
            else
            {
                for (var k = 0; k <= addLevels; k++)
                {
                    _treeBuilder.Append("|   ");
                }
                _treeBuilder.Append(line);
                _treeBuilder.AppendLine();
            }
        }
        #endregion

    }
}
