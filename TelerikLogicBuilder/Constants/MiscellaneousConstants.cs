using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct MiscellaneousConstants
    {
        internal const string ENUMDESCRIPTION = "enumDescription";
        internal const int MESSAGEBOXLENGTH = 1000;
        internal const char PERIOD = '.';
        internal const string PERIODSTRING = ".";
        internal const char COMMA = ',';
        internal const string COMMASTRING = ",";
        internal const char SEMICOLONCHAR = ';';
        internal const string SEMICOLONSTRING = ";";
        internal const string WILDCARDSTRING = "*";
        internal const string QUESTIONMARKSTRING = "?";
        internal const string DOUBLEPERIODSTRING = "..";
        internal const int MAXTEXTDISPLAYED = 100;
        internal const string CLOSE_PROJECT_AND_RESTART = "CLOSE_PROJECT_RESTART";
        internal const string TILDE = "~";
        internal const string DOUBLEQUOTE = "\"";
        internal const string UNDERSCORE = "_";
        internal static readonly string[] BOOEANLIST = new string[] { true.ToString(CultureInfo.CurrentCulture), false.ToString(CultureInfo.CurrentCulture) };
        internal const string GENERICTYPEFORMAT = "{0}[{1}]";
        internal const string ARRAYTYPEFORMAT = "{0}[]";
        internal static readonly char[] DEFAULT_PARAMETER_DELIMITERS = new char[] { ';' };
        internal const string DEFAULT_OBJECT_TYPE = "System.Object";
        internal static readonly HashSet<Type> Literals = new()
        {
            typeof(bool),
            typeof(DateTimeOffset),
            typeof(DateOnly),
            typeof(DateTime),
            typeof(Date),
            typeof(TimeSpan),
            typeof(TimeOnly),
            typeof(TimeOfDay),
            typeof(Guid),
            typeof(decimal),
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(char),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(string)
        };
    }
}
