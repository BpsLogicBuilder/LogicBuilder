namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal interface IUserControlFactory
    {
        IEditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel();
        IRichTextBoxPanel GetRichTextBoxPanel();
    }
}
