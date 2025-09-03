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
            CExport he = new CExport(w);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
