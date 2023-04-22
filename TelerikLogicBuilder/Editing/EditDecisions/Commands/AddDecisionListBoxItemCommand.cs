using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class AddDecisionListBoxItemCommand : ClickCommandBase
    {
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionListBoxItemFactory _decisionListBoxItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionsForm editDecisionsForm;

        public AddDecisionListBoxItemCommand(
            IDecisionDataParser decisionDataParser,
            IDecisionListBoxItemFactory decisionListBoxItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionsForm editDecisionsForm)
        {
            _decisionDataParser = decisionDataParser;
            _decisionListBoxItemFactory = decisionListBoxItemFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionsForm = editDecisionsForm;
        }

        private HelperButtonTextBox TxtEditDecision => editDecisionsForm.TxtEditDecision;
        private IRadListBoxManager<IDecisionListBoxItem> RadListBoxManager => editDecisionsForm.RadListBoxManager;

        public override void Execute()
        {
            string decisionXml = GetDecisionXmlString();
            if  (decisionXml.Length == 0)
                return;

            XmlElement decisionElement = _xmlDocumentHelpers.ToXmlElement(decisionXml);

            RadListBoxManager.Add
            (
                _decisionListBoxItemFactory.GetDecisionListBoxItem
                (
                    _decisionDataParser.Parse(decisionElement).VisibleText,
                    decisionElement.OuterXml,
                    editDecisionsForm
                )
            );

            string GetDecisionXmlString()
            {
                return TxtEditDecision.Tag?.ToString() ?? string.Empty;
            }
        }
    }
}
