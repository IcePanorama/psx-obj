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
            void Swap<T>(ref T x, ref T y)
            {
                T tmp = x;
                x = y;
                y = tmp;
            }

            int FindIdxOfTopLeftMostPt(Vertex[] v)
            {
                int topLeftIdx = 0;
                for (int i = 1; i < 3; i++)
                {
                    Vertex curr = v[topLeftIdx];
                    Vertex newest = v[i];
                    if (newest.y < curr.y)
                        topLeftIdx = i;
                    else if ((newest.y == curr.y) && (newest.x < curr.x))
                        topLeftIdx = i;
                }
                return topLeftIdx;
            }

            int[] RotateArray(int[] arr, int i)
            {
                int[] res = new int[3];
                Array.Copy(arr, i, res, 0, 3 - i);
                Array.Copy(arr, 0, res, 3 - i, i);
                return res;
            }

            int[] vals =
                { int.Parse(v0) - 1, int.Parse(v1) - 1, int.Parse(v2) - 1 };

            foreach (int v in vals)
            {
                if ((v < 0) || (verts.Count < v))
                    throw new
                        ApplicationException("Invalid vertex index: " + v);
            }

            Vertex a = verts[vals[0]];
            Vertex b = verts[vals[1]];
            Vertex c = verts[vals[2]];
            bool isClkwise =
                ((b.x - a.x) * (-c.y * -a.y) - (c.x * a.x) * (-b.y - a.y)) < 0;
            if (!isClkwise)
            {
                Console.WriteLine(
                    "Vertices aren't in clockwise order: fixing them...");

                Swap<Vertex>(ref a, ref c);
                Swap<int>(ref vals[0], ref vals[2]);

                Vertex[] v = { a, b, c };
                int topLeftIdx = FindIdxOfTopLeftMostPt(v);

                vals = RotateArray(vals, topLeftIdx);
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
