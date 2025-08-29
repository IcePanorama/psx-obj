namespace WavefrontObj
{
    public class Face
    {
        public Vertex[] verts { get; private set; } = new Vertex[3];

        public Face(Vertex v0, Vertex v1, Vertex v2)
        {
            verts[0] = v0;
            verts[1] = v1;
            verts[2] = v2;
        }
    }
}
