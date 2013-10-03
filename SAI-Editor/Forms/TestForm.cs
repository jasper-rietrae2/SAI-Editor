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
using SAI_Editor.Database.Classes;

namespace SAI_Editor.Forms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private SmartScriptListView smartScriptListView1;

        private async void TestForm_Load(object sender, EventArgs e)
        {

            smartScriptListView1 = new SmartScriptListView(await SAI_Editor_Manager.Instance.worldDatabase.GetSmartScripts(33303, (int)SourceTypes.SourceTypeCreature));
            smartScriptListView1.View = View.Details;

            smartScriptListView1.Size = new Size(500, 500);

            Controls.Add(smartScriptListView1);

            var sm = new SmartScript { entryorguid = 123, comment = "test" };

            smartScriptListView1.AddSmartScript(sm);

            smartScriptListView1.ExcludeProperty("source_type");
            smartScriptListView1.IncludeProperty("source_type");

            smartScriptListView1.ReplaceSmartScript(new SmartScript { entryorguid = 123, comment = "testing123" });


        }
    }
}
