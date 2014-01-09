using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Updaters_Updater
{
    class Program
    {
        private const string baseRemoteDownloadPath = "http://dl.dropbox.com/u/84527004/SAI-Editor/SAI-Editor Updater/";

        //! Short explanaton: when SAI-Editor is ran and there's a new version available, it reports this to the user. If the
        //! user then updates, it runs the SAI-Editor Updater.exe. If the dropbox contains the Updater's Updater executable,
        //! it is created. Once updating is completed, the SAI-Editor is ran again. The SAI-Editor then always checks if the
        //! Updater's Updater exists and if it does, it closes itself and starts the Updater's Updater instead. This then starts
        //! replacing the SAI-Editor Updater.exe by a new executable that is obtained from Dropbox. Once the Updater's Updater
        //! finished, it closes itself and starts the SAI-Editor. The SAI-Editor then checks if it was called by the arguments
        //! 'RemoveUpdatersUpdater' and if it was, it removes the Updater's Updater. If the args are not given, it starts the
        //! Updater's Updater again.
        static void Main(string[] args)
        {
            Console.Title = "Updater for the SAI-Editor Updater";

            if (args.Length != 1 || args[0] != "RanFromSaiEditor")
            {
                Environment.Exit(0);
                return;
            }

            using (WebClient client = new WebClient())
            {
                try
                {
                    //! Download the Updater from the dropbox to the users' folder
                    string remotefile = "http://dl.dropbox.com/u/84527004/SAI-Editor/SAI-Editor Updater/SAI-Editor Updater.exe";

                    if (DoesUrlExist(remotefile))
                    {
                        string destfile = Directory.GetCurrentDirectory() + @"\SAI-Editor Updater.exe";
                        client.DownloadFile(remotefile, destfile);
                    }
                }
                catch (Exception ex)
                {
                    //! Just display the error message. Will be useful if ran from cmd so we can see the error.
                    Console.WriteLine(ex.Message);
                }
            }

            //! We start the SAI-Editor.exe with arguments to remove this executable because
            //! all it does is update the Updater.
            Process.Start(Directory.GetCurrentDirectory() + @"\SAI-Editor.exe", "RemoveUpdatersUpdater");
        }

        public static bool DoesUrlExist(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse res = req.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
    }
}
