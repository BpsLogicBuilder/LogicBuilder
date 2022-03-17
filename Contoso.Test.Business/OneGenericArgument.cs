namespace Contoso.Test.Business
{
    public class OneGenericArgument<A>
    {
        public OneGenericArgument(A aProperty)
        {
            AProperty = aProperty;
        }

        public A AProperty { get; set; }
    }
}
