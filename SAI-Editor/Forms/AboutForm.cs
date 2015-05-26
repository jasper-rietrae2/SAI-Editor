using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {

        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGithub_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("https://github.com/Discover-/SAI-Editor/");
        }

        private void pictureBoxDiscover_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("https://github.com/Discover-/");
        }

        private void pictureBoxMitch_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("https://github.com/Mitch528/");
        }

        private void buttonTrinitycore_Click(object sender, EventArgs e)
        {
            SAI_Editor_Manager.Instance.StartProcess("https://github.com/TrinityCore/");
        }
    }
}
