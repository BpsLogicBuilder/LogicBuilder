using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers
{
    internal class ValidateXmlTextHelper : IValidateXmlTextHelper
    {
        private readonly IEditFormXml editXmlForm;

        public ValidateXmlTextHelper(IEditFormXml editXmlForm)
        {
            this.editXmlForm = editXmlForm;
        }

        private RichTextBox RichTextBox =>editXmlForm.RichTextBox;

        public void Setup()
        {
            RichTextBox.TextChanged += RichTextBox_TextChanged;
        }

        public void ValidateXml()
        {
            try
            {
                if (!editXmlForm.Application.AssemblyAvailable)
                {
                    editXmlForm.SetErrorMessage(editXmlForm.Application.UnavailableMessage);
                    return;
                }

                editXmlForm.ValidateSchema();
                editXmlForm.ValidateElement();

                SetValid();

            }
            catch (XmlException ex)
            {
                SetInValid(ex);
            }
            catch (XmlValidationException ex)
            {
                SetInValid(ex);
            }
            catch (LogicBuilderException ex)
            {
                SetInValid(ex);
            }
            catch (Exception ex)
            {//screw it - just in case
                SetInValid(ex);
            }
        }

        private void SetInValid(Exception ex)
        {
            editXmlForm.SetErrorMessage(ex.Message);
            editXmlForm.BtnOk.Enabled = false;
        }

        private void SetValid()
        {
            editXmlForm.SetMessage(Strings.editXmlOkLabelText);
            editXmlForm.BtnOk.Enabled = true;
        }

        #region Event Handlers
        private void RichTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateXml();
        }
        #endregion Event Handlers
    }
}
