using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace UTendo_finalproject
{
    public class SceneManager
    {
        private Dictionary<Vector3, Scene> _scenes = new Dictionary<Vector3, Scene>();

        public int CurrentX { get; private set; } = 0;
        public int CurrentY { get; private set; } = 0;
        public int CurrentZ { get; private set; } = 0;

        public Scene CurrentScene
        {
            get
            {
                if (_scenes.TryGetValue(new Vector3(CurrentX, CurrentY, CurrentZ), out Scene scene))
                {
                    return scene;
                }
                return null;
            }
        }

        public void AddScene(int x, int y, int z, Scene scene)
        {
            _scenes[new Vector3(x, y, z)] = scene;
        }

        public void SetStart(int x, int y, int z)
        {
            CurrentX = x;
            CurrentY = y;
            CurrentZ = z;
        }

        public bool CanMoveHorizontal(int dX, int dY)
        {
            int nextX = CurrentX + dX;
            int nextY = CurrentY + dY;
            Vector3 nextPosition = new Vector3(nextX, nextY, CurrentZ);
            
            bool isPCLBlock(int x, int y, int z) => z == 1 && x >= 1 && x <= 2 && y >= 1 && y <= 2;
            
            bool isCurrentPCLUpstairs = isPCLBlock(CurrentX, CurrentY, CurrentZ);
            bool isNextPCLUpstairs = isPCLBlock(nextX, nextY, CurrentZ);

            if (CurrentZ > 0 && !isCurrentPCLUpstairs)
            {
                Debug.WriteLine("Movement blocked: Horizontal movement is restricted to the ground floor or isolated PCL Upstairs block.");
                return false;
            }

            if (isCurrentPCLUpstairs)
            {
                if (!isNextPCLUpstairs)
                {
                    Debug.WriteLine("Movement blocked: Cannot move horizontally outside the PCL Upstairs block.");
                    return false;
                }
            }

            return _scenes.ContainsKey(nextPosition);
        }


        public bool CanMoveFloor(int dZ)
        {
            int nextZ = CurrentZ + dZ;
            Vector3 nextPosition = new Vector3(CurrentX, CurrentY, nextZ);

            return _scenes.ContainsKey(nextPosition);
        }

        public void Move(int dX, int dY)
        {
            if (CanMoveHorizontal(dX, dY))
            {
                CurrentX += dX;
                CurrentY += dY;
                Debug.WriteLine($"Moved to ({CurrentX}, {CurrentY}, {CurrentZ})");
            }
        }


        public void MoveFloor(int dZ)
        {
            if (CanMoveFloor(dZ))
            {
                CurrentZ += dZ;
                Debug.WriteLine($"Moved to floor {CurrentZ}");
            }
            else
            {
                Debug.WriteLine("Movement blocked: No floor found at the destination.");
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();
            CurrentScene?.Draw(spriteBatch, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            
            spriteBatch.End();
        }
    }
}
