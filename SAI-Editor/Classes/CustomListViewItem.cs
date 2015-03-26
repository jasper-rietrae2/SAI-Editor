using System.Drawing;
using System.Windows.Forms;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes
{
    public class CustomListViewItem : ListViewItem
    {
        public Color LastBackColor = Color.White;
        public DatabaseClass Script;

        public CustomListViewItem(string text) : base(text)
        {

        }
    }
}
