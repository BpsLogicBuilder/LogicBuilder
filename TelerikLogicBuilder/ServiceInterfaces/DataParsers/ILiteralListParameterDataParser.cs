﻿using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface ILiteralListParameterDataParser
    {
        LiteralListParameterData Parse(XmlElement xmlElement);
    }
}
