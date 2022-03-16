namespace Contoso.Test.Business.Responses
{
    internal class TestResponseB : BaseResponse
    {
        public TestResponseB(string stringProperty, int intProperty)
        {
            StringProperty = stringProperty;
            IntProperty = intProperty;
        }

        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
}
