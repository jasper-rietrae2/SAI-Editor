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
            { Color.LightPink },
            { Color.Khaki },
            { Color.LightCyan },
            { Color.Beige },
            { Color.Aquamarine },
            { Color.Linen },
            { Color.LightSteelBlue },
            { Color.OldLace },
            { Color.PaleGreen },
            { Color.FromArgb(255, 200, 162, 255) }, // purple
            { Color.FromArgb(255, 193, 193, 193) } // grey
        };
    }
}
