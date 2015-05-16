#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Shared.DecisionTrees.DataStructure;
using Shared.DecisionTrees.Interfaces;
#endregion

namespace Shared.DecisionTrees
{
    public class Classifier<TRecord> : IClassifier<TRecord>
    {

        #region Constructors and Destructors
        public Classifier()
        {
            Records = new Dictionary<string, double>();
            SetRecords();
        }
        #endregion

        #region Implemented Interfaces

        #region IClassifier

        public Dictionary<string, double> Records { get; private set; }

        public MarketAction Classify(TRecord record, Rule root)
        {
            PrepareRecord(record);

            while (true)
            {

                if (root.Action != default(MarketAction))
                {
                    return root.Action;
                }

                if (Records[root.LessOrEqualRule.Property] <= root.LessOrEqualRule.Value)
                {
                    root = root.LessOrEqualRule;
                    continue;
                }

                root = root.GreaterRule;
            }
        }

        #endregion

        #endregion

        #region Methods
        private void PrepareRecord(TRecord record)
        {
            var keys = Records.Keys.ToList();
            foreach (var key in keys)
            {
                Records[key] = Convert.ToDouble(typeof(TRecord).GetProperty(key).GetValue(record, null));
            }
        }

        private void SetRecords()
        {
            var type = typeof (TRecord);
            foreach (var property in type.GetProperties())
            {
                Records.Add(property.Name, 0.0);
            }
        }
        #endregion

    }
}
