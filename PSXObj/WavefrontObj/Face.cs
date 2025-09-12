namespace WavefrontObj
{
    public class Face : GenericVec3<int>
    {
            public int[] verts => data;
            public Face(int v0, int v1, int v2) {
                    data[0] = v0;
                    data[1] = v1;
                    data[2] = v2;
            }
    }
}
