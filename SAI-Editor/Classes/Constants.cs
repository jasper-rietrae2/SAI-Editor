using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAI_Editor.Classes
{
    public static class Constants
    {
        public static List<Color> phaseColors = new List<Color>()
        {
            { Color.FromArgb(255, 206, 240, 125) }, // lime green
            { Color.LightSkyBlue },
            { Color.Thistle },
            //{ Color.LightPink }, //! A lot like Thistle
            { Color.Khaki },
            { Color.LightCyan },
            { Color.Beige },
            { Color.Aquamarine },
            //{ Color.Linen }, //! A lot like Beige (almost identical)
            { Color.LightSteelBlue },
            //{ Color.OldLace }, //! A lot like Beige (almost identical)
            { Color.PaleGreen },
            { Color.FromArgb(255, 200, 162, 255) }, // purple
            { Color.FromArgb(255, 193, 193, 193) }, // grey
            { Color.FromArgb(255, 240, 128, 128) }, // light coral (red-ish)
            { Color.FromArgb(255, 173, 255, 47) },  // green yellow
            { Color.FromArgb(255, 152, 251, 152) }, // pale green
            { Color.FromArgb(255, 102, 205, 170) }, // medium aqua marine
            { Color.FromArgb(255, 175, 238, 238) }, // pale turquoise
            { Color.FromArgb(255, 100, 149, 237) }  // corn flower blue
        };
    }
}
