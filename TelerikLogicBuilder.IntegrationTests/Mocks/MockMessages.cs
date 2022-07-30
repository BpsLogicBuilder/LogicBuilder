using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace TelerikLogicBuilder.IntegrationTests.Mocks
{
    internal class MockMessages : IMessages
    {
        public MessageTab SelectedMessageTab { set { } }

        public bool Visible { set { } }

        public void Clear(MessageTab messageTab)
        {
        }

        public void GoToNextEmptyLine(MessageTab messageTab)
        {
        }

        public void InsertLink(string text, string hyperlink, LinkType linkType, MessageTab messageTab)
        {
        }

        public void InsertLink(string text, string hyperlink, int position, LinkType linkType, MessageTab messageTab)
        {
        }

        public void InsertText(string text, MessageTab messageTab)
        {
        }

        public void InsertText(string text, int position, MessageTab messageTab)
        {
        }

        public void Select(int start, int length, MessageTab messageTab)
        {
        }
    }
}
