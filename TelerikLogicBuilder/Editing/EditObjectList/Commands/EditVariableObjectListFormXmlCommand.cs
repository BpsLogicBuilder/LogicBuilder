using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class EditVariableObjectListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditVariableObjectListForm editVariableObjectListForm;

        public EditVariableObjectListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditVariableObjectListForm editVariableObjectListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editVariableObjectListForm = editVariableObjectListForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditObjectListFormXml editXmlForm = disposableManager.GetEditObjectListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editVariableObjectListForm.XmlDocument),
                editVariableObjectListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editVariableObjectListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editVariableObjectListForm.ClearMessage();
            editVariableObjectListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editVariableObjectListForm.RebuildTreeView();
        }
    }
}
