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
    internal class EditParameterLiteralListHelper : IEditParameterLiteralListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public EditParameterLiteralListHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => parameterRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? literalListElement = null)
        {
            LiteralListParameterElementInfo literalListElementInfo = parameterRichTextBoxValueControl.LiteralListElementInfo;
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditParameterLiteralListForm editLiteralListForm = disposableManager.GetEditParameterLiteralListForm
            (
                assignedTo,
                literalListElementInfo,
                GetXmlDocument()
            );

            editLiteralListForm.ShowDialog(RichTextBox);
            if (editLiteralListForm.DialogResult != DialogResult.OK)
                return;

            parameterRichTextBoxValueControl.UpdateXmlElement(editLiteralListForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = parameterRichTextBoxValueControl.VisibleText;
            parameterRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument GetXmlDocument()
            {
                if (literalListElement != null)
                {
                    if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                        throw _exceptionHelper.CriticalException("{6248B520-9866-41D5-A553-A596A9F2D530}");

                    return _xmlDocumentHelpers.ToXmlDocument(literalListElement.OuterXml);
                }

                LiteralListElementType literalParameterType = _enumHelper.GetLiteralListElementType(literalListElementInfo.LiteralType);
                return _xmlDocumentHelpers.ToXmlDocument
                (
                    _xmlDataHelper.BuildLiteralListXml
                    (
                        literalParameterType,
                        literalListElementInfo.ListType,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listParameterCountFormat,
                            literalListElementInfo.HasParameter
                                ? literalListElementInfo.Parameter.Name
                                : _enumHelper.GetTypeDescription(literalListElementInfo.ListType, _enumHelper.GetVisibleEnumText(literalParameterType)),
                                0
                        ),
                        string.Empty
                    )
                );
            }
        }
    }
}
