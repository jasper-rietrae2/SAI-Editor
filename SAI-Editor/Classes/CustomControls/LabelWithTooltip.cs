using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes.CustomControls
{
    public partial class LabelWithTooltip : Label
    {
        public LabelWithTooltip()
        {

        }

        private int _tooltipParameterId;

        [Category("Custom")]
        public int TooltipParameterId
        {
            get { return _tooltipParameterId; }
            set { _tooltipParameterId = value; }
        }
    }
}
