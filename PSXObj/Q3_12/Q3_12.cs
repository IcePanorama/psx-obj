using System;
using System.Runtime.CompilerServices;

/// A signed, fixed point (Q3.12) decimal numeral. Has a valid range of -8 to
/// 7.999755859375. Not doing any rounding, just cause.
[assembly: InternalsVisibleTo("PSXObj.Tests")]
public class Q3_12
{
    const int FRACTIONAL_BITS = 12;
    internal const int ONE = 1 << FRACTIONAL_BITS;
    const float MIN_SUPPORTED_VALUE = -8.0f;
    const float MAX_SUPPORTED_VALUE = 7.999755859375f;

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
}
