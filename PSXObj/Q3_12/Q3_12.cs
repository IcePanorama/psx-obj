using System;
using System.Runtime.CompilerServices;

/// A signed, fixed point (Q3.12) decimal numeral. Has a valid range of -8 to
/// 7.999755859375. Not doing any rounding, just cause.
[assembly: InternalsVisibleTo("PSXObj.Tests")]
public class Q3_12
{
    static readonly int FRACTIONAL_BITS = 12;
    internal static readonly int ONE = 1 << FRACTIONAL_BITS;
    static readonly float MIN_SUPPORTED_VALUE = -8.0f;
    static readonly float MAX_SUPPORTED_VALUE = 7.999755859375f;

    public short value { get; private set; } = 0;

    public Q3_12(float f)
    {
        if ((f < MIN_SUPPORTED_VALUE) || (MAX_SUPPORTED_VALUE < f))
            throw new
                ApplicationException(
                    "float is outside of the range of a Q3.12 "
                    + string.Format("fixed-point number: {0}", f));

        value = (short)(f * ONE);
    }

    Q3_12(Q3_12 q)
    {
        value = q.value;
    }

    Q3_12(short v)
    {
        value = v;
    }

    //public float ToFloat() => value / (ONE);
    public float ToFloat() => value / (float)(ONE);
    public override string ToString() => value.ToString();

    public override bool Equals(object? o)
    {
        if ((o == null) || (o is not Q3_12))
            return false;

        return this == (Q3_12)o;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ value.GetHashCode();
    }

    public static bool operator ==(Q3_12 l, Q3_12 r) => l.value == r.value;
    public static bool operator !=(Q3_12 l, Q3_12 r) => !(l == r);
    public static Q3_12 operator -(Q3_12 op) => new Q3_12((short)(-op.value));
    public static Q3_12 operator +(Q3_12 l, Q3_12 r)
        => new Q3_12((short)(l.value + r.value));
    public static Q3_12 operator -(Q3_12 l, Q3_12 r) => l + (-r);
}
