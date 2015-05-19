using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SAI_Editor.Classes.Serialization
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class StateObject
    {
        public object ControlValue { get; set; }

        [JsonProperty]
        public string Key { get; set; }

        [JsonProperty]
        public object Value { get; set; }

    }
}
