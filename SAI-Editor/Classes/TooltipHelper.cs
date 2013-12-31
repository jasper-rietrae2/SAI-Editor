using System.Collections.Generic;
using System.Windows.Forms;

namespace SAI_Editor.Classes
{

    public static class ToolTipHelper
    {
        private static readonly Dictionary<string, DetailedToolTip> tooltips = new Dictionary<string, DetailedToolTip>();

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
