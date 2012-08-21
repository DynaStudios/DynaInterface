using DynaStudios.UI.Input;
using DynaStudios.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DynaStudios.UI.Components
{
    public class IconButton : Button
    {

        public Texture2D Icon { get; set; }
        public bool ShowText { get; set; }

        public IconButton(Texture2D icon, string buttonText = "") : base(buttonText)
        {
            Icon = icon;
            ShowText = (buttonText != string.Empty);

            Size = new Vector2(128, 128);
        }

        public IconButton(Texture2D icon, Texture2D backgroundTexture, string buttonText = "")
            : base(buttonText, backgroundTexture)
        {
            Icon = icon;
            ButtonTexture = backgroundTexture;
            ShowText = (buttonText != string.Empty);

            Size = new Vector2(128, 128);
        }
        

        public override void Draw(GameScreen gameScreen, GameTime gameTime)
        {
            SpriteBatch spriteBatch = gameScreen.ScreenManager.SpriteBatch;
            SpriteFont font = gameScreen.ScreenManager.BigFont;

            ButtonTexture = ButtonTexture ?? gameScreen.ScreenManager.BlankTexture;

            var newPosition = Position;
            var textSize = font.MeasureString(Text);
            if (ShowText)
            {
                newPosition.Y -= textSize.Y / 1.5f;
            }

            Rectangle rec = new Rectangle((int)newPosition.X, (int)newPosition.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(Icon, rec, Color.White);

            var textPosition = newPosition;
            textPosition.Y += Size.Y + 10;
            textPosition.X += (Size.X - textSize.X)/2;
            spriteBatch.DrawString(font, Text, textPosition, TextColor);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }
    }
}