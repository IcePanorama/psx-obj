using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;

// tmp
using System.Diagnostics;

namespace WavefrontObj
{
    public class WavefrontObjFile
    {
        public string filePath { get; private set; }
        string? objName = null;
        public List<Vertex> verts { get; private set; } = new List<Vertex>();
        public List<Face> tris { get; private set; } = new List<Face>();
        public List<int[]> texCoords { get; private set; } = new List<int[]>();

        public WavefrontObjFile(string path)
        {
            void validateIndicies(int[] idx, int listLen, string errMsg)
            {
                foreach (int i in idx)
                {
                    if ((i < 0) || (listLen < i))
                        throw new ApplicationException(errMsg + i);
                }
            }

            void processFaces(string v0, string v1, string v2)
            {
                string[][] subs =
                        { v0.Split('/'), v1.Split('/'), v2.Split('/') };

                // Blender's default output is counterclockwise (b/c OpenGL)
                int[] vIdx = {
                    int.Parse(subs[2][0]) - 1,
                    int.Parse(subs[1][0]) - 1,
                    int.Parse(subs[0][0]) - 1
                };

                int[] tIdx = {
                    int.Parse(subs[2][1]) - 1,
                    int.Parse(subs[1][1]) - 1,
                    int.Parse(subs[0][1]) - 1
                };

                validateIndicies(vIdx, verts.Count, "Invalid vertex index: ");
                validateIndicies(tIdx, texCoords.Count,
                        "Invalid texture coordinate index: ");

                tris.Add(new Face(vIdx[0], vIdx[1], vIdx[2]));
            }

            void processTexCoords(string tc0, string tc1)
            {
                Func<string, int> convert = s => (int)(float.Parse(s) * 255);
                texCoords.Add(new int[]{ convert(tc0), convert(tc1) });
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
                        case "vn":
                            Console.WriteLine("Skipping vn for now...");
                            break;
                        case "vt":
                            if (subs.Length != 3)
                                throw new MalformedLineException(l);

                            processTexCoords(subs[1], subs[2]);
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
