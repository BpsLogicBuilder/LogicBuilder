﻿using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface ILiteralListParameterElementValidator
    {
        void Validate(ListOfLiteralsParameter parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}