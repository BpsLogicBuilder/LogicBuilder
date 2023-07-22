using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditRichInputBoxVariableCommand : ClickCommandBase
    {
        private readonly IEditLiteralVariableHelper _editLiteralVariableHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditRichInputBoxVariableCommand(
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _editLiteralVariableHelper = fieldControlHelperFactory.GetEditLiteralVariableHelper(richInputBoxValueControl);
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private IRichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public override void Execute()
        {
            if (!RichInputBox.IsSelectionEligibleForLink())
                return;

            if (RichInputBox.LinkInSelection())
            {
                int start = RichInputBox.SelectionStart;
                int finish = start + RichInputBox.SelectionLength - 1;
                if (RichInputBox.GetBoundary(start + 1)?.Equals(new LinkBoundaries(start, finish)) == true)
                {//start and finish correspond to a real boundary.
                    string xmlString = RichInputBox.GetHiddenLinkText(start + 1);
                    RichInputBox.Select(start, finish - start + 1);//GetBoundary/GetHiddenLinkText may cause the selection to change
                    XmlElement xmlElement = _xmlDocumentHelpers.ToXmlElement(xmlString);
                    if (xmlElement.Name == XmlDataConstants.VARIABLEELEMENT)
                    {
                        _editLiteralVariableHelper.Edit
                        (
                            richInputBoxValueControl.AssignedTo,
                            xmlElement
                        );
                        return;
                    }
                }
                else
                {
                    RichInputBox.Select(start, finish - start + 1);//GetBoundary may cause the selection to change
                }
            }

            _editLiteralVariableHelper.Edit(richInputBoxValueControl.AssignedTo);
        }
    }
}
