using System;
using System.Diagnostics;
using System.IO;

namespace FMG2ParamName
{
    class Program
    {
        public static string ExeDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        static void Main(string[] args)
        {
#if DEBUG
            new DarkSouls3().PatchFiles("");
#endif
            if (File.Exists($@"{ExeDir}\DARKSOULS.exe"))
            {
                Console.WriteLine("Patching Dark Souls PTDE files");
                new DarkSouls1().PatchFiles(ExeDir, false);
            }

            if (File.Exists($@"{ExeDir}\DarkSoulsRemastered.exe"))
            {
                Console.WriteLine("Patching Dark Souls Remastered files");
                new DarkSouls1().PatchFiles(ExeDir, true);
            }
            

            if (File.Exists($@"{ExeDir}..\..\DarkSoulsIII.exe"))
            {
                Console.WriteLine("Patching Dark Souls 3 files");
                new DarkSouls3().PatchFiles(ExeDir);
            }

            Console.WriteLine("Patch Complete");
            Console.ReadLine();
        }
    }
}
