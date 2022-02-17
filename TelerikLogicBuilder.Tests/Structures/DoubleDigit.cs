namespace TelerikLogicBuilder.Tests.Structures
{
    public class DoubleDigit
    {
        public DoubleDigit(double d) { val = d; }
        public double val;
        // ...other members

        // User-defined conversion from Digit to double
        public static implicit operator double(DoubleDigit d)
        {
            return d.val;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator DoubleDigit(double d)
        {
            return new DoubleDigit(d);
        }
    }
}
