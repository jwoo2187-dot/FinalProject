using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UTendo_finalproject
{
    public class InventoryMenu
    {
        private Texture2D _backgroundTexture;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;

        private const int MenuWidth = 600;
        private const int MenuHeight = 250;
        private const int Padding = 20;
        private const int SlotSize = 64;
        private const int SlotSpacing = 10;
        
        private static readonly Color CountColor = Color.White;
        private static readonly Color TitleColor = Color.LightGray;


        public Rectangle Bounds { get; private set; }


        public InventoryMenu(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            _font = font;

            _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { Color.Black * 0.7f });

            var vp = graphicsDevice.Viewport;
            int x = (vp.Width - MenuWidth) / 2;
            int y = (vp.Height - MenuHeight) / 2;
            Bounds = new Rectangle(x, y, MenuWidth, MenuHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Dictionary<string, (PickableObject item, int count)> inventory)
        {
            spriteBatch.Draw(_backgroundTexture, Bounds, Color.White);

            spriteBatch.DrawString(_font, "Inventory (Click item to use)", new Vector2(Bounds.X + Padding, Bounds.Y + Padding), TitleColor);

            Vector2 itemStartPos = new Vector2(Bounds.X + Padding, Bounds.Y + Padding + _font.MeasureString("Inventory").Y + Padding);

            int i = 0;
            foreach (var inventoryEntry in inventory.Values)
            {
                var item = inventoryEntry.item;
                var count = inventoryEntry.count;


                int x = (int)(itemStartPos.X + i * (SlotSize + SlotSpacing));
                int y = (int)itemStartPos.Y;

                if (x + SlotSize > Bounds.Right - Padding)
                    break;
                
                Rectangle itemRect = new Rectangle(x, y, SlotSize, SlotSize);

                spriteBatch.Draw(item.Texture, itemRect, Color.White);
                
                if (count > 1)
                {
                    string countText = count.ToString();
                    Vector2 textSize = _font.MeasureString(countText);
                    
                    Vector2 textPos = new Vector2(
                        x + SlotSize - textSize.X - 2,
                        y + SlotSize - textSize.Y - 2
                    );

                    spriteBatch.DrawString(_font, countText, textPos, CountColor);
                }

                i++;
            }
        }
    }
}