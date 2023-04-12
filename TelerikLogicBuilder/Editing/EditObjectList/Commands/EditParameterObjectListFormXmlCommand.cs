using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class EditParameterObjectListFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditParameterObjectListForm editParameterObjectListForm;

        public EditParameterObjectListFormXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditParameterObjectListForm editParameterObjectListForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editParameterObjectListForm = editParameterObjectListForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditObjectListFormXml editXmlForm = disposableManager.GetEditObjectListFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editParameterObjectListForm.XmlDocument),
                editParameterObjectListForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editParameterObjectListForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editParameterObjectListForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editParameterObjectListForm.RebuildTreeView();
        }
    }
}
