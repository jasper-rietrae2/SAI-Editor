using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes.CustomControls
{
    public class CustomTabControlX : CustomTabControl
    {
        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        for (int i = 0; i < TabCount; ++i)
        //        {
        //            if (GetTabRect(i).Contains(e.Location))
        //            {
        //                ContextMenu cm = new ContextMenu();

        //                cm.MenuItems.Add("Remove all workspaces to the right").Click += (sender, eventArgs) =>
        //                {
        //                    //! - 1 because of the "+" tab
        //                    int count = TabPages.Count - 1;

        //                    for (int x = i; x < count; ++x)
        //                    {
        //                        TabPages.RemoveAt(x);
        //                        count = TabPages.Count - 1;
        //                    }
        //                };

        //                if (i > 0)
        //                {
        //                    cm.MenuItems.Add("Remove all workspaces to the left").Click += (sender, eventArgs) =>
        //                    {
        //                        for (int x = 0; x < i; ++x)
        //                            TabPages.RemoveAt(x);
        //                    };
        //                }


        //                cm.MenuItems.Add("Remove all workspaces but this").Click += (sender, eventArgs) =>
        //                {
        //                    int count = TabPages.Count - 1;

        //                    for (int x = 0; x < count; ++x)
        //                    {
        //                        if (x != i)
        //                        {
        //                            TabPages.RemoveAt(x);
        //                            count = TabPages.Count - 1;
        //                        }
        //                    }
        //                };

        //                cm.Show(this, e.Location);
        //                break;
        //            }
        //        }
        //    }

        //    base.OnMouseUp(e);
        //}
    }
}
