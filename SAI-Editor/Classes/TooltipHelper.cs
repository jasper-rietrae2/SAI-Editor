using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Classes;

namespace SAI_Editor
{
    class TooltipHelper
    {
        private static readonly Dictionary<string, XToolTip> tooltips = new Dictionary<string, XToolTip>();

        public TooltipHelper()
        {

        }

        public static XToolTip GetControlToolTip(string controlName)
        {
            if (tooltips.ContainsKey(controlName))
                return tooltips[controlName];

            XToolTip tooltip = new XToolTip();
            tooltips.Add(controlName, tooltip);
            return tooltip;
        }

        public static XToolTip GetExistingToolTip(string controlName)
        {
            if (tooltips.ContainsKey(controlName))
                return tooltips[controlName];

            return null;
        }
    }
}
