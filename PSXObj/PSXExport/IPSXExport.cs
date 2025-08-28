using WavefrontObj;
using System.Collections.Generic;

namespace PSXExport
{
    abstract class PSXExport
    {
        string _filename;
        List<Face> _tris;

        PSXExport(string filename, List<Face> tris)
        {
            _filename = filename;
            _tris = tris;
        }
    }
}
