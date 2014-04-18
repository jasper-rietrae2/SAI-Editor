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
    public class ConditionListView : ForceSelectListView
    {
        private List<Condition> _conditions = new List<Condition>();
        private List<string> _excludedProperties = new List<string>();
        private readonly PropertyInfo[] _pinfo;

        public bool EnablePhaseHighlighting { get; set; }

        public List<Condition> Conditions
        {
            get { return _conditions; }
        }

        public Condition SelectedCondition
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return _conditions.FirstOrDefault(condition => condition == ((CustomListViewItem)SelectedItems[0]).Script);

                return null;
            }
        }

        public ConditionListView()
        {
            EnablePhaseHighlighting = false;
            _pinfo = typeof(Condition).GetProperties();
            _conditions = new List<Condition>();
            _excludedProperties = new List<string>();

            Init();
        }

        public ConditionListView(List<Condition> conditions, List<string> exProps = null)
        {
            EnablePhaseHighlighting = false;
            _pinfo = typeof(Condition).GetProperties();
            _conditions = conditions;
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

            if (_conditions != null)
                AddConditions(_conditions, true);

            foreach (ColumnHeader header in Columns)
                header.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (keepSelection && lastSelectedIndex != -1)
                Items[lastSelectedIndex].Selected = true;
        }

        public int AddCondition(Condition condition, bool listViewOnly = false, bool selectNewItem = false)
        {
            CustomListViewItem lvi = new CustomListViewItem(condition.SourceTypeOrReferenceId.ToString());
            lvi.Script = condition;
            lvi.Name = condition.SourceTypeOrReferenceId.ToString();

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("SourceTypeOrReferenceId")))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(condition).ToString());
            }

            if (!listViewOnly)
                _conditions.Add(condition);

            ListViewItem newItem = Items.Add(lvi);
            newItem.Selected = selectNewItem;
            return newItem.Index;
        }

        public void AddConditions(List<Condition> conditions, bool listViewOnly = false)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (Condition condition in conditions)
            {
                CustomListViewItem lvi = new CustomListViewItem(condition.SourceTypeOrReferenceId.ToString());
                lvi.Script = condition;
                lvi.Name = condition.SourceTypeOrReferenceId.ToString();

                foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("SourceTypeOrReferenceId")))
                {
                    if (_excludedProperties.Contains(propInfo.Name))
                        continue;

                    lvi.SubItems.Add(propInfo.GetValue(condition).ToString());
                }

                if (!listViewOnly)
                    _conditions.Add(condition);

                items.Add(lvi);
            }

            Items.AddRange(items.ToArray());

        }

        public void RemoveCondition(Condition condition)
        {
            foreach (CustomListViewItem item in Items.Cast<CustomListViewItem>().Where(item => item.Script == condition))
            {
                Items.Remove(item);
                break;
            }

            _conditions.Remove(condition);
        }

        public void ReplaceCondition(Condition condition)
        {
            CustomListViewItem lvi = Items.Cast<CustomListViewItem>().SingleOrDefault(p => p.Script == condition);

            if (lvi == null)
                return;

            lvi.SubItems.Clear();
            lvi.Name = condition.SourceTypeOrReferenceId.ToString();
            lvi.Text = condition.SourceTypeOrReferenceId.ToString();

            foreach (PropertyInfo propInfo in _pinfo.Where(p => !p.Name.Equals("SourceTypeOrReferenceId")))
            {
                if (_excludedProperties.Contains(propInfo.Name))
                    continue;

                lvi.SubItems.Add(propInfo.GetValue(condition).ToString());
            }

            _conditions[_conditions.IndexOf(lvi.Script as Condition)] = condition;
        }

        public void ReplaceData(List<Condition> conditions, List<string> exProps = null)
        {
            _conditions = conditions;
            _excludedProperties = exProps ?? new List<string>();
            Init();
        }

        public void ReplaceConditions(List<Condition> conditions)
        {
            _conditions = conditions;
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
