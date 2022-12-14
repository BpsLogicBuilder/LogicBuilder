using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters
{
    internal class ConfigureConstructorsStateImageSetter : IConfigureConstructorsStateImageSetter
    {
        private readonly IConfigurationFolderStateImageSetter _configurationFolderStateImageSetter;
        private readonly IConstructorXmlParser _constructorXmlParser;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConfigureConstructorsStateImageSetter(
            IConfigurationFolderStateImageSetter configurationFolderStateImageSetter,
            IConstructorXmlParser constructorXmlParser,
            IParametersXmlParser parametersXmlParser,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationFolderStateImageSetter = configurationFolderStateImageSetter;
            _constructorXmlParser = constructorXmlParser;
            _parametersXmlParser = parametersXmlParser;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void SetImage(XmlElement constructorElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application)
        {
            if (!application.AssemblyAvailable)
                return;

            SetStateImage(_constructorXmlParser.Parse(constructorElement));
            _configurationFolderStateImageSetter.SetImage((StateImageRadTreeNode?)treeNode.Parent);

            void SetStateImage(Constructor constructor)
            {
                treeNode.StateImage = Properties.Resources.CheckMark;

                if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out Type? _))
                {
                    treeNode.StateImage = Properties.Resources.Error;
                    return;
                }

                IList<XmlElement> parameterElements = _xmlDocumentHelpers.GetChildElements
                (
                    constructorElement,
                    e => e.Name == XmlDataConstants.PARAMETERSELEMENT,
                    e => e.SelectMany(o => o.ChildNodes.OfType<XmlElement>())
                );

                foreach (XmlElement parameterElement in parameterElements)
                {
                    ParameterBase parameter = _parametersXmlParser.Parse(parameterElement);
                    if (parameter.ParameterCategory == ParameterCategory.Generic
                        || parameter.ParameterCategory == ParameterCategory.GenericList)
                    {
                        continue;
                    }

                    if (!_typeLoadHelper.TryGetSystemType(parameter, application, out Type? _))
                    {
                        treeNode.StateImage = Properties.Resources.Error;
                        return;
                    }
                }
            }
        }
    }
}
