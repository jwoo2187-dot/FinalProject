using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace UTendo_finalproject
{
    public class InteractableObject
    {
        public string Name { get; set; }
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public event Action<InteractableObject> Clicked;

        protected MouseState _prevMouse;

        public InteractableObject(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            var currentMouse = Mouse.GetState();
            
            bool newClick = currentMouse.LeftButton == ButtonState.Pressed && 
                            _prevMouse.LeftButton == ButtonState.Released;

            if (newClick && Bounds.Contains(currentMouse.Position))
            {
                OnClicked(currentMouse);
            }

            _prevMouse = currentMouse;
        }


        protected virtual void OnClicked(MouseState currentMouse)
        {
            Clicked?.Invoke(this); 
            Debug.WriteLine($"Default click handler fired for {Name}");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}



