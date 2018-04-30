using AnimationMaker.Screens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Utility;
using Utility.Drawing;
using Utility.ScreenManager;
using Utility.CustomJsonConverter;


namespace AnimationMaker
{
    public class GameManager : Game
    {
        GraphicsDeviceManager graphics;
        Camera camera;

        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 600
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            CONTENT_MANAGER.Content = Content;
            CONTENT_MANAGER.gameInstance = this;
            this.IsMouseVisible = true;

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new RectangleJsonConverter());
                settings.Converters.Add(new AnimationJsonConverter());
                settings.Converters.Add(new AnimatedEntityJsonConverter());
                return settings;
            };

            //var anim = JsonConvert.DeserializeObject<Utility.Drawing.Animation.AnimatedEntity>(System.IO.File.ReadAllText("anim.txt"));
            //System.IO.File.WriteAllText("anim.txt", JsonConvert.SerializeObject(anim, Formatting.Indented));
        }

        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport);
            CONTENT_MANAGER.camera = camera;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            DrawingHelper.Initialize(GraphicsDevice);
            CONTENT_MANAGER.spriteBatch = new SpriteBatch(GraphicsDevice);
            CONTENT_MANAGER.CurrentInputState = new InputState(Mouse.GetState(), Keyboard.GetState());

            CONTENT_MANAGER.LoadFonts("default");
            CONTENT_MANAGER.LoadSprites("Cats", "tank", "bullet");

            InitScreens();
        }

        private void InitScreens()
        {
            SCREEN_MANAGER.AddScreen(new MainScreen(GraphicsDevice));
            SCREEN_MANAGER.AddScreen(new FileBrowserScreen(GraphicsDevice));

            SCREEN_MANAGER.GotoScreen("MainScreen");
            //SCREEN_MANAGER.GotoScreen("FileBrowserScreen");

            SCREEN_MANAGER.Init();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CONTENT_MANAGER.CurrentInputState = new InputState(Mouse.GetState(), Keyboard.GetState());

            SCREEN_MANAGER.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CONTENT_MANAGER.spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: camera.TransformMatrix);
            {
                SCREEN_MANAGER.Draw(CONTENT_MANAGER.spriteBatch, gameTime);
                //DrawingHelper.Draw(CONTENT_MANAGER.spriteBatch);
                CONTENT_MANAGER.ShowFPS(gameTime);
            }
            CONTENT_MANAGER.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
