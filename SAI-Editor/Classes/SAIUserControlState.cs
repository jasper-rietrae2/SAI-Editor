using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAI_Editor.Classes.CustomControls;
using SAI_Editor.Classes.Serialization;
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
                d[c] = c.Text;
            }, (c, o) =>
            {
                string str = o.ToString();

                if (c.Text != str)
                    c.Text = str;
            }));

            states.Add(new ControlState<ComboBox>((d, c) =>
            {
                d[c] = ((ComboBox)c).SelectedIndex;
            }, (c, o) =>
            {
                ComboBox cb = (ComboBox)c;
                int selected = (int)Convert.ChangeType(o, typeof(int));

                cb.SelectedIndex = selected;
            }));

            states.Add(new ControlState<NumericUpDown>((d, c) =>
            {
                d[c] = ((NumericUpDown)c).Value;
            }, (c, o) =>
            {
                NumericUpDown nup = (NumericUpDown)c;
                decimal value = (decimal)Convert.ChangeType(o, typeof(decimal));

                nup.Value = value;
            }));

            //states.Add(new ControlState<NumericUpDown>((d, c) => d.Add(c, ((NumericUpDown)c).Value), (c, o) => ((NumericUpDown)c).Value = (decimal)o));
            states.Add(new ControlState<CheckBox>((d, c) => d[c] = ((CheckBox)c).Checked, (c, o) => ((CheckBox)c).Checked = (bool)Convert.ChangeType(o, typeof(bool))));
            states.Add(new ControlState<CustomObjectListView>((d, c) =>
            {
                d[c] = ((CustomObjectListView)c).List;
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

        public static Dictionary<int, SAIUserControlState> StatesFromJson(string json, Control control)
        {
            Dictionary<int, SAIUserControlState> states = null;

            try
            {
                var objs = JsonConvert.DeserializeAnonymousType(json, new
                {
                    Workspaces = new[]
                {
                    new
                    {
                        Workspace = 0,
                        Value = new List<StateObject>()
                    }
                }
                });

                var controls = GetChildControls(control).ToList();

                var grouped = (
                               from c in controls
                               from w in objs.Workspaces
                               from s in w.Value
                               where s.Key == c.Name
                               let z = new
                               {
                                   Workspace = w,
                                   Control = c,
                                   State = s
                               }
                               group z by z.Workspace.Workspace into g
                               select g);

                states = new Dictionary<int, SAIUserControlState>();

                foreach (var group in grouped)
                {
                    SAIUserControlState state = new SAIUserControlState();

                    foreach (var g in group)
                    {
                        if (g.Control is CustomObjectListView)
                        {
                            continue;
                        }

                        state.Controls.Add(g.Control, g.State.Value);
                    }

                    //state.Save(controls);
                    states.Add(group.Key, state);
                }
            }
            catch (JsonReaderException ex)
            {

            }

            return states;
        }

        public virtual void Save(Control control)
        {
            Save(GetChildControls(control));
        }

        public virtual void Save(IEnumerable<Control> controls)
        {
            foreach (Control ctrl in controls)
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

        private static IEnumerable<Control> GetChildControls(Control parent)
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
                    //control.Key.Text = value.ToString();
                    Controls[control.Key] = value;
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

        public List<StateObject> ToStateObjects()
        {
            var objs = new List<StateObject>();

            foreach (var kvp in Controls)
            {
                if (kvp.Key is CustomObjectListView)
                {
                    continue;
                }

                objs.Add(new StateObject { Key = kvp.Key.Name, ControlValue = kvp.Value });
            }

            return objs;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(ToStateObjects(), Formatting.Indented, new CustomStateSerializer());
        }
    }
}
