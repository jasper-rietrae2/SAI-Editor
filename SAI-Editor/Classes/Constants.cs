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
        //! A dictionairy containing the colors for each phase in the listview
        public static Dictionary<SmartPhaseMasks /* phase */, Color /* color */> phaseColors = new Dictionary<SmartPhaseMasks, Color>()
        {
            { SmartPhaseMasks.SMART_EVENT_PHASE_ALWAYS, Color.White },
            { SmartPhaseMasks.SMART_EVENT_PHASE_1, Color.Chartreuse },
            { SmartPhaseMasks.SMART_EVENT_PHASE_2, Color.LightSkyBlue },
            { SmartPhaseMasks.SMART_EVENT_PHASE_3, Color.Thistle },
            { SmartPhaseMasks.SMART_EVENT_PHASE_4, Color.LightPink },
            { SmartPhaseMasks.SMART_EVENT_PHASE_5, Color.Khaki },
            { SmartPhaseMasks.SMART_EVENT_PHASE_6, Color.LightCyan }
        };
    }
}
