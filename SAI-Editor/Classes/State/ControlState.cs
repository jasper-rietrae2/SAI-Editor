using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes.State
{
    public class ControlState
    {
        public Type ControlType { get; protected set; }

        public Action<IDictionary<Control, object>, Control> Serializer { get; protected set; }

        public Action<Control, object> Deserializer { get; protected set; }
    }

    public class ControlState<T> : ControlState
    {

        public ControlState(Action<IDictionary<Control, object>, Control> serializer, Action<Control, object> deserializer)
        {
            ControlType = typeof(T);
            Serializer = serializer;
            Deserializer = deserializer;
        }

    }
}
