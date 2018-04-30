using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json;
using System.IO;
using Utility.ScreenManager;
using Utility.UI;
using Utility.Drawing;
using Utility;
using Utility.Drawing.Animation;

namespace AnimationMaker.Screens
{
    class MainScreen : Screen
    {
        Canvas canvas;

        PictureBox pictureBox_spritesheet;
        PictureBox pictureBox_snippet;
        Label label_snippet;

        AnimationPlayer animationPlayer;

        Dictionary<string, List<Rectangle>> animation_list;

        Rectangle selectedFrame;
        bool scaleChanged = false;

        string texture = "bullet";

        public MainScreen(GraphicsDevice device) : base(device, typeof(MainScreen).Name)
        { }

        public override bool Init()
        {
            animation_list = new Dictionary<string, List<Rectangle>>();
            InitCanvas();
            return base.Init();
        }

        private void InitCanvas()
        {
            canvas = new Canvas();

            Label label_name = new Label("Untitled", new Point(400, 0), new Vector2(200, 20), CONTENT_MANAGER.Fonts["default"])
            {
                Origin = new Vector2(-2, -2),
                BackgroundColor = Color.LightGray
            };

            label_snippet = new Label("", new Point(400, 30), new Vector2(20, 20), CONTENT_MANAGER.Fonts["default"])
            {
                Origin = new Vector2(-2, -2)
            };

            CONTENT_MANAGER.gameInstance.Window.TextInput += (obj, e) =>
            {
                //if (label_name.IsFocused)
                {
                    if (char.IsLetterOrDigit(e.Character))
                    {
                        label_name.Text += e.Character;
                    }
                    else if (e.Key == Keys.Back)
                    {
                        int cc = label_name.Text.Length - 1;
                        if (cc >= 0)
                            label_name.Text = label_name.Text.Remove(cc);
                    }
                }
            };

            pictureBox_spritesheet = new PictureBox(CONTENT_MANAGER.Sprites[texture], new Point(50, 0), null, null);

            pictureBox_snippet = new PictureBox(CONTENT_MANAGER.Sprites[texture], new Point(400, 70), Rectangle.Empty, null);

            pictureBox_spritesheet.MouseDrag += (obj, e) =>
            {
                if (e.Size != Point.Zero)
                {
                    //DrawingHelper.RemoveShape(selectedFrame);
                    //selectedFrame = DrawingHelper.GetRectangle(e, Color.Red, false);
                    //DrawingHelper.DrawShape(selectedFrame);
                    selectedFrame = new Rectangle((e.Location.ToVector2() * pictureBox_spritesheet.Scale).ToPoint(), (e.Size.ToVector2() * pictureBox_spritesheet.Scale).ToPoint());
                    label_snippet.Text = selectedFrame.ToString();
                    e.Location = new Point(e.Location.X - pictureBox_spritesheet.Position.X, e.Location.Y - pictureBox_spritesheet.Position.Y);
                    pictureBox_snippet.SourceRectangle = e;
                }
            };

            pictureBox_spritesheet.KeyPress += (obj, e) =>
            {
                if (HelperFunction.IsKeyPress(Keys.OemPlus))
                {
                    pictureBox_spritesheet.Scale += 0.1f;
                    scaleChanged = true;
                }

                if (HelperFunction.IsKeyPress(Keys.OemMinus))
                {
                    if (pictureBox_spritesheet.Scale > 1)
                    {
                        pictureBox_spritesheet.Scale -= 0.1f;
                        scaleChanged = true;
                    }
                }
            };
            Button button_new = new Button("New", new Point(650, 30), new Vector2(60, 30), CONTENT_MANAGER.Fonts["default"])
            {
                ButtonColorReleased = Color.White
            };
            button_new.MouseClick += (o, e) =>
            {
                animationPlayer.StopAnimation();
                label_name.Text = "Untitled";
            };

            Button button_add_sprite = new Button("Add F", new Point(720, 30), new Vector2(60, 30), CONTENT_MANAGER.Fonts["default"])
            {
                ButtonColorReleased = Color.White
            };
            button_add_sprite.MouseClick += (obj, e) =>
            {
                if (string.IsNullOrWhiteSpace(label_name.Text))
                {
                    label_name.Text = "Please enter the name of this animation";
                }
                else if (!animation_list.ContainsKey(label_name.Text))
                {
                    animation_list.Add(label_name.Text, new List<Rectangle>() { pictureBox_snippet.SourceRectangle });
                }
                else
                {
                    animation_list[label_name.Text].Add(pictureBox_snippet.SourceRectangle);
                }
            };

            animationPlayer = new AnimationPlayer(new AnimatedEntity(), new Point(700, 70));

            Button button_test_animation = new Button("Test", new Point(790, 30), new Vector2(60, 30), CONTENT_MANAGER.Fonts["default"])
            {
                ButtonColorReleased = Color.White
            };
            button_test_animation.MouseClick += (o, e) =>
            {
                string animationName = label_name.Text;
                Animation animation = new Animation(animationName, true, 1, null);
                foreach (var frame in animation_list[animationName])
                {
                    animation.AddKeyFrame(frame);
                }
                if (animationPlayer.ContainAnimation(animation.Name))
                {
                    animationPlayer.RemoveAnimation(animation.Name);
                }
                animationPlayer.AddAnimation(animation);
                animationPlayer.LoadContent(pictureBox_snippet.Texture2D);
                animationPlayer.PlayAnimation(animation.Name);
            };

            Button button_export = new Button("Export", new Point(860, 30), new Vector2(60, 30), CONTENT_MANAGER.Fonts["default"])
            {
                ButtonColorReleased = Color.White
            };
            button_export.MouseClick += (o, e) =>
            {
                var animdata = JsonConvert.SerializeObject(animationPlayer.AnimatedEntity, Formatting.Indented);
                File.WriteAllText($"{label_name.Text}.txt", animdata);
            };

            canvas.AddElement("label_test", label_name);
            canvas.AddElement("label_snippet", label_snippet);
            canvas.AddElement("pictureBox_test", pictureBox_spritesheet);
            canvas.AddElement("pictureBox_snippet", pictureBox_snippet);
            canvas.AddElement("button_add_sprite", button_add_sprite);
            canvas.AddElement("animationPlayer", animationPlayer);
            canvas.AddElement("button_test_animation", button_test_animation);
            canvas.AddElement("button_export", button_export);
            canvas.AddElement("button_new", button_new);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            canvas.Update(gameTime, CONTENT_MANAGER.CurrentInputState, CONTENT_MANAGER.LastInputState);

            MoveSnippet();

            base.Update(gameTime);
        }

