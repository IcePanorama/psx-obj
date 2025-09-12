namespace WavefrontObj
{
    public class Vertex : GenericVec3<float>
    {
        public float[] coords => data;
        public float x => coords[0];
        public float y => coords[1];
        public float z => coords[2];

        public Vertex(string x, string y, string z)
        {
            data[0] = (float.Parse(x));
            data[1] = (float.Parse(y));
            data[2] = (float.Parse(z));
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
