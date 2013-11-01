using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Database.Classes;

namespace SAI_Editor.Classes
{
    public class SmartScriptListViewItem : ListViewItem
    {

        public SmartScriptListViewItem(string text)
            : base(text)
        {
        }

        private SmartScript _script;

        public SmartScript Script
        {
            get
            {
                return _script;
            }
            set
            {
                _script = value;
            }
        }

    }
}
