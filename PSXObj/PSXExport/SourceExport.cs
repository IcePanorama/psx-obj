using WavefrontObj;
using System.Text;
using System.IO;
using System;

namespace PSXExport
{
    /// Outputs a given Wavefront OBJ file as an ANSI-C compliant C source code
    /// file. Ideal for when you're still loading your homebrew project via
    /// serial (or if you're just not doing disc I/O stuff yet).
    class SourceExport : PSXExport
    {
        /// Format of the exported file. `{0}` should be the filename in all
        /// caps. `{1}` is the number of tris in the 3d model times 3. `{2}` 
        /// should be an initializer lists for SVECTORs. `{3}` should be a list
        /// of pointers towards vertices in `{0}_VERTS`. ANSI-C files must end
        /// in a new line.
        string srcFileFmt = """
            /*
             * File generated using PSXObj.
             * Homepage: <TODO>
             * GitHub: https://github.com/IcePanorama/psx-obj
             */
            // See: https://github.com/IcePanorama/PSXPsyQTemplate
            #include "sfd_gpui.h"

            #define _PSXOBJ_{0}_NUM_VERTICES_ ({1})
            #define _PSXOBJ_{0}_NUM_TRIS_ ((_PSXOBJ_{0}_NUM_VERTICES_) / 3)

            const unsigned int {0}_NUM_VERTS = (_PSXOBJ_{0}_NUM_VERTICES_);
            const unsigned int {0}_NUM_TRIS  = (_PSXOBJ_{0}_NUM_TRIS_);

            SVECTOR {0}_VERTS[] = {{
            {2}
            }};

            SVECTOR *{0}_TRIS[(_PSXOBJ_{0}_NUM_VERTICES_)] = {{
            {3}
            }};
            """;

        public SourceExport(WavefrontObjFile w) : base(w)
        {
            _filename += ".C";

            if (File.Exists(_filename))
                File.Delete(_filename);

            using (FileStream fs = File.OpenWrite(_filename))
            {
                string nameCaps =
                    _filename.Substring(0, _filename.Length - 2).ToUpper();
                string vertStr = CreateVertsString();
                string triStr = CreateTrisStrings(nameCaps);
                fs.Write(
                    new UTF8Encoding(true)
                    .GetBytes(
                        string.Format(srcFileFmt, nameCaps, w.tris.Count * 3,
                            vertStr, triStr)));
                fs.Write(new UTF8Encoding(true).GetBytes("\r\n"));
            }
        }

        string CreateVertsString()
        {
            const string FMT = "  {{ {0,6}, {1,6}, {2,6}, 0 }}";
            Func<Vertex, string> ApplyFmt =
                v => String.Format(FMT, v.x, v.y, v.z);

            string str = "";
            for (int i = 0; i < _verts.Count - 1; i++)
                str += ApplyFmt(_verts[i]) + ",\n";

            return str + ApplyFmt(_verts[_verts.Count - 1]);
        }

        string CreateTrisStrings(string nameCaps)
        {
            string FMT =
                string.Format("  &{0}_VERTS[{{0}}],&{0}_VERTS[{{1}}],"
                    + "&{0}_VERTS[{{2}}]", nameCaps);
            Func<Face, string> ApplyFmt =
                t => string.Format(FMT, t.verts[0], t.verts[1], t.verts[2]);

            string str = "";
            for (int i = 0; i < _tris.Count - 1; i++)
                str += ApplyFmt(_tris[i]) + "\n";

            return str + ApplyFmt(_tris[_tris.Count - 1]);
        }
    }
}
