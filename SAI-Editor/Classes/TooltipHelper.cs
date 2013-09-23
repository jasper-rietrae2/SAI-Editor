using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor
{
    class TooltipHelper
    {
        private static readonly Dictionary<string, ToolTip> tooltips = new Dictionary<string, ToolTip>();

        public TooltipHelper()
        {

        }

        public static ToolTip GetControlToolTip(string controlName)
        {
            if (tooltips.ContainsKey(controlName))
                return tooltips[controlName];

            ToolTip tooltip = new ToolTip();
            tooltips.Add(controlName, tooltip);
            return tooltip;
        }
    }
}
