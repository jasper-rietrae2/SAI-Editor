using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAI_Editor.Classes.CustomControls;
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
                if (state.ControlValue is CList)
                    state.Value = ((CList)state.ControlValue).Scripts;
                else
                    state.Value = state.ControlValue;

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
