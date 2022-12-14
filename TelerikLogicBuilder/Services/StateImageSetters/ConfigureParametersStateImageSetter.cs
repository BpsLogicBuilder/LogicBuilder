using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters
{
    internal class ConfigureParametersStateImageSetter : IConfigureParametersStateImageSetter
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ConfigureParametersStateImageSetter(
            IExceptionHelper exceptionHelper,
            IParametersXmlParser parametersXmlParser,
            ITypeLoadHelper typeLoadHelper)
        {
            _exceptionHelper = exceptionHelper;
            _parametersXmlParser = parametersXmlParser;
            _typeLoadHelper = typeLoadHelper;
        }

        public void SetImage(XmlElement parameterElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application)
        {
            SetStateImage(_parametersXmlParser.Parse(parameterElement));

            if 
            (
                !new HashSet<string>
                {
                    XmlDataConstants.LITERALPARAMETERELEMENT,
                    XmlDataConstants.OBJECTPARAMETERELEMENT,
                    XmlDataConstants.GENERICPARAMETERELEMENT,
                    XmlDataConstants.LITERALLISTPARAMETERELEMENT,
                    XmlDataConstants.OBJECTLISTPARAMETERELEMENT,
                    XmlDataConstants.GENERICLISTPARAMETERELEMENT
                }.Contains(parameterElement.Name)
            )
            throw _exceptionHelper.CriticalException("{9BB7CC94-6B6C-40E9-81D7-52FD5D01EA85}");

            void SetStateImage(ParameterBase parameter)
            {
                if (parameter.ParameterCategory == ParameterCategory.Generic
                    || parameter.ParameterCategory == ParameterCategory.GenericList)
                {
                    treeNode.StateImage = Properties.Resources.CheckMark;
                    return;
                }

                if (!_typeLoadHelper.TryGetSystemType(parameter, application, out Type? _))
                {
                    treeNode.StateImage = Properties.Resources.Error;
                    return;
                }

                treeNode.StateImage = Properties.Resources.CheckMark;
            }
        }
    }
}
