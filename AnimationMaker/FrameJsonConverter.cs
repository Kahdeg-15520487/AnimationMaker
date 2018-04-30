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
    class RectangleJsonConverter : JsonConverter<Rectangle>
    {
        public override void WriteJson(JsonWriter writer, Rectangle value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            serializer.Serialize(writer, value.X);
            writer.WritePropertyName("y");
            serializer.Serialize(writer, value.Y);
            writer.WritePropertyName("w");
            serializer.Serialize(writer, value.Width);
            writer.WritePropertyName("h");
            serializer.Serialize(writer, value.Height);
            writer.WriteEndObject();
        }

        public override Rectangle ReadJson(JsonReader reader, Type objectType, Rectangle existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Rectangle result = new Rectangle();

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
                    case "x":
                        result.X = serializer.Deserialize<int>(reader);
                        break;
                    case "y":
                        result.Y = serializer.Deserialize<int>(reader);
                        break;
                    case "w":
                        result.Width = serializer.Deserialize<int>(reader);
                        break;
                    case "h":
                        result.Height = serializer.Deserialize<int>(reader);
                        break;
                }
            }

            return result;
        }
    }

    class FrameJsonConverter : JsonConverter<Frame>
    {
        public override void WriteJson(JsonWriter writer, Frame value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("sourcerect");
            serializer.Serialize(writer, value.GetRekt);
            writer.WriteEndObject();
        }

        public override Frame ReadJson(JsonReader reader, Type objectType, Frame existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Frame result = null;

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
                    case "sourcerect":
                        var sr = serializer.Deserialize<Rectangle>(reader);
                        result = new Frame(sr.X, sr.Y, sr.Width, sr.Height);
                        break;
                }
            }

            return result;
        }
    }
}
