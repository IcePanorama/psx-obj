namespace WavefrontObj
{
        public class GenericVec3<T>
        {
                T[] data = new T[3];

                public GenericVec3(T v0, T v1, T v2)
                {
                        data[0] = v0;
                        data[1] = v1;
                        data[2] = v2;
                }
        }
}
