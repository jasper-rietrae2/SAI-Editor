using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes.CustomControls
{
    public partial class LineSeparator : UserControl
    {
        public LineSeparator()
        {
            //InitializeComponent();
            Paint += new PaintEventHandler(LineSeparator_Paint);
            MaximumSize = new Size(2000, 2);
            MinimumSize = new Size(0, 2);
            Width = 350;
        }

        private void LineSeparator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.DarkGray, new Point(0, 0), new Point(this.Width, 0));
            g.DrawLine(Pens.White, new Point(0, 1), new Point(this.Width, 1));
        }
    }
}
