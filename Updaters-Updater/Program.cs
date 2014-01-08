using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updaters_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Environment.Exit(0);
                return;
            }

            Console.ReadLine();
            Process.Start(Directory.GetCurrentDirectory() + @"\SAI-Editor.exe", "RemoveUpdatersUpdater");
        }
    }
}
