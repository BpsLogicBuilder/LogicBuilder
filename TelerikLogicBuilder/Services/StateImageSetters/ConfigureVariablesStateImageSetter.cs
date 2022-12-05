using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters
{
    internal class ConfigureVariablesStateImageSetter : IConfigureVariablesStateImageSetter
    {
        private readonly ICompareImages _compareImages;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariablesXmlParser _variablesXmlParser;

        public ConfigureVariablesStateImageSetter(
            ICompareImages compareImages,
            ITypeLoadHelper typeLoadHelper,
            IVariablesXmlParser variablesXmlParser)
        {
            _compareImages = compareImages;
            _typeLoadHelper = typeLoadHelper;
            _variablesXmlParser = variablesXmlParser;
        }

        public void SetImage(XmlElement variableElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application)
        {
            if (!application.AssemblyAvailable)
                return;

            SetStateImage(_variablesXmlParser.Parse(variableElement));
            SetStateImageFromChildNodes((StateImageRadTreeNode)treeNode.Parent);

            void SetStateImage(VariableBase variable)
            {
                if (!_typeLoadHelper.TryGetSystemType(variable, application, out Type? _))
                {
                    treeNode.StateImage = Properties.Resources.Error;
                    return;
                }

                if (variable.ReferenceCategory == ReferenceCategories.StaticReference
                        || variable.ReferenceCategory == ReferenceCategories.Type)
                {
                    if (!_typeLoadHelper.TryGetSystemType(variable.TypeName, application, out Type? _))
                    {
                        treeNode.StateImage = Properties.Resources.Error;
                        return;
                    }
                }

                treeNode.StateImage = Properties.Resources.CheckMark;
            }
        }

        private void SetStateImageFromChildNodes(StateImageRadTreeNode? treeNode)
        {
            if (treeNode == null)
                return;

            treeNode.StateImage = treeNode.Nodes
                .OfType<StateImageRadTreeNode>()
                .Any(n => _compareImages.AreEqual(n.StateImage, Properties.Resources.Error))
                    ? Properties.Resources.Error
                    : Properties.Resources.CheckMark;

            SetStateImageFromChildNodes((StateImageRadTreeNode)treeNode.Parent);
        }
    }
}
