using WavefrontObj;
using PSXExport.CExport;
using System;

class PSXObj
{
    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Improper usage.");
            return -1;
        }

        try
        {
            new CExport(new WavefrontObjFile(args[0]));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return -1;
        }

        return 0;
    }
}
