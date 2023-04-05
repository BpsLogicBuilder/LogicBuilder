using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands
{
    internal class EditDialogFunctionFormXmlCommand : ClickCommandBase
    {
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDialogFunctionForm editFunctionForm;

        public EditDialogFunctionFormXmlCommand(
            IFunctionDataParser functionDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDialogFunctionForm editFunctionForm)
        {
            _functionDataParser = functionDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editFunctionForm = editFunctionForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditDialogFunctionFormXml editXmlForm = disposableManager.GetEditDialogFunctionFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editFunctionForm.XmlDocument)
            );
            editXmlForm.ShowDialog((IWin32Window)editFunctionForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            FunctionData functionData = _functionDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(editXmlForm.XmlResult)
            );
            editFunctionForm.SetFunctionName(functionData.Name);
            editFunctionForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editFunctionForm.RebuildTreeView();
        }
    }
}
