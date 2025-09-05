using WavefrontObj;
using PSXExport.CExport;
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
