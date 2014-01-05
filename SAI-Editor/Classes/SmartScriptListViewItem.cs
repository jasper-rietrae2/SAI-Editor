using System.Drawing;
using System.Windows.Forms;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes
{
    public class SmartScriptListViewItem : ListViewItem
    {
        private Color _lastBackColor = Color.White;

        public SmartScriptListViewItem(string text) : base(text) { }

        public SmartScript Script { get; set; }

        public Color LastBackColor
        {
            get { return _lastBackColor; }
            set { _lastBackColor = value; }
        }
    }
}
