using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal abstract class ResourcesManagerUtilityBase
    {
        private readonly IExceptionHelper _exceptionHelper;

        protected ResourcesManagerUtilityBase(IExceptionHelper exceptionHelper, IDictionary<string, string> resourceStrings, string moduleName)
        {
            _exceptionHelper = exceptionHelper;
            this.resourceStrings = resourceStrings;
            this.moduleName = moduleName;
        }

        #region Variables
        protected IDictionary<string, string> resourceStrings;
        protected string moduleName;
        #endregion Variables

        protected abstract string Prefix { get; }

        /// <summary>
        /// gets index for the resource file for a format string.  The format string is used when the parameter includes a combination of text, variable or function
        /// </summary>
        /// <param name="parameterNode"></param>
        /// <returns></returns>
        internal string GetShortString(XmlNode parameterNode) 
            => MakeShortString(GetStringFormat(parameterNode));

        /// <summary>
        /// gets index for the resource file for a string
        /// </summary>
        /// <param name="longString"></param>
        /// <returns></returns>
        internal string GetShortString(string longString) 
            => MakeShortString(longString);

        /// <summary>
        /// creates index for the resource file for a string
        /// </summary>
        /// <param name="longString"></param>
        /// <returns></returns>
        private string MakeShortString(string longString)
        {
            string tempString = longString;
            string[] invalidCharacters = new string[] { "&", @"\", "/", "=", "<", ">", "#", "%", "\"", "(", ")", "{", "}", "\r", "\n" };

            foreach (string invalid in invalidCharacters)
                tempString = tempString.Replace(invalid, "A");

            string[] words = tempString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string shortString;
            if (words.Length < 2)
            {
                if (tempString.Length > 5)
                    shortString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Prefix, Strings.underscore, tempString[..6].ToUpper(CultureInfo.InvariantCulture));
                else
                    shortString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Prefix, Strings.underscore, tempString.ToUpper(CultureInfo.InvariantCulture));
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < words.Length && i < 6; i++)
                    stringBuilder.Append(words[i][0]);

                shortString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Prefix, Strings.underscore, stringBuilder.ToString().ToUpper(CultureInfo.InvariantCulture));
            }

            return EnsureUniqueness(shortString, longString);
        }

        /// <summary>
        /// ensures the index is unique in the resource file for this module
        /// </summary>
        /// <param name="shortString"></param>
        /// <param name="longString"></param>
        /// <returns></returns>
        private string EnsureUniqueness(string shortString, string longString)
        {
            if (!resourceStrings.ContainsKey(shortString))
            {
                resourceStrings.Add(shortString, longString);
                return shortString;
            }

            if (resourceStrings[shortString] == longString)
            {
                return shortString;
            }
            else
            {
                string lastChar = shortString[^1..];
                if (short.TryParse(lastChar, out short lastCharShort))
                {
                    lastCharShort++;
                    shortString = shortString[..^1];
                    shortString += lastCharShort.ToString(CultureInfo.CurrentCulture);
                    return EnsureUniqueness(shortString, longString);
                }
                else
                {
                    shortString += "0";
                    return EnsureUniqueness(shortString, longString);
                }
            }
        }

        /// <summary>
        /// creates a string format when the parameter includes one or more of text, variable, function or constructor
        /// </summary>
        /// <param name="parameterNode"></param>
        /// <returns></returns>
        private string GetStringFormat(XmlNode parameterNode)
        {
            StringBuilder stringBuilder = new();
            int argumentCount = 0;

            foreach (XmlNode childNode in parameterNode.ChildNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement xmlElement = (XmlElement)childNode;
                        switch (xmlElement.Name)
                        {
                            case XmlDataConstants.VARIABLEELEMENT:
                            case XmlDataConstants.FUNCTIONELEMENT:
                            case XmlDataConstants.CONSTRUCTORELEMENT:
                                stringBuilder.AppendFormat("{0}{1}{2}", "{", argumentCount++, "}");
                                break;
                            default:
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        XmlText xmlText = (XmlText)childNode;
                        stringBuilder.Append(xmlText.Value);
                        break;
                    case XmlNodeType.Whitespace:
                        XmlWhitespace xmlWhitespace = (XmlWhitespace)childNode;
                        stringBuilder.Append(xmlWhitespace.Value);
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{5EFFDC0A-B33A-4F72-92B9-7D6DA12ECF66}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
