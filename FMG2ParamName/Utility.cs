using SoulsFormats;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FMG2ParamName
{
    static class Utility
    {
        public static void MakeParamDefBnd(string paramdefFolder, DCX.Type compression)
        {
            var deserializedDefs = new List<PARAMDEF>();

            var defs = Directory.GetFiles(paramdefFolder);

            foreach (var def in defs)
            {
                deserializedDefs.Add(PARAMDEF.XmlDeserialize(def));
            }

            var paramdefBND = new BND4();
            var paramdefBNDFiles = new List<BinderFile>();
            var id = 0;
            foreach (var def in deserializedDefs)
            {
                paramdefBNDFiles.Add(new BinderFile(SoulsFormats.Binder.FileFlags.None, id, $@"N:\FRPG\data\INTERROOT_x64\paramdef\{def.ParamType}.paramdef", def.Write()));
                id++;
            }

            paramdefBND.Files = paramdefBNDFiles;
            paramdefBND.Compression = compression;
            paramdefBND.Write($@"{paramdefFolder}\paramdef.paramdefbnd{(paramdefBND.Compression == DCX.Type.None ? "" : ".dcx")}");
        }

        public static byte[] GetEmbededResource(string res)
        {
            var assembly = typeof(FMG2ParamName.Program).GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(res);
            var ms = new MemoryStream();
            resource.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
