using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Drawing.Animation;

namespace AnimationMaker
{
    class AnimationJsonConverter : JsonConverter<Animation>
    {
        public override void WriteJson(JsonWriter writer, Animation value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            serializer.Serialize(writer, value.Name);
            writer.WritePropertyName("loop");
            serializer.Serialize(writer, value.ShouldLoop);
            writer.WritePropertyName("fps");
            serializer.Serialize(writer, value.FramesPerSecond);
            writer.WritePropertyName("transkey");
            serializer.Serialize(writer, value.TransitionKey);
            writer.WritePropertyName("frames");
            writer.WriteStartArray();
            foreach (var frame in value.KeyFrames)
            {
                serializer.Serialize(writer, frame.GetRekt);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override Animation ReadJson(JsonReader reader, Type objectType, Animation existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Animation result = null;
            string name = null;
            bool loop = false;
            float fps = 1;
            string transkey = null;
            List<Rectangle> frames = new List<Rectangle>();

            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    break;
                }

                string propertyName = (string)reader.Value;
                if (!reader.Read())
                {
                    continue;
                }

                switch (propertyName)
                {
                    case "name":
                        name = serializer.Deserialize<string>(reader);
                        break;
                    case "loop":
                        loop = serializer.Deserialize<bool>(reader);
                        break;
                    case "fps":
                        fps = serializer.Deserialize<float>(reader);
                        break;
                    case "transkey":
                        transkey = serializer.Deserialize<string>(reader);
                        break;
                    case "frames":
                        var temp = serializer.Deserialize(reader);
                        frames = JsonConvert.DeserializeObject<List<Rectangle>>(temp.ToString());
                        break;
                }
            }

            result = new Animation(name, loop, fps, transkey);
            foreach (var frame in frames)
            {
                result.AddKeyFrame(frame);
            }

            return result;
        }
    }
}
