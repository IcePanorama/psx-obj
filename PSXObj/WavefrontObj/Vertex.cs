namespace WavefrontObj
{
    public class Vertex
    {
        public Q3_12[] coords { get; private set; } = new Q3_12[3];
        public Q3_12 x => coords[0];
        public Q3_12 y => coords[1];
        public Q3_12 z => coords[2];

        public Vertex(string x, string y, string z)
        {
            coords[0] = new Q3_12(float.Parse(x));
            coords[1] = new Q3_12(float.Parse(y));
            coords[2] = new Q3_12(float.Parse(z));
        }

        public Vertex(Q3_12 x, Q3_12 y, Q3_12 z)
        {
            coords[0] = new Q3_12(x);
            coords[1] = new Q3_12(y);
            coords[2] = new Q3_12(z);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
