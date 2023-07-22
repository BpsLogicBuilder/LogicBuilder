namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal interface IUserControlFactory
    {
        IConfigurationExplorer GetConfigurationExplorer();
        IDocumentsExplorer GetDocumentsExplorer();
        IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel();
        IProjectExplorer GetProjectExplorer();
        IMessages GetMessages();
        RichInputBoxMessagePanel GetRichInputBoxMessagePanel();
        IRichTextBoxPanel GetRichTextBoxPanel();
        IRulesExplorer GetRulesExplorer();
    }
}
