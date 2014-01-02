using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes
{
    public class SearchableComboBox : ComboBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        public ObjectCollection OriginalItems { get; set; }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (OriginalItems == null)
                return;

            Items.Clear();

            if (Text == String.Empty)
            {
                foreach (string item in OriginalItems)
                    Items.Add(item);

                return;
            }

            foreach (string item in OriginalItems)
                if (item.ToLower().Contains(item.ToLower()))
                    Items.Add(item);
        }
    }
}
