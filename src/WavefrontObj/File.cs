namespace WavefrontObj
{
    class WavefrontObjFile
    {
        string filePath;
        string? objName = null;

        public WavefrontObjFile(string path)
        {
            Console.WriteLine("Processing file: " + path);
            filePath = path;

            using (StreamReader sr = File.OpenText(filePath))
            {
                ProcessFile(sr);
            }
        }

        void setObjName(string value)
        {
            if (objName != null)
                throw new
                    ApplicationException(
                        "Multiple objects in a single obj file is "
                        + "not supported!");
            objName = value;
            Console.WriteLine("Object name: " + objName);
        }

        void ProcessFile(StreamReader sr)
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
                        setObjName(subs[1]);
                        continue;
                    case "v":
                        Vertex v = new Vertex(subs[1], subs[2], subs[3]);
                        return;
                    default:
                        break;
                }

                Console.WriteLine(l);
                break;
            }
        }
    }
}
