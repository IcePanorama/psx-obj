namespace WavefrontObj
{
    public class Face //: GenericVec3<int>
    {
            public int[] verts = new int[3];
            public int[] tCoords = new int[3];

            public Face(int v0, int v1, int v2, int t0, int t1, int t2) {
                    verts[0] = v0;
                    verts[1] = v1;
                    verts[2] = v2;

                    tCoords[0] = t0;
                    tCoords[1] = t1;
                    tCoords[2] = t2;
            }
    }
}
