using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Utility.Drawing.Animation;

namespace AnimationMaker
{
    class AnimatedEntityJsonConverter : JsonConverter<AnimatedEntity>
    {
        public override void WriteJson(JsonWriter writer, AnimatedEntity value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("animations");
            writer.WriteStartArray();
            foreach (var animation in value.Animations)
            {
                serializer.Serialize(writer, animation.Value);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override AnimatedEntity ReadJson(JsonReader reader, Type objectType, AnimatedEntity existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            AnimatedEntity result = new AnimatedEntity();

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
                    case "animations":
                        var temp = serializer.Deserialize(reader);
                        var anims = JsonConvert.DeserializeObject<List<Animation>>(temp.ToString());
                        result.AddAnimation(anims);
                        break;
                }
            }

            return result;
        }
    }
}
