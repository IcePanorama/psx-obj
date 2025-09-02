using WavefrontObj;
using PSXExport;
using System;

class PSXObj
{
    public static void Main(string[] args)
    {
        try
        {
            WavefrontObjFile w = new WavefrontObjFile("meatball.obj");
            SourceExport he = new SourceExport(w);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
