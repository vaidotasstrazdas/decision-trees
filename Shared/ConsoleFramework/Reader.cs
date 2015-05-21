using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleFramework.Exceptions;

namespace ConsoleFramework
{
    public static class Reader
    {

        public static string ReadString(string message = "", string defaultValue = null)
        {
            var result = ReadConsoleLine(message, defaultValue);
            return result;
        }

        public static List<string> ReadList(string message = "", List<string> defaultValue = null, string delimiter = ",")
        {
            var defaultValueString = defaultValue != null ? string.Join(delimiter, defaultValue) : null;
            var result = ReadConsoleLine(message, defaultValueString);

            return result.Split(new[] {delimiter}, StringSplitOptions.None)
                         .Select(x => x.Trim())
                         .ToList();
        }

        public static int ReadInt(string message = "", int defaultValue = 0)
        {
            var result = ReadConsoleLine(message, defaultValue.ToString());
            int returnInt;
            if (!int.TryParse(result, out returnInt))
            {
                throw new ConsoleFrameWorkException("Wrong integer entered.");
            }
            return returnInt;
        }

        private static string ReadConsoleLine(string message, string defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.Write(message);
            }
            var result = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            if (defaultValue == null)
            {
                throw new ConsoleFrameWorkException("Provided string is null or empty.");
            }

            return defaultValue;
        }
    }
}
