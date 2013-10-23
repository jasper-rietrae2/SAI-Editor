using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor
{
    public partial class AreatriggersForm : Form
    {
        public AreatriggersForm()
        {
            InitializeComponent();
            zoomPanImageBox1.Image = Image.FromFile(@"C:\Users\Sebastian\Dropbox\Public\6324.jpg");
        }
    }
}
