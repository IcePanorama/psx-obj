using WavefrontObj;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;

namespace PSXExport.CExport
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

        /// Format of the exported C file. `{0}` should be the filename in all
        /// lowercase. `{1}` should the filename in all caps. `{2}` should be
        /// a list of initializer for SVECTORs (See: `vertLineFmt`). `{3}`
        /// should be a list of indicies to vertices contained within `{2}`
        /// (See: `triLineFmtFmt`).
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

        /// How each line of the vertex array in the exported C file should be
        /// formatted.
        static readonly string vertLineFmt = "  {{ {0,6}, {1,6}, {2,6}, 0 }}";

        /// `triLineFmt` without the vertex array name applied. That needs to
        /// be done at run time. Yes, this is named in a confusing way. :^)
        static readonly string triLineFmtFmt =
            "  &{0}[{{0}}],&{0}[{{1}}],&{0}[{{2}}]";

        public CExport(WavefrontObjFile w) : base(w)
        {
            void ExportFile(string path, string output)
            {
                // FIXME: Probably should throw an err here.
                // TODO: Make a command line arg later to allow overwriting.
                if (File.Exists(path))
                    File.Delete(path);

                using (FileStream fs = File.OpenWrite(path))
                {
                    fs.Write(new UTF8Encoding(true).GetBytes(output));
                    /// ANSI-C files MUST end in a newline.
                    fs.Write(new UTF8Encoding(true).GetBytes("\r\n"));
                }
            }

            string CreateListStr<T>(List<T> l, Func<T, string> fmtFunc)
            {
                string str = "";
                for (int i = 0; i < l.Count - 1; i++)
                    str += fmtFunc(l[i]) + ",\n\r";

                return str + fmtFunc(l[l.Count - 1]);
            }

            /// Using this to clean up this ctor ever so slightly.
            /// Param:  ncaps    The object name, in all caps.
            /// Param:  arrName  The name of this objects's vertex array
            void ExportHeader(string ncaps, string arrName)
            {
                string headerTxt =
                    string.Format(H_FILE_FMT, ncaps, w.tris.Count * 3,
                        arrName);
                ExportFile(_filename + ".H", headerTxt);
            }

            /// Same as above, using this to clean up this ctor ever so
            /// slightly.
            /// Param:  ncaps    The object name, in all caps.
            /// Param:  arrName  The name of this objects's vertex array
            void ExportSource(string ncaps, string arrName)
            {
                Func<Vertex, string> vertFmtFn =
                    v => String.Format(vertLineFmt, new Q3_12(v.x), new Q3_12(v.y), new Q3_12(v.z));
                string vertStr = CreateListStr<Vertex>(_verts, vertFmtFn);

                string triLineFmt = string.Format(triLineFmtFmt, arrName);
                Func<Face, string> triFmtFn =
                    t => string.Format(triLineFmt, t.verts[0], t.verts[1],
                            t.verts[2]);
                string triStr = CreateListStr<Face>(_tris, triFmtFn);

                string srcTxt =
                    string.Format(C_FILE_FMT, _filename.ToLower(), ncaps,
                        vertStr, triStr);
                ExportFile(_filename + ".C", srcTxt);
            }

            string nameCaps = _filename.ToUpper();
            string vertArrName = nameCaps + "_VERTS";
            ExportHeader(nameCaps, vertArrName);
            ExportSource(nameCaps, vertArrName);
        }
    }
}
