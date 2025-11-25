using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace UTendo_finalproject
{
    public class PickableObject : InteractableObject
    {
        public event Action<PickableObject> PickedUp;

        public bool IsPickedUp { get; private set; } = false;

        public PickableObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        protected override void OnClicked(MouseState currentMouse)
        {
            Debug.WriteLine($"Pickable object {Name} clicked.");
            
            PickedUp?.Invoke(this); 

            IsPickedUp = true;
            
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsPickedUp)
            {
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsPickedUp)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}










