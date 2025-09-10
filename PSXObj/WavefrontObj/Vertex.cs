namespace WavefrontObj
{
    public class Vertex
    {
        public float[] coords { get; private set; } = new float[3];
        public float x => coords[0];
        public float y => coords[1];
        public float z => coords[2];

        public Vertex(string x, string y, string z)
        {
            coords[0] = (float.Parse(x));
            coords[1] = (float.Parse(y));
            coords[2] = (float.Parse(z));
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
