using WavefrontObj;
using System.Collections.Generic;
using System;

namespace PSXExport
{
    abstract class PSXExport
    {
        protected string _filename;
        protected List<Vertex> _verts;
        protected List<Face> _tris;

        protected PSXExport(WavefrontObjFile w)
        {
            Console.WriteLine("Before: " + w.filePath);
            _filename = CreateFilename(w.filePath);
            Console.WriteLine("After: " + _filename);
            _verts = w.verts;
            _tris = w.tris;
        }

        string CreateFilename(string filename)
        {
            int ext = filename.LastIndexOf('.');
            ext = ext != -1 ? ext : filename.Length;
            return filename.Substring(0, ext) + ".h";
        }
    }
}
