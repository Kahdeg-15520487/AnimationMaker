using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PetGo.Monogame_Utilities.Drawing
{
    public static class TextRenderer
    {
        public static Texture2D RenderText(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, string text,SpriteFont font, Color textColor, Color bckgrdColor, float scale = 1f)
        {
            var textSize = font.MeasureString(text)*scale;
            RenderTarget2D result = new RenderTarget2D(graphicsDevice, (int)textSize.X, (int)textSize.Y);
            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(bckgrdColor);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, Vector2.Zero, textColor);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return result;
        }
    }
}