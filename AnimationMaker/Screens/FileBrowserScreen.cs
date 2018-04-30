using System;
using System.IO;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Utility;
using Utility.Drawing;
using Utility.ScreenManager;
using Utility.UI;

namespace AnimationMaker.Screens
{
    class FileBrowserScreen : Screen
    {
        Canvas canvas;
        int count = 0;

        public FileBrowserScreen(GraphicsDevice device) : base(device, typeof(FileBrowserScreen).Name)
        { }

        public override bool Init()
        {
            InitCanvas();
            return base.Init();
        }

        public void InitCanvas()
        {
            canvas = new Canvas();

            ListView listView = new ListView(new Point(20, 20), new Vector2(300, 300), new Point(10, 40),new Vector2(50,30));
            Button button = new Button("Add Item", new Point(400, 20), new Vector2(80, 30), CONTENT_MANAGER.Fonts["default"]);
            Label label = new Label("", new Point(400, 60), new Vector2(50, 30), CONTENT_MANAGER.Fonts["default"])
            {
                Origin = new Vector2(-2, -2)
            };

            button.MouseClick += (o, e) =>
            {
                listView.AddItem(count++.ToString());
            };

            listView.ItemSelected += (o, e) =>
            {
                label.Text = e.Text;
                CONTENT_MANAGER.Log(e.Text);
            };

            canvas.AddElement("listView", listView);
            canvas.AddElement("button", button);
            canvas.AddElement("label", label);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            canvas.Update(gameTime, CONTENT_MANAGER.CurrentInputState, CONTENT_MANAGER.LastInputState);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            canvas.Draw(spriteBatch, gameTime);
            base.Draw(spriteBatch, gameTime);
        }
    }
}
