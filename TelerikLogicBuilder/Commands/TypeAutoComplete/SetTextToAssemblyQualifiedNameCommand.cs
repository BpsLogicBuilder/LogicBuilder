using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete
{
    internal class SetTextToAssemblyQualifiedNameCommand : ClickCommandBase
    {
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IApplicationForm applicationForm;
        private readonly ITypeAutoCompleteTextControl textControl;

        public SetTextToAssemblyQualifiedNameCommand(
            ITypeLoadHelper typeLoadHelper,
            IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl)
        {
            _typeLoadHelper = typeLoadHelper;
            this.applicationForm = applicationForm;
            this.textControl = textControl;
        }

        private Type? Type
        {
            get
            {
                _typeLoadHelper.TryGetSystemType
                (
                    textControl.Text,
                    applicationForm.Application,
                    out Type? type
                );

                return type;
            }
        }

        public override void Execute()
        {
            if (!textControl.Enabled)
                return;

            if (Type?.AssemblyQualifiedName == null)
                return;

            textControl.Text = Type.AssemblyQualifiedName;
        }
    }
}
