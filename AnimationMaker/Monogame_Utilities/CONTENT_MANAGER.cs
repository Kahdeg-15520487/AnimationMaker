using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Utilities.Drawing.Animation;
using Utilities.Drawing;

namespace Utilities
{
    //singleton to store common data
    public static partial class CONTENT_MANAGER
    {
        public static Game gameInstance;
        public static Camera camera;

        #region resources
        public static ContentManager Content;

        public static SpriteBatch spriteBatch;

        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Dictionary<string, Texture2D> spriteSheets = new Dictionary<string, Texture2D>();

        public static Dictionary<string, AnimatedEntity> animationEntities;
        public static Dictionary<string, Texture2D> animationSheets;
        public static List<Animation> animationTypes;

        public static void LoadFont(params string[] fontList)
        {
            foreach (var font in fontList)
            {
                fonts.Add(font, Content.Load<SpriteFont>(string.Format(@"font\{0}", font)));
            }
        }

        public static void LoadSpriteSheet(params string[] spriteSheetList)
        {
            foreach (var spritesheet in spriteSheetList)
            {
                spriteSheets.Add(spritesheet, Content.Load<Texture2D>(string.Format(@"sprite\{0}",spritesheet)));
            }

        }

        public static void LoadAnimationContent(params string[] animationList)
        {
            //string delimit = "Yellow";
            animationEntities = new Dictionary<string, AnimatedEntity>();
            animationSheets = new Dictionary<string, Texture2D>();
            animationTypes = new List<Animation>();
        }


        public static void LoadSound(params string[] soundlist)
        {
            //menu_select = Content.Load<SoundEffect>(@"sound\sfx\menu_select");
        }

        #endregion

        private static InputState _inputState;
        public static InputState CurrentInputState
        {
            get
            {
                return _inputState;
            }
            set
            {
                LastInputState = _inputState;
                _inputState = value;
            }
        }
        public static InputState LastInputState { get; private set; }

        public static void ShowFPS(GameTime gameTime)
        {
            int frameRate = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.DrawString(fonts["defaultFont"], frameRate.ToString(), new Vector2(0, 0), Color.Black);
        }
    }
}
