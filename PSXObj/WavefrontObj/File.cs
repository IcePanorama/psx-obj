using System;
using System.IO;
using System.Collections.Generic;

namespace WavefrontObj
{
    public class WavefrontObjFile
    {
        string filePath;
        string? objName = null;
        List<Vertex> verts = new List<Vertex>();

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
                        continue; // skip comments
                    case "o":
                        setObjName(subs[1]);
                        continue;
                    case "s":
                        Console.WriteLine(
                                "WARNING: ignoring smooth shading element "
                                + "of value: " + subs[1]);
                        continue;
                    case "v":
                        verts.Add(new Vertex(subs[1], subs[2], subs[3]));
                        continue;
                    default:
                        break;
                }

                Console.WriteLine(l);
                break;
            }
        }
    }
}
