using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditLiteralFunctionHelper : IEditLiteralFunctionHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditLiteralFunctionHelper(
            IExceptionHelper exceptionHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _exceptionHelper = exceptionHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private ApplicationTypeInfo Application => richInputBoxValueControl.Application;
        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Edit(Type assignedTo, XmlElement? functionElement = null)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IDataGraphEditingForm editingForm = assignedTo == typeof(bool)
                                                    ? disposableManager.GetEditBooleanFunctionForm(GetXmlDocument())
                                                    : disposableManager.GetEditValueFunctionForm(assignedTo, GetXmlDocument());

            editingForm.ShowDialog(RichInputBox);
            if (editingForm.DialogResult != DialogResult.OK)
                return;

            XmlElement resultElement = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
            (
                _xmlDocumentHelpers.ToXmlElement
                (
                    _xmlDocumentHelpers.GetDocumentElement(editingForm.XmlDocument).OuterXml
                ),
                Application
            );
            RichInputBox.SelectionProtected = false;
            RichInputBox.InsertLink
            (
                resultElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                resultElement.OuterXml,
                LinkType.Function
            );

            XmlDocument? GetXmlDocument()
            {
                if (functionElement != null)
                {
                    if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                        throw _exceptionHelper.CriticalException("{8A713C7C-C344-478C-9B68-E9D75A84D6D4}");

                    return _xmlDocumentHelpers.ToXmlDocument(functionElement.OuterXml);
                }

                return null;
            }
        }
    }
}
