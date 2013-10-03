using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Forms
{
    public partial class RevertQueryForm : Form
    {
        public RevertQueryForm()
        {
            InitializeComponent();

            if (!Directory.Exists("Reverts"))
                Close();
        }

        private void RevertQueryForm_Load(object sender, EventArgs e)
        {
            calenderScriptsToRevert.TodayDate = DateTime.Now;

            string[] allFiles = Directory.GetFiles("Reverts");

            for (int i = 0; i < allFiles.Length; ++i)
            {
                string fileName = allFiles[i];
                fileName = fileName.Replace(@"Reverts\\", "");
                listViewScripts.Items.Add(allFiles[i]);
            }
        }
    }
}
