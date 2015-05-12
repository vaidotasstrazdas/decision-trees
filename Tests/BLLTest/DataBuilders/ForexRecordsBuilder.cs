using System.Collections.Generic;
using System.Configuration;
using Bridge.IDLL.Data;
using Microsoft.VisualBasic.FileIO;

namespace Tests.BLLTest.DataBuilders
{
    public static class ForexRecordsBuilder
    {

        public static List<ForexRecord> BuildRecords()
        {
            var dataFile = ConfigurationManager.AppSettings["TestDataDirectory"] + "\\ForexTrading.csv";

            var records = new List<ForexRecord>();
            using (var parser = new TextFieldParser(dataFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                    {
                        continue;
                    }
                    records.Add(new ForexRecord
                    {
                        CurrencyPair = fields[0],
                        Date = fields[1],
                        Bid = double.Parse(fields[2]),
                        Ask = double.Parse(fields[3])
                    });
                }

            }
            return records;

        }

    }
}
