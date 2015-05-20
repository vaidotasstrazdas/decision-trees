using System;
using System.IO;
using Bridge.IDLL.Exceptions;
using Bridge.IDLL.Interfaces;
using Shared.DecisionTrees.DataStructure;

namespace Implementation.DLL
{
    public class DecisionTreesRepository : IDecisionTreesRepository
    {

        public string DecisionTreesPath { get; set; }

        public string ReadSource(string period, int month, int chunk, DecisionTreeAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case DecisionTreeAlgorithm.C45:
                    return ReadC45Source(period, month, chunk);
                case DecisionTreeAlgorithm.C50:
                    return ReadC50Source(period, month, chunk);
                default:
                    throw new DalException("Incorrect algorithm provided.");
            }
        }

        private string ReadC45Source(string period, int month, int chunk)
        {
            var sourcePath = GetPath(period, month, chunk, "C45");

            var c45Contents = File.ReadAllText(sourcePath).Replace("Simplified Decision Tree:", "SimplifiedDecisionTree:");
            var secondDelimiter = "SimplifiedDecisionTree:";
            if (!c45Contents.Contains("SimplifiedDecisionTree:"))
            {
                secondDelimiter = "Tree saved";
            }

            var parts = c45Contents.Split(new[] {"Decision Tree:"}, StringSplitOptions.None);
            parts = parts[1].Split(new[] { secondDelimiter }, StringSplitOptions.None);
            var decisionTree = parts[0].Trim();

            return decisionTree;
        }

        private string ReadC50Source(string period, int month, int chunk)
        {
            var sourcePath = GetPath(period, month, chunk, "C50");

            var c50Contents = File.ReadAllText(sourcePath).Replace("Decision tree:", string.Empty);

            var parts = c50Contents.Split(new[] { "-----  Trial 0:  -----" }, StringSplitOptions.None);

            var otherPart = parts[1];
            var delimiter = "***";
            if (otherPart.Contains("-----  Trial 1:  -----"))
            {
                delimiter = "-----  Trial 1:  -----";
            }

            parts = otherPart.Split(new[] { delimiter }, StringSplitOptions.None);
            var decisionTree = parts[0].Trim();
            return decisionTree;
        }

        private string GetPath(string period, int month, int chunk, string algorithmSignature)
        {
            var path = Path.Combine(DecisionTreesPath, GetMonth(month), period, string.Format("Forex_{0}.{1}.done", chunk, algorithmSignature));
            if (!File.Exists(path))
            {
                throw new DalException(string.Format("Path {0} does not exist.", path));
            }
            return path;
        }

        private static string GetMonth(int month)
        {
            return string.Format("{0:00}", month);
        }

    }
}
