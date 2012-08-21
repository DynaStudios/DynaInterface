using System;
using DynaStudios.UI.Input;
using DynaStudios.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DynaStudios.UI.Components
{
    public class Button : IGuiItem
    {
        protected Texture2D ButtonTexture;
        public bool IsHovered;
        private bool _playedClickSound;
        private bool _playedHoverSound;

        public Button(string buttonText)
        {
            Text = buttonText;

            InitVars();
        }

        public Button(string buttonText, Texture2D backgroundTexture)
        {
            Text = buttonText;
            ButtonTexture = backgroundTexture;

            InitVars();
        }

        public Color FillColor { get; set; }
        public Color HoverColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }

        public float Alpha { get; set; }
        public int BorderThickness { get; set; }

        public string Text { get; protected set; }

        #region IGuiItem Members

        public Vector2 Position { get; set; }
        public int ZIndex { get; set; }
        public SoundEffect HoverSound { get; set; }
        public SoundEffect ClickSound { get; set; }

        //Button Properties
        public Vector2 Size { get; set; }

        public float GetWidth(GameScreen gameScreen)
        {
            return (int) Size.X;
        }

        public float GetHeight(GameScreen gameScreen)
        {
            return Size.Y;
        }

        public void Update(GameScreen gameScreen, GameTime gameTime)
        {
            //todo: I don't know yet ^^
        }

        public virtual void Draw(GameScreen gameScreen, GameTime gameTime)
        {
            SpriteBatch spriteBatch = gameScreen.ScreenManager.SpriteBatch;
            SpriteFont font = gameScreen.ScreenManager.Font;

            ButtonTexture = ButtonTexture ?? gameScreen.ScreenManager.BlankTexture;

            var rec = new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);

            //Fill Button
            if (IsHovered)
            {
                spriteBatch.Draw(ButtonTexture, rec, HoverColor*Alpha);
            }
            else
            {
                spriteBatch.Draw(ButtonTexture, rec, FillColor*Alpha);
            }

            //Draw Border
            spriteBatch.Draw(ButtonTexture, new Rectangle(rec.Left, rec.Top, rec.Width, BorderThickness),
                             BorderColor*Alpha);
            spriteBatch.Draw(ButtonTexture, new Rectangle(rec.Left, rec.Top, BorderThickness, rec.Height),
                             BorderColor*Alpha);
            spriteBatch.Draw(ButtonTexture,
                             new Rectangle(rec.Right - BorderThickness, rec.Top, BorderThickness, rec.Height),
                             BorderColor*Alpha);
            spriteBatch.Draw(ButtonTexture,
                             new Rectangle(rec.Left, rec.Bottom - BorderThickness, rec.Width, BorderThickness),
                             BorderColor*Alpha);

            //Draw the Text centered in the button
            if (Text != "")
            {
                Vector2 textSize = font.MeasureString(Text);
                Vector2 textPosition = new Vector2(rec.Center.X, rec.Center.Y) - textSize/2f;
                textPosition.X = (int) textPosition.X;
                textPosition.Y = (int) textPosition.Y;
                spriteBatch.DrawString(font, Text, textPosition, TextColor*Alpha);
            }
        }

        public virtual void HandleInput(InputState input)
        {
            if (input.MouseState.IsMouseIn(Position, Size))
            {
                IsHovered = true;

                if (!_playedHoverSound && HoverSound != null)
                {
                    HoverSound.Play();
                    _playedHoverSound = true;
                }

                if (input.LeftMouseClicked)
                {
                    OnClicked();
                }
            }
            else
            {
                IsHovered = false;
                _playedHoverSound = false;
            }
        }

        #endregion

        public virtual event EventHandler<EventArgs> Clicked;

        public void InitVars()
        {
            //Set Button Default Values
            Size = new Vector2(190, 40);
            FillColor = Color.LightGreen;
            HoverColor = Color.White;
            BorderColor = Color.Green;
            TextColor = Color.White;
            Alpha = 1f;
            BorderThickness = 2;
        }

        protected virtual void OnClicked()
        {
            if (Clicked != null)
            {
                if (ClickSound != null)
                {
                    ClickSound.Play();
                    _playedClickSound = true;
                }
                Clicked(this, EventArgs.Empty);
            }
        }
    }
}