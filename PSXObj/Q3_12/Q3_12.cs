using System;

public class Q3_12
{
    public static readonly int FRACTIONAL_BITS = 12;
    static readonly int ONE = 1 << FRACTIONAL_BITS;
    static readonly float MIN_SUPPORTED_VALUE = -8.0f;
    static readonly float MAX_SUPPORTED_VALUE = 7.999755859375f;

    public short value { get; private set; } = 0;

    Q3_12(short v)
    {
        value = v;
    }

    /// A signed, fixed point (Q3.12) decimal numeral. Has a valid range of -8
    /// to 7.999755859375. Not doing any rounding, just cause.
    public Q3_12(float f)
    {
        if ((f < MIN_SUPPORTED_VALUE) || (MAX_SUPPORTED_VALUE < f))
            throw new
                ApplicationException(
                    "float is outside of the range of a Q3.12 "
                    + string.Format("fixed-point number: {0}", f));

        value = (short)(f * ONE);
        Console.WriteLine(string.Format("Before: {0}, after: {1}", f, value));
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static bool operator <(Q3_12 l, Q3_12 r) => l.value < r.value;
    public static bool operator <(Q3_12 l, int r) => l.value < r;
    public static bool operator >(Q3_12 l, Q3_12 r) => l.value > r.value;
    public static bool operator >(Q3_12 l, int r) => l.value > r;
    public static Q3_12 operator -(Q3_12 op) => new Q3_12((short)(-op.value));
    public static Q3_12 operator +(Q3_12 l, Q3_12 r)
        => new Q3_12((short)(l.value - r.value));
    public static Q3_12 operator -(Q3_12 l, Q3_12 r) => l + (-r);
    public static Q3_12 operator *(Q3_12 l, Q3_12 r)
        => new Q3_12((short)(l.value * r.value));
}
