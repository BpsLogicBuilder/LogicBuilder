namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal interface IUserControlFactory
    {
        IConfigurationExplorer GetConfigurationExplorer();
        IDocumentsExplorer GetDocumentsExplorer();
        IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel();
        IProjectExplorer GetProjectExplorer();
        IMessages GetMessages();
        IRichInputBoxMessagePanel GetRichInputBoxMessagePanel();
        IRichTextBoxPanel GetRichTextBoxPanel();
        IRulesExplorer GetRulesExplorer();
    }
}
