class WavefrontObj
{
    string filePath;
    string? objName = null;

    public WavefrontObj(string path)
    {
        Console.WriteLine("Processing obj file: " + path);
        filePath = path;

        using (StreamReader sr = File.OpenText(filePath))
        {
            string? l;
            while ((l = sr.ReadLine()) != null)
            {
                string[] subs = l.Split(' ');
                switch (subs[0])
                {
                    case "#":
                        continue;
                    case "o":
                        if (objName != null)
                            throw new
                                ApplicationException(
                                    "Multiple objects in a single obj file "
                                    + "is not supported!");
                        objName = subs[1];
                        Console.WriteLine("Object name: " + objName);
                        break;
                    default:
                        break;
                }

                Console.WriteLine(l);
                break;
            }
        }
    }
}
