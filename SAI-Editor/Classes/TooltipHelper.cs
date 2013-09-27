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

        public static XToolTip GetControlToolTip(Control control)
        {
            if (tooltips.ContainsKey(control.Name))
                return tooltips[control.Name];

            XToolTip tooltip = new XToolTip();
            tooltips.Add(control.Name, tooltip);
            return tooltip;
        }

        public static XToolTip GetExistingToolTip(Control control)
        {
            if (tooltips.ContainsKey(control.Name))
                return tooltips[control.Name];

            return null;
        }

        public static void DisableOrEnableAllToolTips(bool enable)
        {
            foreach (XToolTip toolTip in tooltips.Values)
                toolTip.Active = enable;
        }
    }
}
