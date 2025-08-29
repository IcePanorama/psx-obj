using WavefrontObj;
using System.Text;
using System.IO;

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
        string fileFmt = """
            /*
             * File generated using PSXObj.
             * Homepage: <TODO>
             * GitHub: https://github.com/IcePanorama/psx-obj
             */
            // See: https://github.com/IcePanorama/PSXPsyQTemplate
            #include "sfd_gpui.h"

            #define _PSXOBJ_{0}_NUM_TRIS_ ({1})

            const SVECTOR {0}_VERTS[] = {{
            {2}
            }};

            const SVECTOR *{0}_TRIS[(_PSXOBJ_{0}_NUM_TRIS_)] = {{
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
                        string.Format(fileFmt, nameCaps, w.tris.Count * 3,
                            vertStr, triStr)));
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
            return str +
                string.Format(FMT, last.x.value, last.y.value, last.z.value);
        }

        string CreateTrisStrings(string nameCaps)
        {
            const string FMT = "  &{0}, &{1}, &{2}";
            string VERTS_ARR_NAME =
                string.Format("{0}_VERTS[{{0}}]", nameCaps);

            string str = "";
            for (int i = 0; i < _tris.Count - 1; i++)
            {
                Face t = _tris[i];
                str += string.Format(FMT + ",\n",
                    string.Format(VERTS_ARR_NAME, t.verts[0]),
                    string.Format(VERTS_ARR_NAME, t.verts[1]),
                    string.Format(VERTS_ARR_NAME, t.verts[2]));
            }

            Face last = _tris[_tris.Count - 1];
            return str +
                string.Format(FMT,
                    string.Format(VERTS_ARR_NAME, last.verts[0]),
                    string.Format(VERTS_ARR_NAME, last.verts[1]),
                    string.Format(VERTS_ARR_NAME, last.verts[2]));
        }
    }
}
