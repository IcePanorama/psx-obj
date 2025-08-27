using System.Diagnostics;

/// A signed, fixed point (Q3.12) decimal numeral. Has a valid range of -8 to
/// 7.999755859375. Not doing any rounding.
class Q3_12
{
    static readonly int FRACTIONAL_BITS = 12;
    static readonly float MIN_SUPPORTED_VALUE = -8.0f;
    static readonly float MAX_SUPPORTED_VALUE = 7.999755859375f;

    public short value { get; private set; } = 0;

    Q3_12(float f)
    {
        Debug.Assert((MIN_SUPPORTED_VALUE < f)
                     || Math.Abs(MIN_SUPPORTED_VALUE - f) < double.Epsilon);
        Debug.Assert((f < MAX_SUPPORTED_VALUE)
                     || Math.Abs(f - MAX_SUPPORTED_VALUE) < double.Epsilon);
        value = (short)(f * (1 << FRACTIONAL_BITS));
    }
}
