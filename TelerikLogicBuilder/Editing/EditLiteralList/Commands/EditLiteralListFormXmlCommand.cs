using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class EditLiteralListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditLiteralListForm editLiteralListForm;

        public EditLiteralListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditLiteralListForm editLiteralListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editLiteralListForm = editLiteralListForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditLiteralListFormXml editXmlForm = disposableManager.GetEditLiteralListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editLiteralListForm.XmlDocument),
                editLiteralListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editLiteralListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editLiteralListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editLiteralListForm.RebuildTreeView();
        }
    }
}
