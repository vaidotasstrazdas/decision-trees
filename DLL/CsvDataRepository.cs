using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using IDLL.Exceptions;
using IDLL.Interfaces;

namespace DLL
{
    public class CsvDataRepository<TRecord> : ICsvDataRepository<TRecord>
    {
        public List<List<string>> CsvLines { get; set; }
        public List<TRecord> CsvLinesNormalized { get; set; }

        public void LoadData(string dataFile)
        {
            if (string.IsNullOrEmpty(dataFile))
            {
                throw new DalException("Empty or null string provided.");
            }

            if (!File.Exists(dataFile))
            {
                throw new DalException(string.Format("File {0} does not exist.", dataFile));
            }

            CsvLines = new List<List<string>>();
            using (var parser = new TextFieldParser(dataFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields == null)
                    {
                        continue;
                    }
                    CsvLines.Add(fields.ToList());
                }

            }

        }

        public void NormalizeData(int skip = 1)
        {
            CsvLinesNormalized = new List<TRecord>();

            foreach (var line in CsvLines)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }
                var recordObject = Activator.CreateInstance<TRecord>();
                var index = 0;
                foreach (var property in recordObject.GetType().GetProperties())
                {
                    var value = line[index++];
                    property.SetValue(recordObject, Convert.ChangeType(value, property.PropertyType), null);
                }
                CsvLinesNormalized.Add(recordObject);
            }
            

        }

    }
}
