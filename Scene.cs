using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UTendo_finalproject
{
    public class Scene
    {
        private Texture2D _background;
        
        private List<InteractableObject> _interactableObjects = new List<InteractableObject>();
        
        private List<PickableObject> _pickableObjects = new List<PickableObject>();

        public IReadOnlyList<InteractableObject> InteractableObjects => _interactableObjects.AsReadOnly();
        public IReadOnlyList<PickableObject> PickableObjects => _pickableObjects.AsReadOnly();

        public Scene(Texture2D backgroundTexture)
        {
            _background = backgroundTexture;
        }

        
        public void AddObject(InteractableObject obj)
        {
            _interactableObjects.Add(obj);
        }

        public void RemoveObject(InteractableObject obj)
        {
            _interactableObjects.Remove(obj);
        }

        public void AddPickable(PickableObject obj, Action<PickableObject> pickedUpAction)
        {
            obj.PickedUp += pickedUpAction;
            _pickableObjects.Add(obj);
        }

        public void RemovePickable(PickableObject obj)
        {
            _pickableObjects.Remove(obj);
        }


        public void Update(GameTime gameTime)
        {
            foreach (var obj in _interactableObjects)
            {
                obj.Update(gameTime);
            }

            for (int i = _pickableObjects.Count - 1; i >= 0; i--)
            {
                var item = _pickableObjects[i];
                item.Update(gameTime);

                if (item.IsPickedUp) 
                {
                    _pickableObjects.RemoveAt(i);
                    Debug.WriteLine($"Item removed from scene: {item.Name}");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {

            spriteBatch.Draw(_background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);


            foreach (var obj in _interactableObjects)
            {
                obj.Draw(spriteBatch);
            }

            foreach (var item in _pickableObjects)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}




