using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAI_Editor.Classes
{
    class XConverter
    {
        public static Int32 TryParseStringToInt32(string str)
        {
            int output;
            Int32.TryParse(str, out output);
            return output;
        }

        public static UInt32 TryParseStringToUInt32(string str)
        {
            uint output;
            UInt32.TryParse(str, out output);
            return output;
        }
        public static Int32 TryParseStringToInt32(object str)
        {
            int output;
            Int32.TryParse(str.ToString(), out output);
            return output;
        }

        public static UInt32 TryParseStringToUInt32(object str)
        {
            uint output;
            UInt32.TryParse(str.ToString(), out output);
            return output;
        }
    }
}
