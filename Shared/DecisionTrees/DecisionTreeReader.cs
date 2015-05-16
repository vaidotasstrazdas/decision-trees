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

        public Dictionary<string, string> SubTrees { get; private set; }

        #region Private Fields
        private StringBuilder _treeBuilder;
        #endregion

        #region Implemented Interfaces

        #region IDecisionTreeReader

        public string NormalizeTreeSource(string treeSource)
        {
            return treeSource
                .Replace("|", " ")
                .Replace(":...", "    ")
                .Replace(":   ", "    ")
                .Replace(" : ", ":")
                .Replace(" :", ":")
                .Replace(": ", ":")
                .Replace("SubTree", "Subtree");
        }

        public string NormalizeTree(string treeSource)
        {
            var parts = treeSource.Split(new[] { "Subtree " }, StringSplitOptions.None);
            treeSource = parts[0];
            _treeBuilder = new StringBuilder();
            var lines = treeSource.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                ReadLine(line, 0);
            }

            return _treeBuilder.ToString().Trim();
        }

        public void ReadSubTrees(string treeSource)
        {
            var parts = treeSource.Split(new[] { "Subtree " }, StringSplitOptions.None);

            SubTrees = new Dictionary<string, string>();

            for (var i = 1; i < parts.Length; i++)
            {
                var builder = new StringBuilder();
                var subtreeParts = parts[i].Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (var j = 1; j < subtreeParts.Length; j++)
                {
                    builder.AppendLine(subtreeParts[j]);
                }
                SubTrees.Add(subtreeParts[0], builder.ToString());
            }

        }
        #endregion

        #endregion

        #region Methods

        private void ReadLine(string line, int addLevels)
        {
            var level = line.Split(new[] { "    " }, StringSplitOptions.None).Length - 1;
            var parts = line.Split(new[] { ":" }, StringSplitOptions.None);
            var subTreeKey = parts[1];
            if (SubTrees.ContainsKey(subTreeKey))
            {
                line = line.Replace(subTreeKey, string.Empty);
                for (var k = 0; k < addLevels; k++)
                {
                    _treeBuilder.Append("    ");
                }
                _treeBuilder.AppendLine(line);
                var subTreeLines = SubTrees[subTreeKey].Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subtreeLine in subTreeLines)
                {
                    ReadLine(subtreeLine, addLevels + level + 1);
                }
            }
            else
            {
                for (var k = 0; k < addLevels; k++)
                {
                    _treeBuilder.Append("    ");
                }
                _treeBuilder.Append(line);
                _treeBuilder.AppendLine();
            }
        }
        #endregion

    }
}
