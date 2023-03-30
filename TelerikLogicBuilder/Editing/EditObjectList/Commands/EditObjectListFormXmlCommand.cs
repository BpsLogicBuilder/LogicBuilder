using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class EditObjectListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditObjectListForm editObjectListForm;

        public EditObjectListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditObjectListForm editObjectListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editObjectListForm = editObjectListForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditObjectListFormXml editXmlForm = disposableManager.GetEditObjectListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editObjectListForm.XmlDocument),
                editObjectListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editObjectListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editObjectListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editObjectListForm.RebuildTreeView();
        }
    }
}
