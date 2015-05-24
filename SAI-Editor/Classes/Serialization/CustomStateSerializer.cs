using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAI_Editor.Classes.CustomControls;
using SAI_Editor.Classes.Database.Classes;
using SAI_Editor.Classes.State;

namespace SAI_Editor.Classes.Serialization
{
    public class CustomStateSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var states = (List<StateObject>)value;

            writer.WriteStartArray();

            foreach (var state in states)
            {
                if (state.ControlValue is List<DatabaseClass>)
                {
                    var scripts = (List<DatabaseClass>)state.ControlValue;

                    state.IsList = true;
                    var sList = new List<ScriptContainer>();

                    foreach (DatabaseClass s in scripts)
                    {
                        sList.Add(new ScriptContainer { TypeName = s.GetType().FullName, Value = s });
                    }

                    state.Value = sList;
                }
                else
                {
                    state.Value = state.ControlValue;
                }

                JObject.FromObject(state).WriteTo(writer);
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<StateObject>);
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
    }
}
