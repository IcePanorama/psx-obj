using WavefrontObj;
using System.Collections.Generic;
using System.IO;

namespace PSXExport
{
    /// Outputs a given Wavefront OBJ file as an ANSI-C compliant 'h.in' header
    /// file. Ideal for when you're still loading your homebrew project via
    /// serial (or if you're just not doing disc I/O stuff yet).
    class HeaderExport : PSXExport
    {
        /// Format of the exported file. `{0}` should be the filename in all
        /// caps. `{1}` should be lists of four numbers (in the range of
        /// `int16_t`), where the 4th number is always zero. `{2}` is the
        /// number of tris in the 3d model times 3. `{3}` should be a list of 
        /// pointers towards vertices in `{0}_VERTS`.
        string fileFmt = """
            /*
             * File generated using PSXObj.
             * Homepage: <TODO>
             * GitHub: https://github.com/IcePanorama/psx-obj
             */
            #ifndef _PSXOBJ_{0}_MODEL_DATA_H_IN_
            #define _PSXOBJ_{0}_MODEL_DATA_H_IN_

            const SVECTOR {0}_VERTS[] = {
                {1}
            };

            const SVECTOR *{0}_TRIS[{2}] = {
                {3}
            };
            #endif /* _PSXOBJ_{0}_MODEL_DATA_H_IN_ */
            """;

        HeaderExport(string filename, List<Face> tris) : base(filename, tris)
        {
            using (FileStream sw = File.OpenWrite(_filename))
            {
            }
        }
    }
}
