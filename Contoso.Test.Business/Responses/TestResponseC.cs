namespace Contoso.Test.Business.Responses
{
    internal class TestResponseC
    {
        public TestResponseC(object objectProperty)
        {
            ObjectProperty = objectProperty;
        }

        public object ObjectProperty { get; set; }
    }
}
