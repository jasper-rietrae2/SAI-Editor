using System.Drawing;
using System.Windows.Forms;

namespace SAI_Editor.Classes
{
    using SAI_Editor.Classes.Database.Classes;

    public class SmartScriptListViewItem : ListViewItem
    {
        private SmartScript _script;
        private Color _lastBackColor = Color.White;

        public SmartScriptListViewItem(string text) : base(text) { }

        public SmartScript Script
        {
            get { return _script; }
            set { _script = value; }
        }

        public Color LastBackColor
        {
            get { return _lastBackColor; }
            set { _lastBackColor = value; }
        }
    }
}
