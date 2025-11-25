using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UTendo_finalproject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneManager _sceneManager; 
        
        private bool _isInventoryOpen = false;
        private InteractableObject _selectedObject = null;

        private KeyboardState _prevKeyboard;
        private MouseState _prevMouse;

        private SpriteFont _font;
        private InventoryMenu _inventoryMenu; 

        private Texture2D t_union, t_unionUnderground, t_tower, t_towerUpstairs, t_mainMall, t_towerTop, t_PCL;
        private Texture2D t_PCLupSW, t_PCLupSE, t_PCLupNW, t_PCLupNE;
        private Texture2D t_speedwayNorth, t_speedwayCenter, t_speedwaySouth, t_jesterGarage;
        private Texture2D t_GDC, t_GDCupstairs, t_GDCbasement, t_gregory;
        private Texture2D t_rock, t_coin, t_keyCard; 
        private Texture2D t_UTidol; 
        
        private Texture2D t_bookBlue, t_bookRed, t_bookYellow, t_bookGreen;
        private Dictionary<string, (PickableObject item, int count)> _inventory = new Dictionary<string, (PickableObject, int)>();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 960;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _sceneManager = new SceneManager();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            t_union             = Content.Load<Texture2D>("imgs/union");
            t_unionUnderground  = Content.Load<Texture2D>("imgs/unionUnderground");
            t_tower             = Content.Load<Texture2D>("imgs/tower");
            t_towerUpstairs     = Content.Load<Texture2D>("imgs/towerUpstairs");
            t_towerTop          = Content.Load<Texture2D>("imgs/towerTop");
            t_PCL               = Content.Load<Texture2D>("imgs/PCL");
            t_mainMall          = Content.Load<Texture2D>("imgs/mainMall"); 
            t_PCLupSW           = Content.Load<Texture2D>("imgs/PCLupstairsSouthWest");
            t_PCLupSE           = Content.Load<Texture2D>("imgs/PCLupstairsSouthEast");
            t_PCLupNW           = Content.Load<Texture2D>("imgs/PCLupstairsNorthWest");
            t_PCLupNE           = Content.Load<Texture2D>("imgs/PCLupstairsNorthEast");
            t_speedwayNorth     = Content.Load<Texture2D>("imgs/speedwayNorth");
            t_speedwayCenter    = Content.Load<Texture2D>("imgs/speedwayCenter");
            t_speedwaySouth     = Content.Load<Texture2D>("imgs/speedwaySouth");
            t_jesterGarage      = Content.Load<Texture2D>("imgs/jesterGarage");
            t_GDC               = Content.Load<Texture2D>("imgs/GDC");
            t_GDCupstairs       = Content.Load<Texture2D>("imgs/GDCupstairs");
            t_GDCbasement       = Content.Load<Texture2D>("imgs/GDCbasement");
            t_gregory           = Content.Load<Texture2D>("imgs/gregory");
            t_rock              = Content.Load<Texture2D>("imgs/rock");
            t_coin              = Content.Load<Texture2D>("imgs/coin"); 
            t_keyCard           = Content.Load<Texture2D>("imgs/keyCard"); 
            t_UTidol            = Content.Load<Texture2D>("imgs/UTidol"); 
            
            t_bookBlue          = Content.Load<Texture2D>("imgs/book_blue");
            t_bookRed           = Content.Load<Texture2D>("imgs/book_red");
            t_bookYellow        = Content.Load<Texture2D>("imgs/book_yellow");
            t_bookGreen         = Content.Load<Texture2D>("imgs/book_green");
            
            _font               = Content.Load<SpriteFont>("fonts/DefaultFont");
            
            _inventoryMenu = new InventoryMenu(GraphicsDevice, _font); 

            var union = new Scene(t_union);
            var rockUnion = new InteractableObject(t_rock, new Vector2(300, 350)) { Name = "Union Rock" };
            rockUnion.Clicked += HandleObjectClicked;
            union.AddObject(rockUnion);

            var unionUnderground = new Scene(t_unionUnderground);

            var vendingMachine = new CoinDispenserObject(Content.Load<Texture2D>("imgs/vendingMachine"), new Vector2(500, 400));

            vendingMachine.CoinDispensed += HandleCoinDispensed;
            unionUnderground.AddObject(vendingMachine);

            var bowlingBall = new PickableObject(Content.Load<Texture2D>("imgs/bowlingBall"), new Vector2(250, 420))
            {
                Name = "Bowling Ball"
            };

            unionUnderground.AddPickable(bowlingBall, HandlePickablePicked);

            _sceneManager.AddScene(0, 2, 0, union);
            _sceneManager.AddScene(0, 2, -1, unionUnderground);

            _sceneManager.AddScene(1, 3, 0, new Scene(t_tower));
            _sceneManager.AddScene(1, 3, 1, new Scene(t_towerUpstairs));
            
            var towerTop = new Scene(t_towerTop);
            
            if (t_UTidol != null)
            {
                var utIdol = new InteractableObject(t_UTidol, new Vector2(400, 300)) { Name = "UTidol" };
                utIdol.Clicked += HandleObjectClicked;
                towerTop.AddObject(utIdol);
            }
            else
            {
                Debug.WriteLine("CRITICAL ERROR: 'imgs/UTidol' asset failed to load. UTidol object skipped.");
            }
            _sceneManager.AddScene(1, 3, 2, towerTop);

            _sceneManager.AddScene(1, 2, 0, new Scene(t_mainMall));

            var pcl = new Scene(t_PCL);

            pcl.AddPickable(new PickableObject(t_bookBlue, new Vector2(100, 300)) { Name = "Blue Book" }, HandlePickablePicked);
            pcl.AddPickable(new PickableObject(t_bookRed, new Vector2(200, 300)) { Name = "Red Book" }, HandlePickablePicked);
            pcl.AddPickable(new PickableObject(t_bookYellow, new Vector2(300, 300)) { Name = "Yellow Book" }, HandlePickablePicked);
            pcl.AddPickable(new PickableObject(t_bookGreen, new Vector2(400, 300)) { Name = "Green Book" }, HandlePickablePicked);

            _sceneManager.AddScene(1, 1, 0, pcl);

            var pclUpSW = new Scene(t_PCLupSW);
            var pclUpSE = new Scene(t_PCLupSE);
            var pclUpNW = new Scene(t_PCLupNW);
            var pclUpNE = new Scene(t_PCLupNE);

            Texture2D t_blankSpot = Content.Load<Texture2D>("imgs/blankSpot");

            var blankSpot1 = new InteractableObject(t_blankSpot, new Vector2(350, 350)) { Name = "Blank Spot" };
            blankSpot1.Clicked += HandleObjectClicked;
            pclUpSW.AddObject(blankSpot1);
            
            var blankSpot2 = new InteractableObject(t_blankSpot, new Vector2(350, 350)) { Name = "Blank Spot" };
            blankSpot2.Clicked += HandleObjectClicked;
            pclUpSE.AddObject(blankSpot2);
            
            var blankSpot3 = new InteractableObject(t_blankSpot, new Vector2(350, 350)) { Name = "Blank Spot" };
            blankSpot3.Clicked += HandleObjectClicked;
            pclUpNW.AddObject(blankSpot3);

            var blankSpot4 = new InteractableObject(t_blankSpot, new Vector2(350, 350)) { Name = "Blank Spot" };
            blankSpot4.Clicked += HandleObjectClicked;
            pclUpNE.AddObject(blankSpot4);

            _sceneManager.AddScene(1, 1, 1, pclUpSW);
            _sceneManager.AddScene(1, 2, 1, pclUpSE);
            _sceneManager.AddScene(2, 1, 1, pclUpNW);
            _sceneManager.AddScene(2, 2, 1, pclUpNE);

            _sceneManager.AddScene(2, 3, 0, new Scene(t_speedwayNorth));
            _sceneManager.AddScene(2, 2, 0, new Scene(t_speedwayCenter));
            _sceneManager.AddScene(2, 1, 0, new Scene(t_speedwaySouth));

            var jesterGarage = new Scene(t_jesterGarage);
            var shadyMan = new InteractableObject(Content.Load<Texture2D>("imgs/shadyMan"), new Vector2(450, 350))
            {
                Name = "Shady Man"
            };
            shadyMan.Clicked += HandleObjectClicked;
            jesterGarage.AddObject(shadyMan);

            var dumbbell = new PickableObject(Content.Load<Texture2D>("imgs/dumbbell"), new Vector2(300, 380))
            {
                Name = "Dumbbell"
            };


            _sceneManager.AddScene(2, 0, 0, jesterGarage);

            _sceneManager.AddScene(3, 2, 0, new Scene(t_GDC));
            _sceneManager.AddScene(3, 2, 1, new Scene(t_GDCupstairs));
            _sceneManager.AddScene(3, 2, -1, new Scene(t_GDCbasement));

            var gregory = new Scene(t_gregory);
            var dumbbell2 = new PickableObject(Content.Load<Texture2D>("imgs/dumbbell"), new Vector2(300, 380))
            {
                Name = "Dumbbell"
            };
            gregory.AddPickable(dumbbell2, HandlePickablePicked);

            _sceneManager.AddScene(3, 1, 0, gregory);

            _sceneManager.SetStart(3, 2, 1);
        }

        private void HandleObjectClicked(InteractableObject obj)
        {
            _isInventoryOpen = true;
            _selectedObject = obj;
            Debug.WriteLine($"Interactable clicked: {obj.Name}. Opening inventory.");
        }

        private void HandlePickablePicked(PickableObject item)
        {
            AddItemToInventory(item);
            Debug.WriteLine($"Picked up: {item.Name}");
        }
        private void HandleCoinDispensed(string itemName)
        {
            Debug.WriteLine($"Dispensing item: {itemName}");

            if (itemName == "Coin")
            {
                var coinItem = new PickableObject(t_coin, Vector2.Zero) { Name = "Coin" }; 
                
                AddItemToInventory(coinItem);
                
                Debug.WriteLine($"Received a Coin. Current count: {_inventory[itemName].count}");
            }
        }
        
        private void AddItemToInventory(PickableObject item)
        {
            if (_inventory.ContainsKey(item.Name))
            {
                var entry = _inventory[item.Name];
                _inventory[item.Name] = (entry.item, entry.count + 1);
            }
            else
            {
                _inventory.Add(item.Name, (item, 1));
            }
        }
        
        private void RemoveItemFromInventory(string itemName, int count)
        {
            if (_inventory.ContainsKey(itemName))
            {
                var entry = _inventory[itemName];
                int newCount = entry.count - count;
                
                if (newCount <= 0)
                {
                    _inventory.Remove(itemName);
                    Debug.WriteLine($"Removed all instances of {itemName}.");
                }
                else
                {
                    _inventory[itemName] = (entry.item, newCount);
                    Debug.WriteLine($"Removed {count} of {itemName}. {newCount} remaining.");
                }
            }
        }


        protected override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            // movement
            if (IsNewKey(kb, Keys.W) && _sceneManager.CanMoveHorizontal(0, 1))
                _sceneManager.Move(0, 1);
            if (IsNewKey(kb, Keys.S) && _sceneManager.CanMoveHorizontal(0, -1))
                _sceneManager.Move(0, -1);
            if (IsNewKey(kb, Keys.D) && _sceneManager.CanMoveHorizontal(1, 0))
                _sceneManager.Move(1, 0);
            if (IsNewKey(kb, Keys.A) && _sceneManager.CanMoveHorizontal(-1, 0))
                _sceneManager.Move(-1, 0);

            if (IsNewKey(kb, Keys.Up))
                _sceneManager.MoveFloor(+1);
            
            if (IsNewKey(kb, Keys.Down))
            {
                int currentX = _sceneManager.CurrentX;
                int currentY = _sceneManager.CurrentY;
                int currentZ = _sceneManager.CurrentZ;

                bool isPCLUpstairs = currentZ == 1 && currentX >= 1 && currentX <= 2 && currentY >= 1 && currentY <= 2;
                
                bool isPCLSW = currentX == 1 && currentY == 1 && currentZ == 1;

                if (isPCLUpstairs && !isPCLSW)
                {
                    Debug.WriteLine("Movement blocked: Must use the stairs in PCL SW.");
                }
                else if (_sceneManager.CanMoveFloor(-1))
                {
                    _sceneManager.MoveFloor(-1);
                }
            }

            _sceneManager.Update(gameTime); 

            if (_isInventoryOpen)
                UpdateInventory();

            _prevKeyboard = kb;
            base.Update(gameTime);
        }


        private bool IsNewKey(KeyboardState kb, Keys key)
        {
            return kb.IsKeyDown(key) && !_prevKeyboard.IsKeyDown(key);
        }
        
        private string GetClickedInventoryItem(MouseState ms)
        {
            const int MenuWidth = 600;
            const int Padding = 20;
            const int SlotSize = 64;
            const int SlotSpacing = 10;
            
            var vp = GraphicsDevice.Viewport;
            int xMenu = (vp.Width - MenuWidth) / 2;
            int yMenu = (vp.Height - 250) / 2;
            Rectangle menuBounds = new Rectangle(xMenu, yMenu, MenuWidth, 250);

            float headerHeight = _font.MeasureString("Inventory (Click item to use)").Y;
            Vector2 itemStartPos = new Vector2(menuBounds.X + Padding, menuBounds.Y + Padding + headerHeight + Padding);

            int i = 0;
            foreach (var entry in _inventory)
            {
                string itemName = entry.Key;
                
                int x = (int)(itemStartPos.X + i * (SlotSize + SlotSpacing));
                int y = (int)itemStartPos.Y;

                if (x + SlotSize > menuBounds.Right - Padding)
                    break;

                Rectangle itemRect = new Rectangle(x, y, SlotSize, SlotSize);

                if (itemRect.Contains(ms.X, ms.Y))
                {
                    return itemName;
                }

                i++;
            }
            return null;
        }


        private void UpdateInventory()
        {
            var ms = Mouse.GetState();
            bool newClick = ms.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released;

            if (newClick)
            {
                string clickedItemName = GetClickedInventoryItem(ms);
                
                if (clickedItemName != null && _selectedObject != null)
                {
                    UseItemOnObject(clickedItemName, _selectedObject);
                }
                else if (!_inventoryMenu.Bounds.Contains(ms.X, ms.Y))
                {
                    _isInventoryOpen = false;
                    _selectedObject = null;
                    Debug.WriteLine("Inventory closed by clicking outside.");
                }
            }
            _prevMouse = ms;
        }

        private void UseItemOnObject(string itemName, InteractableObject obj)
        {
            Debug.WriteLine($"Attempting to use {itemName} on {obj.Name}");

            if (obj.Name == "Shady Man" && itemName == "Coin")
            {
                if (_inventory.ContainsKey("Coin") && _inventory["Coin"].count >= 25)
                {
                    RemoveItemFromInventory("Coin", 25);
                    
                    var keyCard = new PickableObject(t_keyCard, Vector2.Zero) { Name = "Key Card" };
                    AddItemToInventory(keyCard);
                    
                    Debug.WriteLine("Trade successful: 25 Coins exchanged for Key Card.");
                    
                    _isInventoryOpen = false;
                    _selectedObject = null;
                    return; 
                }
                else
                {
                    Debug.WriteLine("Trade failed: Not enough coins (requires 25).");
                }
            }

            if (obj.Name == "UTidol")
            {
                bool acquisitionSuccessful = false;
                string successMessage = "";

                if (itemName == "Key Card" && _inventory.ContainsKey("Key Card") && _inventory["Key Card"].count >= 1)
                {
                    RemoveItemFromInventory("Key Card", 1);
                    successMessage = "Success! Key Card used to unlock the UTidol's case.";
                    acquisitionSuccessful = true;
                }
                else if ((itemName == "Bowling Ball" || itemName == "Dumbbell") && _inventory.ContainsKey(itemName) && _inventory[itemName].count >= 1)
                {
                    RemoveItemFromInventory(itemName, 1);
                    successMessage = $"Smash! {itemName} used to forcefully acquire the UTidol!";
                    acquisitionSuccessful = true;
                }
                else if (itemName == "Key Card" || itemName == "Bowling Ball" || itemName == "Dumbbell")
                {
                    Debug.WriteLine($"Interaction failed: You need a {itemName} to proceed, but you don't have enough.");
                }
                else
                {
                    Debug.WriteLine($"The {itemName} has no effect on the UTidol.");
                }

                if (acquisitionSuccessful && _sceneManager.CurrentScene != null)
                {
                    Debug.WriteLine(successMessage);
                    
                    _sceneManager.CurrentScene.RemoveObject(obj);

                    var utIdolItem = new PickableObject(t_UTidol, Vector2.Zero) { Name = "UTidol" };
                    AddItemToInventory(utIdolItem);

                    Debug.WriteLine("You acquired the UTidol and it has been added to your inventory!");

                    _isInventoryOpen = false;
                    _selectedObject = null;
                    return;
                }
                else if (acquisitionSuccessful)
                {
                     Debug.WriteLine("ERROR: Cannot acquire UTidol: SceneManager.CurrentScene is null.");
                }
                
                _isInventoryOpen = false;
                _selectedObject = null;
                return;
            }

            if (obj.Name == "Blank Spot")
            {
                string[] bookNames = { "Blue Book", "Red Book", "Yellow Book", "Green Book" };

                if (bookNames.Contains(itemName))
                {
                    Texture2D bookTexture = null;
                    switch (itemName)
                    {
                        case "Blue Book": bookTexture = t_bookBlue; break;
                        case "Red Book": bookTexture = t_bookRed; break;
                        case "Yellow Book": bookTexture = t_bookYellow; break;
                        case "Green Book": bookTexture = t_bookGreen; break;
                    }

                    if (bookTexture != null)
                    {
                        RemoveItemFromInventory(itemName, 1);

                        var placedBook = new PickableObject(bookTexture, obj.Position) 
                        { 
                            Name = itemName
                        };
                        
                        if (_sceneManager.CurrentScene != null)
                        {
                            _sceneManager.CurrentScene.AddPickable(placedBook, HandlePickablePicked);

                            Debug.WriteLine($"{itemName} successfully placed at the Blank Spot. It is now pickable again.");
                        }
                        else
                        {
                            Debug.WriteLine("ERROR: Cannot place book or remove blank spot: SceneManager.CurrentScene is null.");
                        }

                        _isInventoryOpen = false;
                        _selectedObject = null;
                        return;
                    }
                }
            }

            _isInventoryOpen = false;
            _selectedObject = null;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _sceneManager.Draw(_spriteBatch, GraphicsDevice);

            if (_isInventoryOpen)
            {
                _spriteBatch.Begin();
                _inventoryMenu.Draw(_spriteBatch, _inventory);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}