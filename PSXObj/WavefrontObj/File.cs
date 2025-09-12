using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;

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
            void processFaces(string v0, string v1, string v2)
            {
                // Blender's default output isn't clockwise?
                int[] vals =
                { int.Parse(v2) - 1, int.Parse(v1) - 1, int.Parse(v0) - 1 };

                foreach (int v in vals)
                {
                    if ((v < 0) || (verts.Count < v))
                        throw new
                            ApplicationException("Invalid vertex index: " + v);
                }

                tris.Add(new Face(vals[0], vals[1], vals[2]));
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
                            if (subs.Length != 4) // Only working on tris atm
                                throw new MalformedLineException(l);

                            processFaces(subs[1], subs[2], subs[3]);
                            break;
                        case "o":
                            if (subs.Length < 2)
                                throw new MalformedLineException(l);

                            objName = subs[1];
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
                                        "Support for elements of type "
                                        + subs[0] + " is not currently "
                                        + "implemented.");
                    }
                }
            }

            Console.WriteLine("Processing file: " + path);
            filePath = path;
            using (StreamReader sr = File.OpenText(filePath))
            {
                ProcessFile(sr);
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
