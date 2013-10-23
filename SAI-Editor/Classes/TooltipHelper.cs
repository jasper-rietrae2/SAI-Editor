using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Classes;

namespace SAI_Editor
{
    class ToolTipHelper
    {
        private static readonly Dictionary<string, DetailedToolTip> tooltips = new Dictionary<string, DetailedToolTip>();

        public ToolTipHelper()
        {

        }

        public static DetailedToolTip GetControlToolTip(Control control)
        {
            if (tooltips.ContainsKey(control.Name))
                return tooltips[control.Name];

            DetailedToolTip tooltip = new DetailedToolTip();
            tooltips.Add(control.Name, tooltip);
            return tooltip;
        }

        public static DetailedToolTip GetExistingToolTip(Control control)
        {
            if (tooltips.ContainsKey(control.Name))
                return tooltips[control.Name];

            return null;
        }

        public static void DisableOrEnableAllToolTips(bool enable)
        {
            foreach (DetailedToolTip toolTip in tooltips.Values)
                toolTip.Active = enable;
        }
    }
}
