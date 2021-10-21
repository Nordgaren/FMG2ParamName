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
            if (File.Exists($@"{ExeDir}\DARKSOULS.exe") || File.Exists($@"{ExeDir}\DarkSoulsRemastered.exe"))
                new DarkSouls1().Translate(ExeDir);

            if (File.Exists($@"{ExeDir}\DarkSoulsIII.exe") || File.Exists($@"{ExeDir}..\..\DarkSoulsIII.exe"))
                new DarkSouls3().Translate(ExeDir);

            Console.WriteLine("Patch Complete");
            Console.ReadLine();
        }
    }
}
