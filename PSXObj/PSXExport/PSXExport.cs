using WavefrontObj;
using System.Collections.Generic;
using System;

namespace PSXExport
{
    abstract class PSXExport
    {
        protected string _filename;
        protected List<Face> _tris;

        protected PSXExport(string filename, List<Face> tris)
        {
            Console.WriteLine("Before: " + filename);
            _filename = CreateFilename(filename);
            Console.WriteLine("After: " + _filename);
            _tris = tris;
        }

        string CreateFilename(string filename)
        {
            int ext = filename.IndexOf('.');
            ext = ext != -1 ? ext : filename.Length;
            return filename.Substring(0, filename.Length - ext) + ".h";
        }
    }
}
