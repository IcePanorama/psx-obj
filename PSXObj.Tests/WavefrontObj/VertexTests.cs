using WavefrontObj;

namespace PSXObj.Tests
{
    public class VertexTests
    {
        /// Not testing invalid floats as that's coverted by Q3_12Tests
        [Theory]
        [InlineData(0.0f, 0.0f, 0.0f)]
        [InlineData(0.216921f, -0.572847f, 0.159694f)]
        [InlineData(-8.0f, -1.0f, 7.999755859375f)]
        void Q3_12_ValidFloatsAreConvertedIntoVertices(
                float x,
                float y,
                float z)
        {
            Vertex v = new
                Vertex(Convert.ToString(x), Convert.ToString(y),
                       Convert.ToString(z));

            /*
            Assert.Equal(new Q3_12(x), v.x);
            Assert.Equal(new Q3_12(y), v.y);
            Assert.Equal(new Q3_12(z), v.z);
            */
            Assert.Equal(x, v.x);
            Assert.Equal(y, v.y);
            Assert.Equal(z, v.z);
        }
    }
}
