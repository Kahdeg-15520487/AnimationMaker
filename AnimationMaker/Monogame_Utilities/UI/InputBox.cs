﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utilities.Drawing;
using Utilities.Utility;

namespace Utilities.UI
{
    public class InputBox : UIObject
    {
        StringBuilder textBuffer = new StringBuilder();

        private string temp_text;
        /// <summary>
        /// The current text in the buffer. Assign text to this will clear the buffer.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return textBuffer.ToString();
            }
            set
            {
                textBuffer.Clear();
                textBuffer.Append(value);
            }
        }
        public override Vector2 Size
        {
            get
            {
                return base.Size;
            }

            set
            {
                base.Size = value;
            }
        }
        //Chau Van Sang adds 
        /// <summary>
        /// Speed of flicker (change between "" and "|")
        /// </summary>       
        private const int speed_flick = 25;
        /// <summary>
        /// Increase until equal to speed_flicker and change isCursor_flicker value then reset to 0
        /// </summary>
        private int temp_speed_flicker = 0;
        /// <summary>
        /// Use to change between "" and "|"
        /// </summary>
        private bool isCursor_flicker = false;

        public Color caretColor { get; set; } = Color.DarkGray;
        public int CursorPosition { get; set; }
        public List<char> ignoreCharacter;
        private int maxTextLength;
        private int textSpacing;

        public InputBox(string text, Point position, Vector2? size, SpriteFont font, Color foregroundColor, Color backgroundColor)
        {
            Text = text;
            Position = position;
            if (size != null)
            {
                Size = size.Value;
            }
            else
            {

            }
            this.Font = font;
            this.foregroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
            CursorPosition = 0;
            maxTextLength = findMaxTextLength();
            textSpacing = rect.Width / maxTextLength;
            ignoreCharacter = new List<char>();

            CONTENT_MANAGER.gameInstance.Window.TextInput += TextInputHandler;
        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            if (isFocused)
            {
                if (Font.Characters.Contains(e.Character) && !ignoreCharacter.Contains(e.Character))
                {
                    if (Font.MeasureString(textBuffer).X == rect.Width)
                    {
                        temp_text = textBuffer.ToString();
                        textBuffer.Append(e.Character);
                    }
                    if (Font.MeasureString(textBuffer).X > rect.Width - 1)
                    {
                        textBuffer.Append(e.Character);
                        return;
                    }

                    textBuffer.Append(e.Character);

                    CursorPosition++;
                }
            }
        }

        private int findMaxTextLength()
        {
            string teststr = "A";
            while (Font.MeasureString(teststr).X < rect.Width)
            {
                teststr += "A";
            }
            return teststr.Length;
        }

        public override void Update(GameTime gameTime, InputState inputState, InputState lastInputState)
        {
            base.Update(gameTime, inputState, lastInputState);

            //Chau Van Sang adds isCursor for cursor flickering
            if (isFocused == true)
            {
                temp_speed_flicker += 1;
                if (temp_speed_flicker == speed_flick)
                {
                    isCursor_flicker = !isCursor_flicker;
                    temp_speed_flicker = 0;
                }

                if (HelperFunction.IsKeyPress(Keys.Back))
                {
                    if (textBuffer.Length > 0)
                    {
                        if (Font.MeasureString(textBuffer).X > rect.Width)
                        {
                            textBuffer.Remove(textBuffer.Length - 1, 1);
                            return;
                        }
                        textBuffer.Remove((CursorPosition - 1).Clamp(textBuffer.Length, 0), 1);
                        CursorPosition--;
                    }
                }
            }
        }

        public void Clear()
        {
            textBuffer.Clear();
            CursorPosition = 0;
        }

        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            try
            {
                if (Font.MeasureString(textBuffer).X <= rect.Width)
                {
                    spriteBatch.DrawString(Font, textBuffer, rect.Location.ToVector2(), foregroundColor, Rotation, origin, scale, SpriteEffects.None, LayerDepth.GuiLower);
                }
                else
                {
                    spriteBatch.DrawString(Font, temp_text, rect.Location.ToVector2(), foregroundColor, Rotation, origin, scale, SpriteEffects.None, LayerDepth.GuiLower);
                }
            }
            catch (Exception e)
            {
                CONTENT_MANAGER.Log(e.Message);
            }

            //Draw text caret
            spriteBatch.DrawString(Font, IsFocused ? isCursor_flicker ? "" : "|" : "|", rect.Location.ToVector2() + new Vector2(CursorPosition * textSpacing - 5, -2), caretColor);

            DrawingHelper.DrawRectangle(rect, BackgroundColor, true);
        }
    }
}