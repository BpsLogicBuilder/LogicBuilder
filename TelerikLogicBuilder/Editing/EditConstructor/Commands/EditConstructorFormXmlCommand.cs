using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
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

        private HelperButtonDropDownList CmbSelectConstructor => editConstructorForm.CmbSelectConstructor;

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditConstructorFormXml editXmlForm = disposableManager.GetEditConstructorFormXml
            (
                _xmlDocumentHelpers.GetXmlString(editConstructorForm.XmlDocument), 
                editConstructorForm.AssignedTo
            );
            editXmlForm.ShowDialog((IWin32Window)editConstructorForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            ConstructorData constructorData = _constructorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(editXmlForm.XmlResult)
            );
            CmbSelectConstructor.Text = constructorData.Name;
            editConstructorForm.ReloadXmlDocument(editXmlForm.XmlResult);
            editConstructorForm.RebuildTreeView();
        }
    }
}
