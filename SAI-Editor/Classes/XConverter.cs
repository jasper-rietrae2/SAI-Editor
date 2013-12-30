using System;

namespace SAI_Editor.Classes
{
    class XConverter
    {
        public static Int32 ToInt32(string str)
        {
            int output;
            Int32.TryParse(str, out output);
            return output;
        }

        public static Int32 ToInt32(object str)
        {
            int output;
            Int32.TryParse(str.ToString(), out output);
            return output;
        }

        public static UInt32 ToUInt32(string str)
        {
            uint output;
            UInt32.TryParse(str, out output);
            return output;
        }

        public static UInt32 ToUInt32(object str)
        {
            uint output;
            UInt32.TryParse(str.ToString(), out output);
            return output;
        }

        public static long ToInt64(string str)
        {
            long output;
            long.TryParse(str, out output);
            return output;
        }

        public static long ToInt64(object str)
        {
            long output;
            long.TryParse(str.ToString(), out output);
            return output;
        }
        public static double ToDouble(string str)
        {
            double output;
            Double.TryParse(str, out output);
            return output;
        }

        public static double ToDouble(object str)
        {
            double output;
            Double.TryParse(str.ToString(), out output);
            return output;
        }
    }
}
