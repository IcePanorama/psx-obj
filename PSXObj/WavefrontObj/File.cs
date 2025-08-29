using System;
using System.IO;
using System.Collections.Generic;

namespace WavefrontObj
{
    public class WavefrontObjFile
    {
        public string filePath { get; private set; }
        string? objName = null;
        public List<Vertex> verts { get; private set; } = new List<Vertex>();
        public List<Face> tris { get; private set; } = new List<Face>();

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

        void processFaces(string v0, string v1, string v2)
        {
            int[] vals =
                { int.Parse(v0) - 1, int.Parse(v1) - 1, int.Parse(v2) - 1 };
            foreach (int v in vals)
            {
                if ((v < 0) || (verts.Count < v))
                    throw new
                        ApplicationException("Invalid vertex index: " + v);
            }
            tris.Add(new Face(verts[vals[0]], verts[vals[1]], verts[vals[2]]));
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
                        break; // skip comments
                    case "f":
                        if (subs.Length < 4)
                            throw new MalformedLineException(l);

                        processFaces(subs[1], subs[2], subs[3]);
                        break;
                    case "o":
                        if (subs.Length < 2)
                            throw new MalformedLineException(l);

                        setObjName(subs[1]);
                        break;
                    case "s":
                        if (subs.Length < 2)
                            throw new MalformedLineException(l);

                        Console.WriteLine(
                                "WARNING: ignoring smooth shading element "
                                + "of value: " + subs[1]);
                        break;
                    case "v":
                        if (subs.Length < 4)
                            throw new MalformedLineException(l);

                        verts.Add(new Vertex(subs[1], subs[2], subs[3]));
                        break;
                    default:
                        throw new
                            NotImplementedException(
                                "Support for elements of type " + subs[0]
                                + " is not currently implemented.");
                }
            }
        }
    }

    class MalformedLineException : Exception
    {
        public MalformedLineException(string line)
            : base("Malformed input: " + line)
        {
        }
    }
}
