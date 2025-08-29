using WavefrontObj;
using PSXExport;

class PSXObj
{
    public static void Main(string[] args)
    {
        WavefrontObjFile w = new WavefrontObjFile("meatball.obj");
        HeaderExport he = new HeaderExport(w);
    }
}
