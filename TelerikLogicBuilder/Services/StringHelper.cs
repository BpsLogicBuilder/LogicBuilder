using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class StringHelper : IStringHelper
    {
        public string EnsureUniqueName(string nameString, Dictionary<string, string> names)
        {
            if (!names.ContainsKey(nameString))
            {
                names.Add(nameString, nameString);
                return nameString;
            }
            else
            {
                string lastChar = nameString[^1..];//^1 == 1 from end.
                if (short.TryParse(lastChar, out short lastCharShort))
                {
                    lastCharShort++;
                    nameString = nameString[0..^1];
                    nameString += lastCharShort.ToString(CultureInfo.CurrentCulture);
                    return EnsureUniqueName(nameString, names);
                }
                else
                {
                    nameString += "0";
                    return EnsureUniqueName(nameString, names);
                }
            }
        }

        public string[] SplitWithQuoteQualifier(string argument, params string[] delimiters)
        {
            if (string.IsNullOrEmpty(argument))
                return Array.Empty<string>();

            using StringReader sr = new(argument);
            using TextFieldParser parser = new(sr) { HasFieldsEnclosedInQuotes = true, Delimiters = delimiters };
            return parser.ReadFields();
        }

        public string ToTitleCase(string str) 
            => Regex.Replace(str, "(\\B[A-Z])", " $1");// \\B ignores the occurrence at the beginning of the string
    }
}
