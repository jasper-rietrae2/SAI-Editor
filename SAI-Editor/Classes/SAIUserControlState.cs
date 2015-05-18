using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SAI_Editor.Classes.CustomControls;
using SAI_Editor.Classes.State;

namespace SAI_Editor.Classes
{
    public class SAIUserControlState : ICloneable
    {
        public static readonly Dictionary<Type, ControlState> StatesByType;

        public readonly Dictionary<Control, object> Controls;

        static SAIUserControlState()
        {
            List<ControlState> states = new List<ControlState>();

            states.Add(new ControlState<TextBox>((d, c) =>
            {
                d.Add(c, c.Text);
            }, (c, o) =>
            {
                string str = o as string;

                if (c.Text != str)
                    c.Text = str;
            }));

            states.Add(new ControlState<ComboBox>((d, c) =>
            {
                d.Add(c, ((ComboBox)c).SelectedIndex);
            }, (c, o) =>
            {
                ComboBox cb = (ComboBox)c;
                int selected = (int)o;

                cb.SelectedIndex = selected;
            }));

            states.Add(new ControlState<NumericUpDown>((d, c) =>
            {
                d.Add(c, ((NumericUpDown)c).Value);
            }, (c, o) =>
            {
                NumericUpDown nup = (NumericUpDown)c;
                decimal value = (decimal)o;

                nup.Value = value;
            }));

            //states.Add(new ControlState<NumericUpDown>((d, c) => d.Add(c, ((NumericUpDown)c).Value), (c, o) => ((NumericUpDown)c).Value = (decimal)o));
            states.Add(new ControlState<CheckBox>((d, c) => d.Add(c, ((CheckBox)c).Checked), (c, o) => ((CheckBox)c).Checked = (bool)o));
            states.Add(new ControlState<CustomObjectListView>((d, c) =>
            {
                d.Add(c, ((CustomObjectListView)c).List);
            }, (c, o) =>
            {
                CustomObjectListView listView = (CustomObjectListView)c;
                CList list = (CList)o;

                listView.List = (CList)list.Clone();
                list.Apply();
            }));

            StatesByType = states.ToDictionary(p => p.ControlType);
        }

        public SAIUserControlState(IDictionary<Control, object> controls)
        {
            Controls = controls.ToDictionary(p => p.Key, p => p.Value);
        }

        public SAIUserControlState()
        {
            Controls = new Dictionary<Control, object>();
        }

        public virtual void Load()
        {
            foreach (var ctrl in Controls)
            {
                Type ctrlType = ctrl.Key.GetType();
                StatesByType[ctrlType].Deserializer(ctrl.Key, ctrl.Value);
            }
        }

        public virtual void Save(Control control)
        {
            Controls.Clear();

            foreach (Control ctrl in GetChildControls(control))
            {
                Type type = ctrl.GetType();

                if (StatesByType.ContainsKey(type))
                    StatesByType[type].Serializer(Controls, ctrl);
            }
        }

        public virtual void ClearControls(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = String.Empty;
                else if (ctrl is NumericUpDown)
                    ((NumericUpDown)ctrl).Value = 0;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                else if (ctrl is ListView)
                    ((ListView)ctrl).Items.Clear();
            }
        }

        public object Clone()
        {
            return new SAIUserControlState(Controls);
        }

        private IEnumerable<Control> GetChildControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren)
                    foreach (Control c2 in GetChildControls(c))
                        yield return c2;

                yield return c;
            }
        }

        public void SetControlValueByName(string name, object value)
        {
            foreach (KeyValuePair<Control, object> control in Controls)
            {
                if (control.Key.Name == name)
                {
                    control.Key.Text = value.ToString();
                    //Controls[control.Key] = value;
                    break;
                }
            }
        }

        public Control GetControlByName(string name)
        {
            foreach (KeyValuePair<Control, object> control in Controls)
                if (control.Key.Name == name)
                    return control.Key;

            return null;
        }

        public object GetControlValueName(string name)
        {
            foreach (KeyValuePair<Control, object> control in Controls)
                if (control.Key.Name == name)
                    return control.Value;

            return null;
        }
    }
}
