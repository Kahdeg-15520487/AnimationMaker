using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PetGo.Monogame_Utilities.Drawing
{
    public static class TextureRenderer
    {
        public static Texture2D Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Vector2 size, float scale = 1f,params Tuple<Vector2,Texture2D>[] textureList)
        {
            RenderTarget2D result = new RenderTarget2D(graphicsDevice, (int)size.X, (int)size.Y);
            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach (var sprite in textureList)
            {
                spriteBatch.Draw(sprite.Item2, sprite.Item1, Color.White);
            }

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return result;
        }
    }
}