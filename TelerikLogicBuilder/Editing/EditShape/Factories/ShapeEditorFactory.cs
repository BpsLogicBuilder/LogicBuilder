using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape.Factories
{
    internal class ShapeEditorFactory : IShapeEditorFactory
    {
        public IShapeEditor GetShapeEditor(string universalMasterName)
            => universalMasterName switch
            {
                UniversalMasterName.ACTION => Program.ServiceProvider.GetRequiredService<IActionShapeEditor>(),
                UniversalMasterName.CONDITIONOBJECT or UniversalMasterName.WAITCONDITIONOBJECT => Program.ServiceProvider.GetRequiredService<IConditionShapeEditor>(),
                UniversalMasterName.CONNECTOBJECT => Program.ServiceProvider.GetRequiredService<IConnectorShapeEditor>(),
                UniversalMasterName.DECISIONOBJECT or UniversalMasterName.WAITDECISIONOBJECT => Program.ServiceProvider.GetRequiredService<IDecisionShapeEditor>(),
                UniversalMasterName.DIALOG => Program.ServiceProvider.GetRequiredService<IDialogShapeEditor>(),
                UniversalMasterName.JUMPOBJECT => Program.ServiceProvider.GetRequiredService<IJumpShapeEditor>(),
                UniversalMasterName.MODULE => Program.ServiceProvider.GetRequiredService<IModuleShapeEditor>(),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{EB5FFEF4-2266-4569-A0DA-A2C6E30574B0}")),
            };
    }
}
