using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Database.Classes;
using SAI_Editor.Classes;
using SAI_Editor;
using SAI_Editor.Properties;
using System.Drawing;

namespace SAI_Editor.Classes
{
    public class SmartScriptListView : ForceSelectListView
    {
        private List<SmartScript> _smartScripts = new List<SmartScript>();
        private List<string> _excludedProperties = new List<string>();
        private readonly PropertyInfo[] _pinfo;

        public SmartScriptListView()
        {
            _pinfo = typeof(SmartScript).GetProperties();
            _smartScripts = new List<SmartScript>();
            _excludedProperties = new List<string>();

            Init();
        }

        public SmartScriptListView(List<SmartScript> scripts, List<string> exProps = null)
        {
            _pinfo = typeof(SmartScript).GetProperties();
            _smartScripts = scripts;
            _excludedProperties = exProps ?? new List<string>();

            Init();
        }

        public void Init()
        {
            Items.Clear();
            Columns.Clear();

            foreach (PropertyInfo propInfo in _pinfo)
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                Columns.Add(propInfo.Name);
            }

            if (_smartScripts != null)
                AddSmartScripts(_smartScripts, true);

            foreach (ColumnHeader header in Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public List<SmartScript> SmartScripts
        {
            get
            {
                return _smartScripts;
            }
        }

        public SmartScript SelectedSmartScript
        {
            get
            {
                if (SelectedItems.Count > 0)
                    foreach (SmartScript smartScript in _smartScripts)
                        if (smartScript.entryorguid.ToString() == SelectedItems[0].SubItems[0].Text && smartScript.id.ToString() == SelectedItems[0].SubItems[2].Text)
                            return smartScript;

                return null;
            }
        }

        public SmartScript GetSmartScript(int entryorguid, int id)
        {
            foreach (SmartScript smartScript in _smartScripts)
                if (smartScript.entryorguid == entryorguid && smartScript.id == id)
                    return smartScript;

            return null;
        }

        public int AddSmartScript(SmartScript script, bool listViewOnly = false)
        {
            ListViewItem lvi = new ListViewItem(script.entryorguid.ToString());
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
            HandleHighlightItems();
            return newItem.Index;
        }

        public void HandleHighlightItems()
        {
            if (!Settings.Default.PhaseHighlighting)
                return;

            List<Color> phaseColors = new List<Color>(Constants.phaseColors);
            Dictionary<int /* phase */, List<SmartScript>> smartScriptsPhases = new Dictionary<int, List<SmartScript>>();

            foreach (SmartScript smartScript in _smartScripts)
            {
                if (!smartScriptsPhases.ContainsKey(smartScript.event_phase_mask))
                {
                    List<SmartScript> newSmartScriptList = new List<SmartScript>();
                    newSmartScriptList.Add(smartScript);
                    smartScriptsPhases[smartScript.event_phase_mask] = newSmartScriptList;
                }
                else
                    smartScriptsPhases[smartScript.event_phase_mask].Add(smartScript);
            }

            foreach (List<SmartScript> smartScripts in smartScriptsPhases.Values)
            {
                bool foundColor = false;

                foreach (SmartScript smartScript in smartScripts)
                {
                    foreach (ListViewItem item in Items)
                    {
                        if (item.Text == smartScript.entryorguid.ToString() && item.SubItems[2].Text == smartScript.id.ToString())
                        {
                            //! Try-catch has to be right here and not outside the loops. Otherwise all items coming after an
                            //! item that caused an exception will not be colored.
                            try
                            {
                                item.BackColor = phaseColors.First();
                                foundColor = true;
                                break;
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                if (foundColor)
                    phaseColors.RemoveAt(0);
            }
        }

        public void AddSmartScripts(List<SmartScript> scripts, bool listViewOnly = false)
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (SmartScript script in scripts)
            {
                ListViewItem lvi = new ListViewItem(script.entryorguid.ToString());
                lvi.Name = script.entryorguid.ToString();

                foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
                {
                    if (_excludedProperties.Contains(propInfo.Name))
                        continue;

                    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
                }

                if (!listViewOnly)
                    _smartScripts.Add(script);

                items.Add(lvi);
            }

            Items.AddRange(items.ToArray());
            HandleHighlightItems();
        }

        public void RemoveSmartScript(SmartScript script)
        {
            foreach (ListViewItem item in Items)
            {
                if (item.Text == script.entryorguid.ToString() && item.SubItems[2].Text == script.id.ToString())
                {
                    Items.Remove(item);
                    break;
                }
            }

            foreach (SmartScript smartScript in _smartScripts)
            {
                if (smartScript.entryorguid == script.entryorguid && smartScript.id == script.id)
                {
                    _smartScripts.Remove(smartScript);
                    break;
                }
            }
        }

        public void RemoveSmartScript(int entryorguid, int id)
        {
            foreach (ListViewItem item in Items)
            {
                if (item.Text == entryorguid.ToString() && item.SubItems[2].Text == id.ToString())
                {
                    Items.Remove(item);
                    break;
                }
            }

            _smartScripts.Remove(GetSmartScript(entryorguid, id));
        }

        public void ReplaceSmartScript(SmartScript script)
        {
            ListViewItem lvi = null;

            foreach (ListViewItem item in Items)
            {
                if (item.SubItems[0].Text == script.entryorguid.ToString() && item.SubItems[2].Text == script.id.ToString())
                {
                    lvi = item;
                    break;
                }
            }

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

            _smartScripts[_smartScripts.IndexOf(GetSmartScript(script.entryorguid, script.id))] = script;
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

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            HandleHighlightItems();
            base.OnItemSelectionChanged(e);
        }
    }
}
