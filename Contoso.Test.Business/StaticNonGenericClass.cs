using System;

namespace Contoso.Test.Business
{
    public static class StaticNonGenericClass
    {
        public static bool StaticNonGenericMethod()
        {
            return true;
        }

        public static void StaticVoidMethod(string arg1, string arg2)
        {
            if (arg1 == null)
                throw new ArgumentNullException("arg1");

            if (arg2 == null)
                throw new ArgumentNullException("arg2");
        }
    }
}
