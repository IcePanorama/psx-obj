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
            /// Creates a MS-DOS/8.3 compliant filename from the given
            /// filename. This base class does not add an extension, leaving
            /// that for subclasses to implement.
            string CreateFilename(string filename)
            {
                int ext = filename.LastIndexOf('.');
                ext = ext != -1 ? Math.Min(ext, 9) : filename.Length;
                return filename.Substring(0, ext).ToUpper();
            }

            Console.WriteLine("Before: " + w.filePath);
            _filename = CreateFilename(w.filePath);
            Console.WriteLine("After: " + _filename);
            _verts = w.verts;
            _tris = w.tris;
        }
    }
}
