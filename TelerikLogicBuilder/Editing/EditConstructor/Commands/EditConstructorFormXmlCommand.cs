using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands
{
    internal class EditConstructorFormXmlCommand : ClickCommandBase
    {
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditConstructorForm editConstructorForm;

        public EditConstructorFormXmlCommand(
            IConstructorDataParser constructorDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditConstructorForm editConstructorForm)
        {
            _constructorDataParser = constructorDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editConstructorForm = editConstructorForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            using IEditConstructorFormXml editXmlForm = disposableManager.GetEditConstructorFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editConstructorForm.XmlDocument), 
                editConstructorForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editConstructorForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editConstructorForm.ClearMessage();
            ConstructorData constructorData = _constructorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(editXmlForm.XmlResult)
            );
            editConstructorForm.SetConstructorName(constructorData.Name);
            editConstructorForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editConstructorForm.RebuildTreeView();
        }
    }
}
