﻿using System;

namespace Contoso.Test.Business
{
    internal class StaticGenericClassOneArgument<A>
    {
        public static bool StaticMethodOne(A aParameter)
        {
            if (aParameter == null)
                return false;

            return true;
        }

        public static A StaticMethodReturnsGenericType()
        {
            throw new NotImplementedException();
        }
    }
}
