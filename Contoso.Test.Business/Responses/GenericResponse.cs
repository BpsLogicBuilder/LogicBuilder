namespace Contoso.Test.Business.Responses
{
    public class GenericResponse<A, B> : BaseResponse
    {
        public GenericResponse(A aProperty, B bPropert)
        {
            AProperty = aProperty;
            BProperty = bPropert;
        }

        public A AProperty { get; set; }
        public B BProperty { get; set; }
    }
}
