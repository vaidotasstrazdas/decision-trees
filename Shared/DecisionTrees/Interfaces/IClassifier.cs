#region Usings
using System.Collections.Generic;
using Shared.DecisionTrees.DataStructure;
#endregion

namespace Shared.DecisionTrees.Interfaces
{
    public interface IClassifier<in TRecord>
    {

        Dictionary<string, double> Records { get; }

        MarketAction Classify(TRecord record, Rule root);

    }
}
