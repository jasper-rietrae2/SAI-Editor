using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAI_Editor.Classes.Database.Classes;

namespace SAI_Editor.Classes.Serialization
{
    [JsonObject]
    public class ScriptContainer
    {
        [JsonProperty]
        public string TypeName { get; set; }

        [JsonProperty]
        public object Value { get; set; }

    }
}
