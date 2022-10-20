using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IResourcesManager
    {
        /// <summary>
        /// gets index for the resource file for a format string.  The format string is used when the parameter includes a combination of text, variable or function
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        string GetShortString(XmlNode xmlNode);

        /// <summary>
        /// gets index for the resource file for a string
        /// </summary>
        /// <param name="longString"></param>
        /// <returns></returns>
        string GetShortString(string longString);
    }
}
