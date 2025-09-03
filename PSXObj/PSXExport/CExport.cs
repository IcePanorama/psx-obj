using WavefrontObj;
using System.Text;
using System.IO;
using System;

namespace PSXExport
{
    /// Outputs a given Wavefront OBJ file as an ANSI-C compliant .C and .H
    /// file. Ideal for when you're still loading your homebrew project via
    /// serial (or if you're just not doing disc I/O stuff yet).
    class CExport : PSXExport
    {
        /// Format of the exported header file. `{0}` should be the filename in
        /// all caps. `{1}` is the number of tris in the 3d model times 3.
        /// `{2}` should be the desired name for the array of vertices.
        static readonly string H_FILE_FMT = """
            /*
             * File generated using PSXObj.
             * Homepage: <TODO>
             * GitHub: https://github.com/IcePanorama/psx-obj
             */
            #ifndef _PSXOBJ_{0}_DATA_H_
            #define _PSXOBJ_{0}_DATA_H_

            #include "sfd_gpui.h" // See: https://github.com/IcePanorama/PSXPsyQTemplate

            #define _PSXOBJ_{0}_TOTAL_N_VERTS_ ({1})
            #define _PSXOBJ_{0}_N_TRIS_ ((_PSXOBJ_{0}_TOTAL_N_VERTS_) / 3)

            // Technically not necessary, but this makes me feel better. :^)
            extern const SVECTOR {2}[];
            extern const SVECTOR *{0}_TRIS[(_PSXOBJ_{0}_TOTAL_N_VERTS_)];

            #endif /* _PSXOBJ_{0}_DATA_H_ */
            """;

        /// Format of the exported file. `{0}` should be the filename in all
        /// lowercase. `{1}` should the filename in all caps. `{2}` should be
        /// a list of initializer for SVECTORs. `{3}` should be a list of
        /// indicies of vertices contained within `{2}`.
        static readonly string C_FILE_FMT = """
            /*
             * File generated using PSXObj.
             * Homepage: <TODO>
             * GitHub: https://github.com/IcePanorama/psx-obj
             */
            #include "{0}.h"

            const SVECTOR {1}_VERTS[] = {{
            {2}
            }};

            const SVECTOR *{1}_TRIS[(_PSXOBJ_{1}_TOTAL_N_VERTS_)] = {{
            {3}
            }};
            """;

        public CExport(WavefrontObjFile w) : base(w)
        {
            void ExportFile(string path, string output)
            {
                // FIXME: Probably should throw an err here. Make a command
                // line arg later to allow overwriting.
                if (File.Exists(path))
                    File.Delete(path);

                using (FileStream fs = File.OpenWrite(path))
                {
                    fs.Write(new UTF8Encoding(true).GetBytes(output));
                    /// ANSI-C files MUST end in a newline.
                    fs.Write(new UTF8Encoding(true).GetBytes("\r\n"));
                }
            }

            string nameCaps = _filename.ToUpper();
            string vertArrName = nameCaps + "_VERTS";
            string headerTxt =
                string.Format(H_FILE_FMT, nameCaps, w.tris.Count * 3,
                    vertArrName);
            ExportFile(_filename + ".H", headerTxt);

            string nameLower = _filename.ToLower();
            string vertStr = CreateVertsString();
            string triStr = CreateTrisStrings(vertArrName);
            string srcTxt =
                string.Format(C_FILE_FMT, nameLower, nameCaps, vertStr,
                    triStr);
            ExportFile(_filename + ".C", srcTxt);
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

        string CreateTrisStrings(string vertArrName)
        {
            string FMT =
                string.Format("  &{0}[{{0}}],&{0}[{{1}}],&{0}[{{2}}]",
                    vertArrName);
            Func<Face, string> ApplyFmt =
                t => string.Format(FMT, t.verts[0], t.verts[1], t.verts[2]);

            string str = "";
            for (int i = 0; i < _tris.Count - 1; i++)
                str += ApplyFmt(_tris[i]) + ",\n";

            return str + ApplyFmt(_tris[_tris.Count - 1]);
        }
    }
}
