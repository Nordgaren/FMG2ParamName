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
            ExeDir = @"F:\Steam\steamapps\common\DARK SOULS REMASTERED";
            if (File.Exists($@"{ExeDir}\DARKSOULS.exe") || File.Exists($@"{ExeDir}\DarkSoulsRemastered.exe"))
                new DarkSouls1().RenameParamRows(ExeDir);

            Console.WriteLine("Patch Complete");
            Console.ReadLine();
        }
    }
}
