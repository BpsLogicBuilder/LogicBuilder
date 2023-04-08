using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete
{
    internal class AddUpdateGenericArgumentsCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateGenericArguments _updateGenericArguments;

        private readonly IApplicationHostControl applicationForm;
        private readonly ITypeAutoCompleteTextControl textControl;

        public AddUpdateGenericArgumentsCommand(
            IExceptionHelper exceptionHelper,
            IServiceFactory serviceFactory,
            ITypeLoadHelper typeLoadHelper,
            IApplicationHostControl applicationForm,
            ITypeAutoCompleteTextControl textControl)
        {
            _exceptionHelper = exceptionHelper;
            _typeLoadHelper = typeLoadHelper;
            _updateGenericArguments = serviceFactory.GetUpdateGenericArguments(applicationForm);
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

            if (Type == null)
                throw _exceptionHelper.CriticalException("{EA379A8C-17E7-470F-9F32-9E5E823B4790}");

            if (!Type.IsGenericType)
                throw _exceptionHelper.CriticalException("{EBC57B8C-FD90-46DF-9C62-7B1BE124599D}");

            ((Control)applicationForm).Cursor = Cursors.WaitCursor;
            _updateGenericArguments.Update(textControl);
            ((Control)applicationForm).Cursor = Cursors.Default;
        }
    }
}
