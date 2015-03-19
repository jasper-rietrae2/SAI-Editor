using System;

namespace SAI_Editor.Classes
{
    static class CustomConverter
    {
        public static Int32 ToInt32(object str)
        {
            int output;
            Int32.TryParse(str.ToString(), out output);
            return output;
        }

        public static UInt32 ToUInt32(object str)
        {
            uint output;
            UInt32.TryParse(str.ToString(), out output);
            return output;
        }

        public static long ToInt64(object str)
        {
            long output;
            long.TryParse(str.ToString(), out output);
            return output;
        }

        public static double ToDouble(object str)
        {
            double output;
            Double.TryParse(str.ToString(), out output);
            return output;
        }

        public static float ToFloat(object str)
        {
            float output;
            float.TryParse(str.ToString(), out output);
            return output;
        }
    }
}
