﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands
{
    internal class EditBooleanFunctionFormXmlCommand : ClickCommandBase
    {
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditBooleanFunctionForm editFunctionForm;

        public EditBooleanFunctionFormXmlCommand(
            IFunctionDataParser functionDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditBooleanFunctionForm editFunctionForm)
        {
            _functionDataParser = functionDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editFunctionForm = editFunctionForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            using IEditBooleanFunctionFormXml editXmlForm = disposableManager.GetEditBooleanFunctionFormXml
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
