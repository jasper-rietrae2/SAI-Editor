using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Database.Classes;

namespace System.Windows.Forms
{
    public class SmartScriptListView : XListView
    {
        private List<SmartScript> _smartScripts;
        private List<string> _excludedProperties;
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

        private void Init()
        {
            Items.Clear();
            Columns.Clear();

            foreach (PropertyInfo propInfo in _pinfo)
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                Columns.Add(propInfo.Name);
            }

            foreach (SmartScript script in _smartScripts)
                AddSmartScript(script);

            foreach (ColumnHeader header in Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void AddSmartScript(SmartScript script)
        {
            ListViewItem lvi = new ListViewItem(script.entryorguid.ToString());
            lvi.Name = script.entryorguid.ToString();

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("entryorguid")))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            }

            if (_smartScripts.All(p => p.entryorguid != script.entryorguid))
                _smartScripts.Add(script);

            Items.Add(lvi);
        }

        public void ReplaceSmartScript(SmartScript script)
        {
            ListViewItem lvi = Items[script.entryorguid.ToString()];

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

            _smartScripts[_smartScripts.IndexOf(_smartScripts.Single(p => p.entryorguid == script.entryorguid))] = script;
        }

        public void RemoveSmartScript(int scriptId)
        {
            Items.RemoveByKey(scriptId.ToString());
            _smartScripts.Remove(_smartScripts.Single(p => p.entryorguid == scriptId));
        }

        public void ReplaceData(List<SmartScript> scripts, List<string> exProps = null)
        {
            _smartScripts = scripts;
            _excludedProperties = exProps ?? new List<string>();
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
            foreach (string propName in propNames)
                _excludedProperties.Add(propName);

            Init();
        }
    }
}
