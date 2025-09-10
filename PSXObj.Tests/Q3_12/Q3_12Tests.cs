namespace PSXObj.Tests
{
    public class Q3_12Tests
    {
        [Fact]
        void Q3_12_OneEquals4096()
        {
            Assert.Equal(4096, Q3_12.ONE);
        }

        [Theory]
        [InlineData(0.0f, 0)]
        [InlineData(1.0f, 4096)]
        [InlineData(-1.0f, -4096)]
        [InlineData(-8.0f, -32768)]
        [InlineData(7.999755859375f, 32767)]
        void Q3_12_ValidFloatsAreConvertedProperly(float f, short expected)
        {
            Assert.Equal(expected, (new Q3_12(f)).value);
        }

        [Theory]
        [InlineData(-8.1f)]
        [InlineData(8.0f)]
        void Q3_12_InvalidFloatsProduceApplicationException(float f)
        {
            Assert.Throws<ApplicationException>(() => new Q3_12(f));
        }
    }
}
