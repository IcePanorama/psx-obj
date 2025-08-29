namespace WavefrontObj
{
    public class Face
    {
        public int[] verts { get; private set; } = new int[3];

        public Face(int v0, int v1, int v2)
        {
            verts[0] = v0;
            verts[1] = v1;
            verts[2] = v2;
        }
    }
}
