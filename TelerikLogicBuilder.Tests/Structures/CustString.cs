namespace TelerikLogicBuilder.Tests.Structures
{
    public struct CustString
    {
        public CustString(string i) { val = i; }
        public string val;
        // ...other members

        // User-defined conversion from Digit to string
        public static implicit operator string(CustString i)
        {
            return i.val;
        }
        //  User-defined conversion from string to Digit
        public static implicit operator CustString(string i)
        {
            return new CustString(i);
        }

        public static bool operator <(CustString a, CustString b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(CustString a, CustString b)
        {
            return a.CompareTo(b) > 0;
        }

        public override int GetHashCode()
        {
            return this.val.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustString custString)
                return this == custString;
            if (obj is string aString)
                return this == new CustString(aString);
            return false;
        }


        public int CompareTo(object obj)
        {
            if (obj is not CustString)
                return -1;
            return this.val.CompareTo(((CustString)obj).val);
        }

        public static bool operator ==(CustString left, CustString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustString left, CustString right)
        {
            return !(left == right);
        }
    }
}
