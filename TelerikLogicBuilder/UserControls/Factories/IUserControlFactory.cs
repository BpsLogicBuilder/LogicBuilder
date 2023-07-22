namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal interface IUserControlFactory
    {
        ConfigurationExplorer GetConfigurationExplorer();
        DocumentsExplorer GetDocumentsExplorer();
        IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel();
        IProjectExplorer GetProjectExplorer();
        IMessages GetMessages();
        RichInputBoxMessagePanel GetRichInputBoxMessagePanel();
        IRichTextBoxPanel GetRichTextBoxPanel();
        RulesExplorer GetRulesExplorer();
    }
}
