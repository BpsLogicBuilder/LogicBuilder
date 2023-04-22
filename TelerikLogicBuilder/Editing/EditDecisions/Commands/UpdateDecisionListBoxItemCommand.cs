using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class UpdateDecisionListBoxItemCommand : ClickCommandBase
    {
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionListBoxItemFactory _decisionListBoxItemFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionsForm editDecisionsForm;

        public UpdateDecisionListBoxItemCommand(
            IDecisionDataParser decisionDataParser,
            IDecisionListBoxItemFactory decisionListBoxItemFactory,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionsForm editDecisionsForm)
        {
            _decisionDataParser = decisionDataParser;
            _decisionListBoxItemFactory = decisionListBoxItemFactory;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionsForm = editDecisionsForm;
        }

        private HelperButtonTextBox TxtEditDecision => editDecisionsForm.TxtEditDecision;
        private IRadListBoxManager<IDecisionListBoxItem> RadListBoxManager => editDecisionsForm.RadListBoxManager;

        public override void Execute()
        {
            string decisionXml = GetDecisionXmlString();
            if (decisionXml.Length == 0)
                throw _exceptionHelper.CriticalException("{F971032C-2BD2-4C48-B030-257357662823}");

            XmlElement decisionElement = _xmlDocumentHelpers.ToXmlElement(decisionXml);

            RadListBoxManager.Update
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
