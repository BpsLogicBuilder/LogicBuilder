namespace TelerikLogicBuilder.Tests.Structures
{
    public struct IntDigit
    {
        public IntDigit(int i) { val = i; }
        public int val;
        // ...other members

        // User-defined conversion from Digit to int
        public static implicit operator int(IntDigit i)
        {
            return i.val;
        }
        //  User-defined conversion from int to Digit
        public static implicit operator IntDigit(int i)
        {
            return new IntDigit(i);
        }

        public static bool operator <(IntDigit a, IntDigit b)
        {
            return a.val < b.val;
        }

        public static bool operator >(IntDigit a, IntDigit b)
        {
            return a.val > b.val;
        }
    }
}
