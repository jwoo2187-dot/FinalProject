using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace UTendo_finalproject
{
    public class CoinDispenserObject : InteractableObject
    {
        public event Action<string> CoinDispensed;

        public CoinDispenserObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Name = "Vending Machine";
        }
        
        protected override void OnClicked(MouseState currentMouse)
        {
            Debug.WriteLine($"{Name} clicked. Dispensing Coin...");
            
            CoinDispensed?.Invoke("Coin"); 
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}