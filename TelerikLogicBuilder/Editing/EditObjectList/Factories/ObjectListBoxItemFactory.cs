using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class ObjectListBoxItemFactory : IObjectListBoxItemFactory
    {
        public IObjectListBoxItem GetParameterObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListParameterInputStyle listControl)
            => listControl switch
            {
                ListParameterInputStyle.HashSetForm => new ObjectHashSetFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<IObjectElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IObjectHashSetListBoxItemComparer>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                ListParameterInputStyle.ListForm => new ObjectListFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<IObjectElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1FBB18EC-AD29-4D87-A672-F8D52E2742B0}")),
            };

        public IObjectListBoxItem GetVariableObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListVariableInputStyle listControl)
            => listControl switch
            {
                ListVariableInputStyle.HashSetForm => new ObjectHashSetFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<IObjectElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IObjectHashSetListBoxItemComparer>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                ListVariableInputStyle.ListForm => new ObjectListFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<IObjectElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{700874A9-4298-4606-83F1-CC1662059F1C}")),
            };
    }
}
