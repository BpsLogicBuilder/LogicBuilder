using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditVariableLiteralListHelper : IEditVariableLiteralListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IVariableRichTextBoxValueControl variableRichTextBoxValueControl;

        public EditVariableLiteralListHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.variableRichTextBoxValueControl = variableRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => variableRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? literalListElement = null)
        {
            LiteralListVariableElementInfo literalListElementInfo = variableRichTextBoxValueControl.LiteralListElementInfo;
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditVariableLiteralListForm editLiteralListForm = disposableManager.GetEditVariableLiteralListForm
            (
                assignedTo,
                literalListElementInfo,
                GetXmlDocument()
            );

            editLiteralListForm.ShowDialog(RichTextBox);
            if (editLiteralListForm.DialogResult != DialogResult.OK)
                return;

            variableRichTextBoxValueControl.UpdateXmlElement(editLiteralListForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = variableRichTextBoxValueControl.VisibleText;
            variableRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument GetXmlDocument()
            {
                if (literalListElement != null)
                {
                    if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                        throw _exceptionHelper.CriticalException("{FBB58792-78D1-4EB5-AFFF-B56861D0796C}");

                    return _xmlDocumentHelpers.ToXmlDocument(literalListElement.OuterXml);
                }

                LiteralListElementType literalVariableType = _enumHelper.GetLiteralListElementType(literalListElementInfo.LiteralType);
                return _xmlDocumentHelpers.ToXmlDocument
                (
                    _xmlDataHelper.BuildLiteralListXml
                    (
                        literalVariableType,
                        literalListElementInfo.ListType,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listVariableCountFormat,
                            literalListElementInfo.HasVariable
                                ? literalListElementInfo.Variable.Name
                                : _enumHelper.GetTypeDescription(literalListElementInfo.ListType, _enumHelper.GetVisibleEnumText(literalVariableType)),
                                0
                        ),
                        string.Empty
                    )
                );
            }
        }
    }
}
