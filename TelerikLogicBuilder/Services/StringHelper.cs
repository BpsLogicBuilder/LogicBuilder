using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class StringHelper : IStringHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public StringHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string EnsureUniqueName(string nameString, HashSet<string> names)
        {
            if (!names.Contains(nameString))
            {
                names.Add(nameString);
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

        public string GetResoureString(string key, bool ignoreCase)
        {
            bool original = Strings.ResourceManager.IgnoreCase;
            Strings.ResourceManager.IgnoreCase = ignoreCase;
            string result = Strings.ResourceManager.GetString(key) ?? throw _exceptionHelper.CriticalException("{BB22946E-A51B-464A-AEDA-A68F1458D47E}");
            Strings.ResourceManager.IgnoreCase = original;

            return result;
        }

        public string[] SplitWithQuoteQualifier(string argument, params string[] delimiters)
        {
            if (string.IsNullOrEmpty(argument))
                return Array.Empty<string>();

            using StringReader sr = new(argument);
            using TextFieldParser parser = new(sr) { HasFieldsEnclosedInQuotes = true, Delimiters = delimiters };
            return parser.ReadFields()!;/*Not null if argument is not null*/
        }

        public string ToCamelCase(string s)
        {
            string[] parts = SplitWithQuoteQualifier(s, MiscellaneousConstants.PERIODSTRING);
            parts = parts.Select(p => ConvertToCamelCase(p)).ToArray();

            return string.Join(MiscellaneousConstants.PERIODSTRING, parts);
        }

        public string ToShortName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return string.Empty;

            string[] parts = fullName.Split(new char[] { MiscellaneousConstants.PERIOD }, StringSplitOptions.RemoveEmptyEntries);
            return parts[^1];
        }

        private static string ConvertToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))//Quit if first character is already lowercase
                return s;

            char[] charArray = s.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                //If the second character is not upper case stop processing
                if (i == 1 && !char.IsUpper(charArray[i]))
                    break;

                //i is between the first and last characters AND the next character is lower case
                //i.e. if all previous characters were uppercase then keep setting charcters to lower case
                // until the next charArray[i + 1] is lower case.
                if (i > 0
                    && (i + 1 < charArray.Length)
                    && !char.IsUpper(charArray[i + 1]))
                    break;

                charArray[i] = char.ToLowerInvariant(charArray[i]);
            }

            return new string(charArray);
        }
    }
}
