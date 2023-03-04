using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class RichInputBoxEventsHelper : IRichInputBoxEventsHelper
    {
        private readonly IEditVariableHelper _editVariableHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public RichInputBoxEventsHelper(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IVariableDataParser variableDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _editVariableHelper = fieldControlHelperFactory.GetEditVariableHelper(richInputBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _variableDataParser = variableDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private IList<RadButton> CommandButtons => richInputBoxValueControl.CommandButtons;

        private RadMenuItem MnuItemInsertConstructor => richInputBoxValueControl.MnuItemInsertConstructor;
        private RadMenuItem MnuItemInsertFunction => richInputBoxValueControl.MnuItemInsertFunction;
        private RadMenuItem MnuItemInsertVariable => richInputBoxValueControl.MnuItemInsertVariable;
        private RadMenuItem MnuItemCopy => richInputBoxValueControl.MnuItemCopy;
        private RadMenuItem MnuItemCut => richInputBoxValueControl.MnuItemCut;
        private RadMenuItem MnuItemPaste => richInputBoxValueControl.MnuItemPaste;
        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Setup()
        {
            RichInputBox.KeyUp += RichInputBox_KeyUp;
            RichInputBox.MouseClick += RichInputBox_MouseClick;
            RichInputBox.MouseUp += RichInputBox_MouseUp;
            RichInputBox.TextChanged += RichInputBox_TextChanged;
            RichInputBox.Validated += RichInputBox_Validated;
        }

        public void SetupForListItemEditor()
        {
            RichInputBox.KeyUp += RichInputBox_KeyUp;
            RichInputBox.MouseClick += RichInputBox_MouseClick;
            RichInputBox.MouseUp += RichInputBox_MouseUp;
            RichInputBox.TextChanged += RichInputBox_TextChanged;
        }

        private void EnableOrDisableCopyCutPaste()
        {
            if (RichInputBox.IsSelectionEligibleForLink() && !RichInputBox.LinkInSelection())
            {
                MnuItemCopy.Enabled = !string.IsNullOrEmpty(RichInputBox.SelectedText);
                MnuItemCut.Enabled = !string.IsNullOrEmpty(RichInputBox.SelectedText);
                MnuItemPaste.Enabled = !string.IsNullOrEmpty(Clipboard.GetText());
            }
            else
            {
                MnuItemCopy.Enabled = false;
                MnuItemCut.Enabled = false;
                MnuItemPaste.Enabled = false;
            }
        }

        private void EnableOrDisableLinkCreation()
        {
            SetEnabled(RichInputBox.IsSelectionEligibleForLink());
            void SetEnabled(bool enable)
            {
                foreach (RadButton radButton in CommandButtons)
                    radButton.Enabled = enable;

                MnuItemInsertConstructor.Enabled = enable;
                MnuItemInsertFunction.Enabled = enable;
                MnuItemInsertVariable.Enabled = enable;
            }
        }

        private void UpdateLink(string xmlString)
        {
            XmlElement xmlElement = _xmlDocumentHelpers.ToXmlElement(xmlString);
            switch (xmlElement.Name)
            {
                case XmlDataConstants.VARIABLEELEMENT:
                    _editVariableHelper.Edit(richInputBoxValueControl.AssignedTo, _variableDataParser.Parse(xmlElement));
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    //UpdateFunction(xmlString);
                    break;
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    //UpdateConstructor(xmlString);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{9389E712-1EFC-41FB-A582-548A6EF8329D}");
            }
        }

        #region Event Handlers
        private void RichInputBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (!(e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Down
                || e.KeyCode == Keys.End
                || e.KeyCode == Keys.PageDown
                || e.KeyCode == Keys.Left
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Home
                || e.KeyCode == Keys.PageUp))
                return;
            EnableOrDisableLinkCreation();
        }

        private void RichInputBox_MouseClick(object? sender, MouseEventArgs e)
        {
            int charIndex = RichInputBox.GetCharIndexFromPosition(e.Location);
            LinkBoundaries? boundary = RichInputBox.GetBoundary(charIndex);
            if (boundary != null)
            {
                string xmlString = RichInputBox.GetHiddenLinkText(charIndex);
                RichInputBox.Select(boundary.Start, boundary.Finish - boundary.Start + 1);
                UpdateLink(xmlString);
            }
        }

        private void RichInputBox_MouseUp(object? sender, MouseEventArgs e)
        {
            EnableOrDisableCopyCutPaste();
            EnableOrDisableLinkCreation();
        }

        private void RichInputBox_TextChanged(object? sender, EventArgs e)
            => richInputBoxValueControl.InvokeChanged();

        private void RichInputBox_Validated(object? sender, EventArgs e)
        {
            if (RichInputBox.Modified)
            {
                richInputBoxValueControl.RequestDocumentUpdate();
                RichInputBox.Modified = false;
            }
        }
        #endregion Event Handlers
    }
}
