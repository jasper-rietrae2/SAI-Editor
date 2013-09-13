using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAI_Editor.Properties;

namespace SAI_Editor
{
    class SAI_Editor_Manager
    {
        public WorldDatabase worldDatabase { get; set; }

        private static object _lock = new object();
        private static SAI_Editor_Manager _instance;

        public static SAI_Editor_Manager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new SAI_Editor_Manager();

                    return _instance;
                }
            }
        }

        public SAI_Editor_Manager()
        {
            worldDatabase = new WorldDatabase(Settings.Default.Host, Settings.Default.Port, Settings.Default.User, Settings.Default.Password, Settings.Default.Database);
        }

        public bool IsNumericString(string str)
        {
            try
            {
                Int32 strInt = Int32.Parse(str);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}
