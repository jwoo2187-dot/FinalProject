using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UTendo_finalproject
{
    public abstract class GameObject
    {
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; protected set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        public string Name { get; set; } = "Object";
        private MouseState _prevMouse;

        public GameObject(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        protected abstract void OnClick(MouseState mouseState);

        public virtual void Update(GameTime gameTime)
        {
            var currentMouse = Mouse.GetState();
            
            bool newClick = currentMouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released;

            if (newClick && BoundingBox.Contains(currentMouse.Position))
            {
                OnClick(currentMouse);
            }

            _prevMouse = currentMouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}