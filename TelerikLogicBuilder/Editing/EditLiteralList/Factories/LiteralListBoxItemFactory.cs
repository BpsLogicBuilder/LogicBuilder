using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListBoxItemFactory : ILiteralListBoxItemFactory
    {
        public ILiteralListBoxItem GetParameterLiteralListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListParameterInputStyle listControl)
            => listControl switch
            {
                ListParameterInputStyle.HashSetForm => new LiteralHashSetFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<ILiteralElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                ListParameterInputStyle.ListForm => new LiteralListFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<ILiteralElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{25DDAA19-4F80-40DA-943A-3CEA251F186D}")),
            };

        public ILiteralListBoxItem GetVariableLiteralListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListVariableInputStyle listControl)
            => listControl switch
            {
                ListVariableInputStyle.HashSetForm => new LiteralHashSetFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<ILiteralElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                ListVariableInputStyle.ListForm => new LiteralListFormListBoxItem
                (
                    Program.ServiceProvider.GetRequiredService<ILiteralElementValidator>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    visibleText,
                    hiddenText,
                    assignedTo,
                    applicationControl
                ),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{5CA2F358-2DC5-464D-BAC7-E08482A1CE26}")),
            };
    }
}
