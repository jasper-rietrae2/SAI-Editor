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
    public class ConditionListView : CustomListView
    {
        public ConditionListView()
            : base(CustomListViewType.CustomListViewCondition)
        {

        }

        public ConditionListView(List<DatabaseClass> scripts, List<string> exProps = null)
            : base(CustomListViewType.CustomListViewCondition, scripts, exProps)
        {

        }

        public Condition SelectedScript
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return (_scripts.FirstOrDefault(script => script == ((CustomListViewItem)SelectedItems[0]).Script) as Condition);

                return null;
            }
        }

        public List<Condition> Scripts
        {
            get { return _scripts.Cast<Condition>().ToList(); }
        }
    }
}
