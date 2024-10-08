﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands
{
    internal class EditValueFunctionFormXmlCommand : ClickCommandBase
    {
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditValueFunctionForm editFunctionForm;

        public EditValueFunctionFormXmlCommand(
            IFunctionDataParser functionDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditValueFunctionForm editFunctionForm)
        {
            _functionDataParser = functionDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editFunctionForm = editFunctionForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            using IEditValueFunctionFormXml editXmlForm = disposableManager.GetEditValueFunctionFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editFunctionForm.XmlDocument),
                editFunctionForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editFunctionForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editFunctionForm.ClearMessage();
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
