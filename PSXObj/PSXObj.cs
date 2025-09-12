using WavefrontObj;
using PSXExport.CExport;
using System;

class PSXObj
{
    public static void Main(string[] args)
    {
        //Console.ReadLine();
        if (args.Length == 0)
        {
            Console.WriteLine("Improper usage.");
            return;
        }

        try
        {
            WavefrontObjFile w = new WavefrontObjFile(args[0]);
            CExport ce = new CExport(w);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
