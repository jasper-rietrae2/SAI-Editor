using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SAI_Editor.Properties;
using System.Drawing;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes.CustomControls
{
    public class SmartScriptListView : CustomListView
    {
        private Stack<Color> _colors = new Stack<Color>(Constants.phaseColors);
        private Dictionary<int, Color> _phaseColors = new Dictionary<int, Color>();

        public bool EnablePhaseHighlighting { get; set; }

        public SmartScriptListView()
            : base(CustomListViewType.CustomListViewSmartScript)
        {

        }

        public SmartScriptListView(List<DatabaseClass> scripts, List<string> exProps = null)
            : base(CustomListViewType.CustomListViewSmartScript, scripts, exProps)
        {
        }

        public SmartScript SelectedScript
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return (_scripts.FirstOrDefault(script => script == ((CustomListViewItem)SelectedItems[0]).Script) as SmartScript);

                return null;
            }
        }

        public List<SmartScript> Scripts
        {
            get { return _scripts.Cast<SmartScript>().ToList(); }
        }

        public override void Init(bool keepSelection = false)
        {
            int lastSelectedIndex = SelectedIndices.Count > 0 ? SelectedIndices[0] : -1;
            base.Init(keepSelection);

            if (keepSelection && lastSelectedIndex != -1)
            {
                Items[lastSelectedIndex].Selected = true;
                ((CustomListViewItem)Items[lastSelectedIndex]).LastBackColor = SelectedItems[0].BackColor;
                Items[lastSelectedIndex].BackColor = Color.FromArgb(51, 153, 254);
                Items[lastSelectedIndex].ForeColor = Color.White;
            }

            _colors = new Stack<Color>(Constants.phaseColors);
            _phaseColors.Clear();

            if (_scripts != null)
            {
                int[] phasemasks = _scripts.Select(p => (p as SmartScript).event_phase_mask).Distinct().ToArray();

                if (phasemasks.Length > Constants.phaseColors.Count)
                {
                    MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (int phasemask in phasemasks.Where(phasemask => phasemask != 0 && !_phaseColors.ContainsKey(phasemask)))
                    _phaseColors.Add(phasemask, _colors.Pop());
            }
        }

        public int AddScript(SmartScript script, bool listViewOnly = false, bool selectNewItem = false)
        {
            int index = base.AddScript(script, listViewOnly, selectNewItem);

            ListViewItem newItem = Items[index];

            if (Settings.Default.PhaseHighlighting && script.event_phase_mask != 0)
            {
                if (!_phaseColors.ContainsKey(script.event_phase_mask))
                {
                    if (_colors.Count == 0)
                    {
                        MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }

                    _phaseColors.Add(script.event_phase_mask, _colors.Pop());
                }

                newItem.BackColor = _phaseColors[script.event_phase_mask];
            }

            newItem.Selected = selectNewItem;
            Select();
            EnsureVisible(index);
            return index;
        }

        public void AddScripts(List<SmartScript> scripts, bool listViewOnly = false)
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (SmartScript script in scripts)
            {
                CustomListViewItem lvi = new CustomListViewItem(script.entryorguid.ToString());
                lvi.Script = script;
                lvi.Name = script.entryorguid.ToString();

                foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
                {
                    if (_excludedProperties.Contains(propInfo.Name))
                        continue;

                    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
                }

                if (!listViewOnly)
                    _scripts.Add(script);

                if (Settings.Default.PhaseHighlighting && script.event_phase_mask != 0)
                {
                    if (!_phaseColors.ContainsKey(script.event_phase_mask))
                    {
                        if (_colors.Count == 0)
                        {
                            MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        _phaseColors.Add(script.event_phase_mask, _colors.Pop());
                        lvi.BackColor = _phaseColors[script.event_phase_mask];
                    }

                    lvi.BackColor = _phaseColors[script.event_phase_mask];
                }

                items.Add(lvi);
            }

            Items.AddRange(items.ToArray());
        }

        public void ReplaceScript(SmartScript script)
        {
            base.ReplaceScript(script);

            CustomListViewItem lvi = Items.Cast<CustomListViewItem>().SingleOrDefault(p => p.Script == script);

            if (Settings.Default.PhaseHighlighting && script.event_phase_mask != 0)
            {
                if (!_phaseColors.ContainsKey(script.event_phase_mask))
                {
                    if (_colors.Count == 0)
                    {
                        MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _phaseColors.Add(script.event_phase_mask, _colors.Pop());
                }

                lvi.BackColor = _phaseColors[script.event_phase_mask];
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                //! This is the color given when an item is selected WITH focus...
                ((CustomListViewItem)SelectedItems[0]).LastBackColor = SelectedItems[0].BackColor;
                SelectedItems[0].BackColor = Color.FromArgb(51, 153, 254);
                SelectedItems[0].ForeColor = Color.White;
            }

            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                SelectedItems[0].BackColor = ((CustomListViewItem)SelectedItems[0]).LastBackColor;
                SelectedItems[0].ForeColor = Color.Black;
            }

            base.OnGotFocus(e);
        }
    }
}
