namespace Contoso.Test.Business
{
    public static class StaticGenericClass<A, B>
    {
        public static bool StaticMethod(A aParameter, B bParameter)
        {
            if(aParameter == null)
                return false;

            if (bParameter == null)
                return false;

            return true;
        }

        public static bool StaticMethodOneArgument(A aParameter)
        {
            if (aParameter == null)
                return false;
            return true;
        }

        public static B StaticMethodGenericReturn(A aParameter, B bParameter)
        {
            if (aParameter == null)
                return bParameter;

            return bParameter;
        }
    }
}
