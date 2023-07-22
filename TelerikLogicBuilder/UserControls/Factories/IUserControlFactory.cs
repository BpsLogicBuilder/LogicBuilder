namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal interface IUserControlFactory
    {
        IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel();
        IProjectExplorer GetProjectExplorer();
        IMessages GetMessages();
        IRichTextBoxPanel GetRichTextBoxPanel();
    }
}
