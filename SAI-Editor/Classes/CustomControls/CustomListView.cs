using SAI_Editor.Classes.Database.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes.CustomControls
{
    public enum CustomListViewType
    {
        CustomListViewSmartScript,
        CustomListViewCondition,
    }

    public class CustomListView : ForceSelectListView
    {
        protected List<DatabaseClass> _scripts = new List<DatabaseClass>();
        protected List<string> _excludedProperties = new List<string>();
        protected CustomListViewType _customListViewType;
        protected readonly PropertyInfo[] _pinfo;

        protected CustomListView(CustomListViewType customListViewType)
        {
            switch (customListViewType)
            {
                case CustomListViewType.CustomListViewCondition:
                    _pinfo = typeof(Condition).GetProperties();
                    break;
                case CustomListViewType.CustomListViewSmartScript:
                    _pinfo = typeof(SmartScript).GetProperties();
                    break;
            }

            _scripts = new List<DatabaseClass>();
            _excludedProperties = new List<string>();
            _customListViewType = customListViewType;

            Init();
        }

        protected CustomListView(CustomListViewType customListViewType, List<DatabaseClass> scripts, List<string> exProps = null)
        {
            switch (customListViewType)
            {
                case CustomListViewType.CustomListViewCondition:
                    _pinfo = typeof(Condition).GetProperties();
                    break;
                case CustomListViewType.CustomListViewSmartScript:
                    _pinfo = typeof(SmartScript).GetProperties();
                    break;
            }

            _scripts = scripts;
            _excludedProperties = exProps ?? new List<string>();
            _customListViewType = customListViewType;

            Init();
        }

        protected string ListViewKey
        {
            get
            {
                switch (_customListViewType)
                {
                    case CustomListViewType.CustomListViewCondition:
                        return "SourceTypeOrReferenceId";
                    case CustomListViewType.CustomListViewSmartScript:
                        return "entryorguid";
                    default:
                        return "Unknown _customListViewType: " + _customListViewType.ToString();
                }
            }
        }

        protected string GetListViewKeyValue(DatabaseClass script)
        {
            return (_customListViewType == CustomListViewType.CustomListViewCondition ? (script as Condition).SourceTypeOrReferenceId : (script as SmartScript).entryorguid).ToString();
        }

        public virtual void Init(bool keepSelection = false)
        {
            int lastSelectedIndex = SelectedIndices.Count > 0 ? SelectedIndices[0] : -1;

            Items.Clear();
            Columns.Clear();

            foreach (PropertyInfo propInfo in _pinfo.Where(propInfo => !_excludedProperties.Contains(propInfo.Name)))
                Columns.Add(propInfo.Name);

            if (_scripts != null)
                AddScripts(_scripts, true);

            foreach (ColumnHeader header in Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (keepSelection && lastSelectedIndex != -1)
                Items[lastSelectedIndex].Selected = true;
        }

        public virtual int AddScript(DatabaseClass script, bool listViewOnly = false, bool selectNewItem = false)
        {
            CustomListViewItem lvi = new CustomListViewItem(GetListViewKeyValue(script));
            lvi.Script = script;
            lvi.Name = GetListViewKeyValue(script);

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals(ListViewKey)))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            }

            if (!listViewOnly)
                _scripts.Add(script);

            ListViewItem newItem = Items.Add(lvi);
            newItem.Selected = selectNewItem;
            Select();
            Focus();
            EnsureVisible(newItem.Index);
            return newItem.Index;
        }

        public virtual void AddScripts(List<DatabaseClass> scripts, bool listViewOnly = false)
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (DatabaseClass script in scripts)
            {
                CustomListViewItem lvi = new CustomListViewItem(GetListViewKeyValue(script));
                lvi.Script = script;
                lvi.Name = GetListViewKeyValue(script);

                foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals(ListViewKey)))
                {
                    if (_excludedProperties.Contains(propInfo.Name))
                        continue;

                    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
                }

                if (!listViewOnly)
                    _scripts.Add(script);

                items.Add(lvi);
            }

            Items.AddRange(items.ToArray());
        }

        public virtual void RemoveScript(DatabaseClass script)
        {
            foreach (CustomListViewItem item in Items.Cast<CustomListViewItem>().Where(item => item.Script == script))
            {
                Items.Remove(item);
                break;
            }

            _scripts.Remove(script);
        }

        public void ReplaceScript(DatabaseClass script)
        {
            CustomListViewItem lvi = Items.Cast<CustomListViewItem>().SingleOrDefault(p => p.Script == script);

            if (lvi == null)
                return;

            lvi.SubItems.Clear();
            lvi.Name = GetListViewKeyValue(script);
            lvi.Text = GetListViewKeyValue(script);

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals(ListViewKey)))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            }

            _scripts[_scripts.IndexOf(lvi.Script)] = script;
        }

        public void ReplaceData(List<DatabaseClass> scripts, List<string> exProps = null)
        {
            _scripts = scripts;
            _excludedProperties = exProps ?? new List<string>();
            Init();
        }

        public virtual void ReplaceScripts(List<DatabaseClass> scripts)
        {
            _scripts = scripts;
            Init();
        }

        public void ClearScripts()
        {
            _scripts = new List<DatabaseClass>();
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
    }
}
