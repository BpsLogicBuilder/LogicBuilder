using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class EditParameterLiteralListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditParameterLiteralListForm editParameterLiteralListForm;

        public EditParameterLiteralListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditParameterLiteralListForm editParameterLiteralListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editParameterLiteralListForm = editParameterLiteralListForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            using IEditLiteralListFormXml editXmlForm = disposableManager.GetEditLiteralListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editParameterLiteralListForm.XmlDocument),
                editParameterLiteralListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editParameterLiteralListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editParameterLiteralListForm.ClearMessage();
            editParameterLiteralListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editParameterLiteralListForm.RebuildTreeView();
        }
    }
}
