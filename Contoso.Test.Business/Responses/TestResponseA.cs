namespace Contoso.Test.Business.Responses
{
    internal class TestResponseA : BaseResponse
    {
        public TestResponseA(string stringProperty)
        {
            StringProperty = stringProperty;
        }

        public string StringProperty { get; set; }
    }
}
