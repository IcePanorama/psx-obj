using WavefrontObj;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

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

            const SVECTOR {0}_VERTS[] = {{
            {1}
            }};

            const SVECTOR *{0}_TRIS[{2}] = {{
            {3}
            }};
            #endif /* _PSXOBJ_{0}_MODEL_DATA_H_IN_ */
            """;

        public HeaderExport(WavefrontObjFile w) : base(w)
        {
            using (FileStream fs = File.OpenWrite(_filename))
            {
                string nameCaps =
                    _filename.Substring(0, _filename.LastIndexOf("."))
                        .ToUpper();
                string vertStr = CreateVertsString();
                fs.Write(
                    new UTF8Encoding(true)
                    .GetBytes(
                        string.Format(fileFmt, nameCaps, vertStr,
                            w.verts.Count * 3, "")));
            }
        }

        string CreateVertsString()
        {
            const string FMT = "  {{ {0,6}, {1,6}, {2,6}, 0 }}";
            string str = "";
            for (int i = 0; i < _verts.Count - 1; i++)
            {
                Vertex v = _verts[i];
                str +=
                    string.Format(FMT + ",\n", v.x.value, v.y.value,
                        v.z.value);
            }

            Vertex last = _verts[_verts.Count - 1];
            str +=
                string.Format(FMT, last.x.value, last.y.value, last.z.value);
            return str;
        }
    }
}
