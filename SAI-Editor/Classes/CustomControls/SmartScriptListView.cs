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
    public class SmartScriptListView : ForceSelectListView
    {
        private List<SmartScript> _smartScripts = new List<SmartScript>();
        private List<string> _excludedProperties = new List<string>();
        private readonly PropertyInfo[] _pinfo;
        private Stack<Color> _colors = new Stack<Color>(Constants.phaseColors);
        private Dictionary<int, Color> _phaseColors = new Dictionary<int, Color>();

        public bool EnablePhaseHighlighting { get; set; }

        public List<SmartScript> SmartScripts
        {
            get { return _smartScripts; }
        }

        public SmartScript SelectedSmartScript
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return _smartScripts.FirstOrDefault(smartScript => smartScript == ((SmartScriptListViewItem)SelectedItems[0]).Script);

                return null;
            }
        }

        public SmartScriptListView()
        {
            EnablePhaseHighlighting = false;
            _pinfo = typeof(SmartScript).GetProperties();
            _smartScripts = new List<SmartScript>();
            _excludedProperties = new List<string>();

            Init();
        }

        public SmartScriptListView(List<SmartScript> scripts, List<string> exProps = null)
        {
            EnablePhaseHighlighting = false;
            _pinfo = typeof(SmartScript).GetProperties();
            _smartScripts = scripts;
            _excludedProperties = exProps ?? new List<string>();

            Init();
        }

        public void Init(bool keepSelection = false)
        {
            int lastSelectedIndex = SelectedIndices.Count > 0 ? SelectedIndices[0] : -1;

            Items.Clear();
            Columns.Clear();

            foreach (PropertyInfo propInfo in _pinfo.Where(propInfo => !_excludedProperties.Contains(propInfo.Name)))
                Columns.Add(propInfo.Name);

            if (_smartScripts != null)
                AddSmartScripts(_smartScripts, true);

            foreach (ColumnHeader header in Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (keepSelection && lastSelectedIndex != -1)
            {
                Items[lastSelectedIndex].Selected = true;
                ((SmartScriptListViewItem)Items[lastSelectedIndex]).LastBackColor = SelectedItems[0].BackColor;
                Items[lastSelectedIndex].BackColor = Color.FromArgb(51, 153, 254);
                Items[lastSelectedIndex].ForeColor = Color.White;
            }

            _colors = new Stack<Color>(Constants.phaseColors);
            _phaseColors.Clear();

            if (_smartScripts != null)
            {
                int[] phasemasks = _smartScripts.Select(p => p.event_phase_mask).Distinct().ToArray();

                if (phasemasks.Length > Constants.phaseColors.Count)
                {
                    MessageBox.Show("There are not enough colors in the application because you are using too many different phasemasks.", "Not enough colors!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (int phasemask in phasemasks.Where(phasemask => phasemask != 0 && !_phaseColors.ContainsKey(phasemask))) _phaseColors.Add(phasemask, _colors.Pop());
            }
        }

        public int AddSmartScript(SmartScript script, bool listViewOnly = false, bool selectNewItem = false)
        {
            SmartScriptListViewItem lvi = new SmartScriptListViewItem(script.entryorguid.ToString());
            lvi.Script = script;
            lvi.Name = script.entryorguid.ToString();

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            }

            if (!listViewOnly)
                _smartScripts.Add(script);

            ListViewItem newItem = Items.Add(lvi);

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
            return newItem.Index;
        }

        public void AddSmartScripts(List<SmartScript> scripts, bool listViewOnly = false)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (SmartScript script in scripts)
            {
                SmartScriptListViewItem lvi = new SmartScriptListViewItem(script.entryorguid.ToString());
                lvi.Script = script;
                lvi.Name = script.entryorguid.ToString();

                foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
                {
                    if (_excludedProperties.Contains(propInfo.Name))
                        continue;

                    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
                }

                if (!listViewOnly)
                    _smartScripts.Add(script);

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

        public void RemoveSmartScript(SmartScript script)
        {
            foreach (SmartScriptListViewItem item in Items.Cast<SmartScriptListViewItem>().Where(item => item.Script == script))
            {
                Items.Remove(item);
                break;
            }

            _smartScripts.Remove(script);
        }

        public void ReplaceSmartScript(SmartScript script)
        {
            SmartScriptListViewItem lvi = Items.Cast<SmartScriptListViewItem>().SingleOrDefault(p => p.Script == script);

            if (lvi == null)
                return;

            lvi.SubItems.Clear();
            lvi.Name = script.entryorguid.ToString();
            lvi.Text = script.entryorguid.ToString();

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            }

            _smartScripts[_smartScripts.IndexOf(lvi.Script)] = script;

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

        public void ReplaceData(List<SmartScript> scripts, List<string> exProps = null)
        {
            _smartScripts = scripts;
            _excludedProperties = exProps ?? new List<string>();
            Init();
        }

        public void ReplaceSmartScripts(List<SmartScript> scripts)
        {
            _smartScripts = scripts;
            Init();
        }

        public void IncludeProperty(string propName)
        {
            _excludedProperties.Remove(propName);
            Init();
        }

        public void ExcludeProperty(string propName)
        {
            _excludedProperties.Add(propName);
            Init();
        }

        public void IncludeProperties(List<string> propNames)
        {
            foreach (string propName in propNames)
                _excludedProperties.Remove(propName);

            Init();
        }

        public void ExcludeProperties(List<string> propNames)
        {
            _excludedProperties.Clear();

            foreach (string propName in propNames)
                _excludedProperties.Add(propName);

            Init();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                //! This is the color given when an item is selected WITH focus...
                ((SmartScriptListViewItem)SelectedItems[0]).LastBackColor = SelectedItems[0].BackColor;
                SelectedItems[0].BackColor = Color.FromArgb(51, 153, 254);
                SelectedItems[0].ForeColor = Color.White;
            }

            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                SelectedItems[0].BackColor = ((SmartScriptListViewItem)SelectedItems[0]).LastBackColor;
                SelectedItems[0].ForeColor = Color.Black;
            }

            base.OnGotFocus(e);
        }
    }
}
