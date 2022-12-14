using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters
{
    internal class ConfigureFunctionsStateImageSetter : IConfigureFunctionsStateImageSetter
    {
        private readonly IConfigurationFolderStateImageSetter _configurationFolderStateImageSetter;
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConfigureFunctionsStateImageSetter(
            IConfigurationFolderStateImageSetter configurationFolderStateImageSetter,
            IFunctionXmlParser functionXmlParser,
            IParametersXmlParser parametersXmlParser,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationFolderStateImageSetter = configurationFolderStateImageSetter;
            _functionXmlParser = functionXmlParser;
            _parametersXmlParser = parametersXmlParser;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void SetImage(XmlElement functionElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application)
        {
            if (!application.AssemblyAvailable)
                return;

            SetStateImage(_functionXmlParser.Parse(functionElement));
            _configurationFolderStateImageSetter.SetImage((StateImageRadTreeNode?)treeNode.Parent);

            void SetStateImage(Function function)
            {
                treeNode.StateImage = Properties.Resources.CheckMark;
                if (function.ReturnType.ReturnTypeCategory == ReturnTypeCategory.Generic
                    || function.ReturnType.ReturnTypeCategory == ReturnTypeCategory.GenericList)
                {
                    return;
                }

                if (!_typeLoadHelper.TryGetSystemType(function.ReturnType, Array.Empty<GenericConfigBase>(), application, out Type? _))
                {
                    treeNode.StateImage = Properties.Resources.Error;
                    return;
                }

                if (function.ReferenceCategory == ReferenceCategories.StaticReference
                    || function.ReferenceCategory == ReferenceCategories.Type)
                {
                    if (!_typeLoadHelper.TryGetSystemType(function.TypeName, application, out Type? _))
                    {
                        treeNode.StateImage = Properties.Resources.Error;
                        return;
                    }
                }

                IList<XmlElement> parameterElements = _xmlDocumentHelpers.GetChildElements
                (
                    functionElement,
                    e => e.Name == XmlDataConstants.PARAMETERSELEMENT,
                    e => e.SelectMany(o => o.ChildNodes.OfType<XmlElement>())
                );

                foreach(XmlElement parameterElement in parameterElements)
                {
                    ParameterBase parameter = _parametersXmlParser.Parse(parameterElement);
                    if (parameter.ParameterCategory == ParameterCategory.Generic
                        || parameter.ParameterCategory == ParameterCategory.GenericList)
                    {
                        continue;
                    }

                    if(!_typeLoadHelper.TryGetSystemType(parameter, application, out Type? _))
                    {
                        treeNode.StateImage = Properties.Resources.Error;
                        return;
                    }
                }
            }
        }
    }
}
