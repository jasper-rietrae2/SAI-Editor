using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Newtonsoft.Json;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class CList : ICloneable
    {
        public List<DatabaseClass> Scripts { get; protected set; }
        protected List<string> ExcludedProperties = new List<string>();

        protected readonly PropertyInfo[] PropInfo;

        protected abstract string GetListViewKey();
        protected abstract string GetListViewKeyValue(DatabaseClass script);

        public FastObjectListView ListView { get; set; }

        protected CList(FastObjectListView listView, Type type)
        {
            ListView = listView;
            PropInfo = type.GetProperties();
            Scripts = new List<DatabaseClass>();
        }

        public virtual void Apply(bool keepSelection = false)
        {
            int lastSelectedIndex = ListView.SelectedIndex;
            object lastSelected = ListView.SelectedObjects.Count > 0 ? ListView.SelectedObject : null;

            //ListView.Items.Clear();

            ListView.ClearObjects();
            ListView.Columns.Clear();

            foreach (PropertyInfo propInfo in PropInfo.Where(propInfo => !ExcludedProperties.Contains(propInfo.Name)))
            {
                ListView.Columns.Add(new OLVColumn(propInfo.Name, propInfo.Name));
            }

            if (Scripts != null)
                AddScripts(Scripts, true);

            ResizeColumns();

            if (keepSelection && lastSelected != null)
                ListView.SelectObject(lastSelected);

            if (keepSelection && lastSelectedIndex != -1)
                ListView.Items[lastSelectedIndex].Selected = true;
        }

        public void ResizeColumns()
        {
            foreach (ColumnHeader header in ListView.Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public virtual void AddScript(DatabaseClass script, bool listViewOnly = false, bool selectNewItem = false)
        {
            ListView.AddObject(script);

            if (!listViewOnly)
                Scripts.Add(script);

            //CustomListViewItem lvi = new CustomListViewItem(GetListViewKeyValue(script));
            //lvi.Script = script;
            //lvi.Name = GetListViewKeyValue(script);

            //foreach (PropertyInfo propInfo in PropInfo.Where(p => !p.Name.Equals(GetListViewKey())))
            //{
            //    if (ExcludedProperties.Contains(propInfo.Name))
            //        continue;

            //    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            //}

            //if (!listViewOnly)
            //    Scripts.Add(script);

            //ListViewItem newItem = ListView.Items.Add(lvi);

            if (selectNewItem)
            {
                ListView.SelectObject(script);
                //newItem.Selected = true;
                //newItem.Focused = true;
                ListView.Select();
                ListView.EnsureModelVisible(script);
            }
        }

        public virtual void AddScripts(List<DatabaseClass> scripts, bool listViewOnly = false)
        {
            ListView.AddObjects(scripts);

            if (!listViewOnly)
                Scripts.AddRange(scripts);

            //List<ListViewItem> items = new List<ListViewItem>();

            //foreach (DatabaseClass script in scripts)
            //{
            //    CustomListViewItem lvi = new CustomListViewItem(GetListViewKeyValue(script));
            //    lvi.Script = script;
            //    lvi.Name = GetListViewKeyValue(script);

            //    foreach (PropertyInfo propInfo in PropInfo.Where(p => !p.Name.Equals(GetListViewKey())))
            //    {
            //        if (ExcludedProperties.Contains(propInfo.Name))
            //            continue;

            //        lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            //    }

            //    if (!listViewOnly)
            //        Scripts.Add(script);

            //    ListView.Items.Add(lvi);
            //    //items.Add(lvi);
            //}

            //ListView.Items.AddRange(items.ToArray());
        }

        public void ReplaceScript(DatabaseClass script)
        {
            //CustomListViewItem lvi = ListView.Items.Cast<CustomListViewItem>().SingleOrDefault(p => p.Script == script);

            //if (lvi == null)
            //    return;

            //lvi.SubItems.Clear();
            //lvi.Name = GetListViewKeyValue(script);
            //lvi.Text = GetListViewKeyValue(script);

            //foreach (PropertyInfo propInfo in PropInfo.Where(p => !p.Name.Equals(GetListViewKey())))
            //{
            //    if (ExcludedProperties.Contains(propInfo.Name))
            //        continue;

            //    lvi.SubItems.Add(propInfo.GetValue(script).ToString());
            //}

            ListView.UpdateObject(script);
            //Scripts[Scripts.IndexOf(lvi.Script)] = script;
        }

        public virtual void RemoveScript(DatabaseClass script)
        {
            ListView.RemoveObject(script);
            Scripts.Remove(script);
        }

        public void ReplaceData(List<DatabaseClass> scripts, List<string> exProps = null)
        {
            Scripts = scripts;
            ExcludedProperties = exProps ?? new List<string>();
            Apply();
        }

        public virtual void ReplaceScripts(List<DatabaseClass> scripts)
        {
            Scripts = scripts;
            Apply();
        }

        public void ClearScripts()
        {
            Scripts.Clear();
            Apply();
        }

        public void IncludeProperty(string propName)
        {
            ExcludedProperties.Remove(propName);
            Apply();
        }

        public void ExcludeProperty(string propName)
        {
            ExcludedProperties.Add(propName);
            Apply();
        }

        public void IncludeProperties(List<string> propNames)
        {
            foreach (string propName in propNames)
                ExcludedProperties.Remove(propName);

            Apply();
        }

        public void ExcludeProperties(List<string> propNames)
        {
            ExcludedProperties = new List<string>(propNames);
            Apply();
        }

        public abstract object Clone();
    }
}
