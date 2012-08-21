using System;
using System.Collections.Generic;
using DynaStudios.UI.Components;
using DynaStudios.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DynaStudios.UI.Screens
{
    public class HorizontalInfo : GameScreen
    {
        public Texture2D BackgroundTexture { get; set; }

        public List<IGuiItem> PanelEntries { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 PresentationOffset { get; set; }
        public int ItemPadding { get; set; }

        public HorizontalInfo(Texture2D background)
        {
            BackgroundTexture = background;

            PanelEntries = new List<IGuiItem>();

            //Window is always on top of at least one other screen
            IsPopup = true;
        }

        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            foreach (IGuiItem panelEntry in PanelEntries)
            {
                panelEntry.HandleInput(input);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 size = CalculatePanelEntries();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            //Draw Background
            Rectangle rec = new Rectangle((int) PresentationOffset.X, (int) PresentationOffset.Y, (int) Size.X, (int) Size.Y);
            spriteBatch.Draw(BackgroundTexture, rec, Color.White);

            foreach (IGuiItem panelEntry in PanelEntries)
            {
                panelEntry.Draw(this, gameTime);
            }

            spriteBatch.End();
        }

        private Vector2 CalculatePanelEntries()
        {

            Vector2 position = PresentationOffset;
            var viewport = ScreenManager.GraphicsDevice.Viewport;

            int width = 0, height = 0;
            int offsetCount = 0;

            float emptyXSpace = 0;
            if (PanelEntries.Count >= 2)
            {
                emptyXSpace = Size.X / PanelEntries.Count;
            }

            foreach (IGuiItem panelEntry in PanelEntries)
            {
                width += (int) panelEntry.GetWidth(this);
                height = (int) panelEntry.GetHeight(this);

                var startX = (offsetCount == 0)
                                 ? position.X + emptyXSpace / (PanelEntries.Count + 1.5f)
                                 : position.X + emptyXSpace * offsetCount;
                position.X = startX;
                position.Y = (PresentationOffset.Y + Size.Y)/2 + panelEntry.GetHeight(this)/2 - ItemPadding;

                panelEntry.Position = position;


                offsetCount++;
            }

            width += (PanelEntries.Count - 1)*ItemPadding;
            height += ItemPadding*2;

            return new Vector2(Math.Abs(width), Math.Abs(height));

        }
    }
}