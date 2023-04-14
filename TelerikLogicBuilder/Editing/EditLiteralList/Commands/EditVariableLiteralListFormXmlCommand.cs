using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class EditVariableLiteralListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditVariableLiteralListForm editVariableLiteralListForm;

        public EditVariableLiteralListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditVariableLiteralListForm editVariableLiteralListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editVariableLiteralListForm = editVariableLiteralListForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditLiteralListFormXml editXmlForm = disposableManager.GetEditLiteralListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editVariableLiteralListForm.XmlDocument),
                editVariableLiteralListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editVariableLiteralListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editVariableLiteralListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editVariableLiteralListForm.RebuildTreeView();
        }
    }
}