        private void MoveSnippet()
        {
            //with shift : move the whole snippet rectangle
            //without shift : adjust the size of the snippet rectangle
            bool checkShift = HelperFunction.IsKeyDown(Keys.LeftShift) || HelperFunction.IsKeyDown(Keys.RightShift);
            //with ctrl : press to move
            //without ctrl : hold down to move
            bool checkCtrl = HelperFunction.IsKeyDown(Keys.LeftControl) || HelperFunction.IsKeyDown(Keys.RightControl);

            Rectangle rekt = pictureBox_snippet.SourceRectangle;
            Point position = rekt.Location;
            Point size = rekt.Size;
            if ((HelperFunction.IsKeyDown(Keys.Left) && !checkCtrl) || (HelperFunction.IsKeyPress(Keys.Left) && checkCtrl))
            {
                if (checkShift)
                {
                    //move size
                    size = new Point(MathHelper.Clamp(size.X - 1, 0, size.X), size.Y);
                }
                else
                {
                    position = new Point(MathHelper.Clamp(position.X - 1, 0, position.X), position.Y);
                }
            }
            if ((HelperFunction.IsKeyDown(Keys.Right) && !checkCtrl) || (HelperFunction.IsKeyPress(Keys.Right) && checkCtrl))
            {
                if (checkShift)
                {
                    //move size
                    size = new Point(size.X + 1, size.Y);
                }
                else
                {
                    position = new Point(position.X + 1, position.Y);
                }
            }
            if ((HelperFunction.IsKeyDown(Keys.Up) && !checkCtrl) || (HelperFunction.IsKeyPress(Keys.Up) && checkCtrl))
            {
                if (checkShift)
                {
                    //move size
                    size = new Point(size.X, MathHelper.Clamp(size.Y - 1, 0, size.Y));
                }
                else
                {
                    position = new Point(position.X, MathHelper.Clamp(position.Y - 1, 0, position.Y));
                }
            }
            if ((HelperFunction.IsKeyDown(Keys.Down) && !checkCtrl) || (HelperFunction.IsKeyPress(Keys.Down) && checkCtrl))
            {
                if (checkShift)
                {
                    //move size
                    size = new Point(size.X, size.Y + 1);
                }
                else
                {
                    position = new Point(position.X, position.Y + 1);
                }
            }
            rekt.Location = position;
            rekt.Size = size;
            if (pictureBox_snippet.SourceRectangle != rekt || scaleChanged)
            {
                scaleChanged = false;
                pictureBox_snippet.SourceRectangle = rekt;
                label_snippet.Text = rekt.ToString();
                if (selectedFrame != null)
                {
                    selectedFrame.Location = new Point(pictureBox_spritesheet.Position.X + position.X * (int)pictureBox_spritesheet.Scale, pictureBox_spritesheet.Position.Y + position.Y * (int)pictureBox_spritesheet.Scale);
                    selectedFrame.Size = (size.ToVector2() * pictureBox_spritesheet.Scale).ToPoint();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            canvas.Draw(spriteBatch, gameTime);
            DrawingHelper.DrawRectangle(selectedFrame, Color.Red, false);
            base.Draw(spriteBatch, gameTime);
        }
    }
}
