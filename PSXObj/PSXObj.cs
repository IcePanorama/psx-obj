using WavefrontObj;
using PSXExport.CExport;
using System;

class PSXObj
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Improper usage.");
            return;
        }

        try
        {
            WavefrontObjFile w = new WavefrontObjFile(args[0]);
            CExport he = new CExport(w);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
