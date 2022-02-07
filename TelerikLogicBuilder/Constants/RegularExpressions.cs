namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct RegularExpressions
    {
        internal const string USERNAME = "^[^\\/\"" + @"\[\]" + ":|<>+=;,?*@]{1,20}$";
        internal const string DOMAINNAME = "^[^ \\/:" + @"\*\?" + "\"<>|]{1,63}$";
        internal const string STRONGPASSWORD = @"^(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$";
        internal const string URL = "^(http|https)://[^*?<>|]{1,500}$";
        internal const string WCFURL = @"^(http|https|net\.tcp|net\.pipe|net\.msmq)://[^*?<>|]{1,500}$";
        internal const string FILENAME = "^[^\\/:*?<>|]{1,50}$";
        internal const string FULLYQUALIFIEDCLASSNAME = @"^[A-Za-z_]{1}[A-Za-z0-9_\`\,\[\]\.\+\= ]*[A-Za-z0-9_\]]{1}$";
        internal const string FILEPATH = "^[^*?<>|]{1,230}$";
        internal const string RELATIVEURL = "^[^*?<>|]{1,500}$";
        internal const string XMLNAMEATTRIBUTE = "^[^\"" + @"\\" + "]{1,50}$";
        internal const string XMLATTRIBUTE = "^[^\"" + @"\\" + "]{1,}$";
        internal const string NOTEMPTYSTRING = "^.{1,}$";

        internal const string WINDOWSWFTYPEDOMAINSTRING = @"^[^\n]{1,1000}$";

        internal const string VARIABLEORFUNCTIONNAME = @"^[A-Za-z_]{1}[\w]*$";
        internal const string GENERICARGUMENTNAME = @"^[A-Za-z_]{1}[\w]*$";
        internal const string REFERENCEDVARIABLEORFUNCTION = @"^[A-Za-z_]{1}[A-Za-z0-9_\.]*[A-Za-z0-9_]{1}$";
        internal const string REMOVENUMERICANDNONWORD = "[^A-Za-z_]";
        internal const string REMOVENONWORD = @"[^\w]";
    }
}
