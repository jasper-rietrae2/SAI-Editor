using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Properties;

namespace SAI_Editor.Classes
{
    public class SmartScriptList : CList
    {
        private Stack<Color> _colors = new Stack<Color>(Constants.phaseColors);
        private readonly Dictionary<int, Color> _phaseColors = new Dictionary<int, Color>();

        private readonly FastObjectListView _oListView;

        public SmartScript SelectedScript
        {
            get
            {
                if (_oListView.SelectedObjects.Count > 0)
                    return (Scripts.FirstOrDefault(script => script == (SmartScript)_oListView.SelectedObject) as SmartScript);

                return null;
            }
        }

        public List<SmartScript> SmartScripts
        {
            get { return Scripts.Cast<SmartScript>().ToList(); }
        }

        public SmartScriptList(FastObjectListView listView)
            : base(listView, typeof(SmartScript))
        {
            _oListView = listView;
            _oListView.FormatRow += oListView_FormatRow;
        }

        protected override string GetListViewKey()
        {
            return "entryorguid";
        }

        protected override string GetListViewKeyValue(DatabaseClass script)
        {
            return (((SmartScript)script).entryorguid).ToString();
        }

        public override void Apply(bool keepSelection = false)
        {
            int lastSelectedIndex = _oListView.SelectedIndices.Count > 0 ? _oListView.SelectedIndices[0] : -1;
            base.Apply(keepSelection);

            //TODO: Fix me
            //if (keepSelection && lastSelectedIndex != -1)
            //{
            //    oListView.Items[lastSelectedIndex].Selected = true;
            //    ((CustomListViewItem)oListView.Items[lastSelectedIndex]).LastBackColor = oListView.SelectedItems[0].BackColor;
            //    oListView.Items[lastSelectedIndex].BackColor = Color.FromArgb(51, 153, 254);
            //    oListView.Items[lastSelectedIndex].ForeColor = Color.White;
            //}

            _colors = new Stack<Color>(Constants.phaseColors);
            _phaseColors.Clear();

            if (Scripts != null)
            {
                int[] phasemasks = Scripts.Select(p => ((SmartScript)p).event_phase_mask).Distinct().ToArray();

                if (phasemasks.Length > Constants.phaseColors.Count)
                {
                    MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (int phasemask in phasemasks.Where(phasemask => phasemask != 0 && !_phaseColors.ContainsKey(phasemask)))
                    _phaseColors.Add(phasemask, _colors.Pop());
            }
        }

        public override object Clone()
        {
            SmartScriptList newList = new SmartScriptList(ListView);
            newList.Scripts = new List<DatabaseClass>(Scripts);
            newList.ExcludedProperties = new List<string>(ExcludedProperties);

            return newList;
        }

        public void AddScript(SmartScript script, bool listViewOnly = false, bool selectNewItem = false)
        {
            base.AddScript(script, listViewOnly, selectNewItem);

            //TODO: Fix me
            //if (Settings.Default.PhaseHighlighting && script.event_phase_mask != 0)
            //{
            //    if (!_phaseColors.ContainsKey(script.event_phase_mask))
            //    {
            //        if (_colors.Count == 0)
            //        {
            //            MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return -1;
            //        }

            //        _phaseColors.Add(script.event_phase_mask, _colors.Pop());
            //    }

            //    newItem.BackColor = _phaseColors[script.event_phase_mask];
            //}

            ListView.SelectObject(script);
            _oListView.Select();
            _oListView.EnsureModelVisible(script);
        }

        private void oListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            SmartScript script = (SmartScript)e.Model;

            if (script != null && _phaseColors.ContainsKey(script.event_phase_mask))
                e.Item.BackColor = _phaseColors[script.event_phase_mask];
        }

    }
}
