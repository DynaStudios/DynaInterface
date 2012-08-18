using System;
using System.Collections.Generic;
using DynaStudios.UI.Components.Items;
using DynaStudios.UI.Input;
using DynaStudios.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DynaStudios.UI.Components
{
    public class DropDown : IGuiItem
    {
        private bool _collapsed;
        private Texture2D _dropDownBackground;
        private Vector2 _dropDownItemSize;
        private bool _hovered;
        private bool _playedHoverSound;

        public DropDown(List<DropDownItem> items, DropDownItem selectedItem)
        {
            Items = items;
            SelectedItem = selectedItem;

            _collapsed = true;
            Size = new Vector2(190, 30);
        }

        public List<DropDownItem> Items { get; protected set; }
        public DropDownItem SelectedItem { get; protected set; }

        #region IGuiItem Members

        public Vector2 Position { get; set; }
        public int ZIndex { get; set; }
        public SoundEffect HoverSound { get; set; }
        public SoundEffect ClickSound { get; set; }

        public Vector2 Size { get; set; }

        //DropDown Properties

        public float GetWidth(GameScreen gameScreen)
        {
            return Size.X;
        }

        public float GetHeight(GameScreen gameScreen)
        {
            return Size.Y;
        }

        public void Update(GameScreen gameScreen, GameTime gameTime)
        {
        }

        public void Draw(GameScreen gameScreen, GameTime gameTime)
        {
            SpriteBatch spriteBatch = gameScreen.ScreenManager.SpriteBatch;
            SpriteFont font = gameScreen.ScreenManager.Font;
            SpriteFont smallFont = gameScreen.ScreenManager.SmallFont;

            if (_dropDownBackground == null)
            {
                _dropDownBackground = gameScreen.ScreenManager.Game.Content.Load<Texture2D>("Images/Game/Menu/btnBg");
            }

            //Draw DropDown InnerBox
            var rec = new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
            spriteBatch.Draw(_dropDownBackground, rec, Color.Gray);

            //Draw the Text centered in the DropDown
            Vector2 textSize = font.MeasureString(SelectedItem.Label);
            Vector2 textPosition = new Vector2(rec.Center.X, rec.Center.Y) - textSize/2f;
            textPosition.X = (int) textPosition.X;
            textPosition.Y = (int) textPosition.Y;
            spriteBatch.DrawString(font, SelectedItem.Label, textPosition, Color.White);

            //Draw DropDown Content
            if (!_collapsed && Items.Count != 0)
            {
                Vector2 position = Position;
                _dropDownItemSize = smallFont.MeasureString(Items[0].Label);

                //Initial Position
                position.Y += Size.Y;

                for (int i = 0; i < Items.Count; i++)
                {
                    DropDownItem item = Items[i];

                    //Enable Zebra Coloring
                    bool even = (i%2 == 0);

                    item.Draw(gameScreen, position, Size, even);
                    position.Y += _dropDownItemSize.Y;
                }
            }
        }

        public void HandleInput(InputState input)
        {
            var mousePosition = new Vector2(input.MouseState.X, input.MouseState.Y);

            //Check if DropDows is collapsed
            if (_collapsed)
            {
                //Check if Mouse is inside DropDownBox
                if (mousePosition.X >= Position.X && mousePosition.Y >= Position.Y &&
                    mousePosition.X <= Position.X + Size.X &&
                    mousePosition.Y <= Position.Y + Size.Y)
                {
                    _hovered = true;

                    if (!_playedHoverSound && HoverSound != null)
                    {
                        HoverSound.Play();
                        _playedHoverSound = true;
                    }

                    if (input.LeftMouseClicked)
                    {
                        _collapsed = false;
                    }
                }
                else
                {
                    _playedHoverSound = false;
                    _hovered = false;
                }
            }
            else
            {
                //Check if Item gets selected
                float expandedDropDownSize = Size.Y + (_dropDownItemSize.Y*Items.Count);
                if (mousePosition.X >= Position.X && mousePosition.Y >= Position.Y &&
                    mousePosition.X <= Position.X + Size.X &&
                    mousePosition.Y <= Position.Y + expandedDropDownSize)
                {
                    //Mouse is inside DropDown or DropDownItems
                    if (mousePosition.X >= Position.X && mousePosition.Y >= Position.Y &&
                        mousePosition.X <= Position.X + Size.X &&
                        mousePosition.Y <= Position.Y + Size.Y)
                    {
                        //Mouse is just over DropDown Box
                        if (input.LeftMouseClicked)
                        {
                            _collapsed = true;
                        }
                    }
                    else
                    {
                        //Mouse Clicked an DropDownItem!
                        if (input.LeftMouseClicked)
                        {
                            double selectedItem = Math.Floor((mousePosition.Y - Position.Y)/_dropDownItemSize.Y);

                            if (selectedItem - 1 < Items.Count)
                            {
                                SelectedItem = Items[(int) (selectedItem - 1)];
                                OnSelectionChanged(new EventArgs());
                            }
                            _collapsed = true;
                        }
                    }
                }
                else
                {
                    if (input.LeftMouseClicked)
                    {
                        _collapsed = true;
                    }
                }
            }
        }

        #endregion

        //DropDownEvents
        public event EventHandler<EventArgs> Clicked;

        public void OnClicked(EventArgs e)
        {
            EventHandler<EventArgs> handler = Clicked;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<EventArgs> SelectionChanged;

        public void OnSelectionChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = SelectionChanged;
            if (handler != null) handler(this, e);
        }
    }
}