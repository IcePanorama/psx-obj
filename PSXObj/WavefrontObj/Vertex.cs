namespace WavefrontObj
{
    public class Vertex
    {
        /*
        public Q3_12[] coords { get; private set; } = new Q3_12[3];
        public Q3_12 x => coords[0];
        public Q3_12 y => coords[1];
        public Q3_12 z => coords[2];
        */
        public float[] coords { get; private set; } = new float[3];
        public float x => coords[0];
        public float y => coords[1];
        public float z => coords[2];

        public Vertex(string x, string y, string z)
        {
            /*
            coords[0] = new Q3_12(float.Parse(x));
            coords[1] = new Q3_12(float.Parse(y));
            coords[2] = new Q3_12(float.Parse(z));
            */
            coords[0] = (float.Parse(x));
            coords[1] = (float.Parse(y));
            coords[2] = (float.Parse(z));
        }

	public Vertex(float x, float y, float z)
        {
            coords[0] = x;
            coords[1] = y;
            coords[2] = z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
