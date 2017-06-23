using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Water
{
    class StageGameState : IGameState
    {
        private Dictionary<String, Stage> _stages;
        private Stage _currentStage;
        private Generator generator = new Generator();

        private GameStateManager gameStateManager;
        private static GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        /// <summary>
        /// Camera for following the player around
        /// </summary>
        private Camera2D camera;

        bool end = false;



        private bool jumping = false;

        //

        Texture2D pixel;
        GameObject Floor;
        Enemy enemy;

        /// <summary>
        /// Holds GameObject waterDispenser
        /// </summary>
        GameObject waterDispenser;

        /// <summary>
        /// Holds texture of waterDispenser
        /// </summary>
        Texture2D waterDispenserTexture;

        /// <summary>
        /// Holds song for when waterDispenser lands
        /// </summary>
        Song waterDispenserLandingSound;

        /// <summary>
        /// Hold wether waterDispenser landed or not
        /// </summary>
        bool waterDispenserLanded;

        /// <summary>
        /// Set waterDispenser dropspeed
        /// </summary>
        float dropSpeed = 5f;

        Song slurp;
        

        AActor player;

        Dictionary<string, Dictionary<string, SpriteAnimation>> spriteAnimations;

        SpriteBatch spriteBatch;
        Texture2D map;


        Point mapSize;
        Matrix matrix;

        /// <summary>
        /// Hold wether boss is defeated or not -> waterDispenser spawns when defeated
        /// </summary>
        bool bossDefeated = false; // TEMP! MUST BE FALSE

        /// <summary>
        /// Hold wether player has fnished the staged (interacted with waterDispenser)
        /// </summary>
        bool finished = false;

        /// <summary>
        /// Properties for fading when finished
        /// </summary>
        int aplhaValue = 1;
        int fadeIncrement = 1;
        Texture2D fader;

        private Vector2 startPosition; // Holds player starting position 

        public StageGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            spriteBatch = new SpriteBatch(graphics);
            spriteAnimations = new Dictionary<string, Dictionary<string, SpriteAnimation>>();

            _stages = new Dictionary<string, Stage>();
            _stages.Add("1", new Stage());
            _stages.Add("2", new Stage());

        }

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            pixel = new Texture2D(graphics, 1, 1);
            Color[] colourData = new Color[1];
            colourData[0] = Color.Red; //The Colour of the rectangle
            pixel.SetData<Color>(colourData);


            if (args.Length > 0)
            {
                //_currentStage = _stages[args[0].ToString()];
                player = (Player)args[1];
                _currentStage = new Stage();
                _currentStage.loadContent((int)args[0]);
            }
            else
            {
                player = new Player();
                _currentStage = new Stage();
                _currentStage.loadContent(1);
                
            }

            loadCameraSettings();

            Floor = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 270));

            waterDispenserTexture = content.Load<Texture2D>("Images\\stages\\waterdispenser"); // Load waterDispenser texture
            waterDispenser = new GameObject(waterDispenserTexture, new Vector2(400, -waterDispenserTexture.Height)); // Initialize new GameObject for waterDispenser
            waterDispenserLandingSound = content.Load<Song>("audio/plop"); // Load landing sound 
            slurp = content.Load<Song>("audio/slurp");

            fader = content.Load<Texture2D>("healthbar");
            enemy = new Enemy();
            enemy.health = 100;
            player.attack = 49;
            player.health = 100;

            //   enemy = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 260));

            //player.position = new Vector2(screenSize.X / 20, screenSize.Y / 1.4f); // Set player starting position
            player.Position = new Vector2(255, 260); // Set player starting position
            enemy.Position = new Vector2(125, Floor.Position.Y); // Set player starting position

            //    cameraLocation = new Point(player.Position.ToPoint().X, player.Position.ToPoint().Y);

            player.actionStateMachine.Change("stand");
            enemy.actionStateMachine.Change("stand");
            //Debug.WriteLine(player);

            spriteAnimations.Add("enemy", new Dictionary<string, SpriteAnimation>());
            spriteAnimations["enemy"].Add("stand", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/stand"), 3, 10));
            spriteAnimations["enemy"].Add("attack", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/attack"), 1, 15));
            spriteAnimations["enemy"].Add("moveLeft", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 1, 20));
            spriteAnimations["enemy"]["moveLeft"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });
            spriteAnimations["enemy"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1 });
            spriteAnimations["enemy"]["attack"].setSpriteSequence(new List<int>() { 0 });

            enemy.spriteAnimations = spriteAnimations["enemy"];

            spriteAnimations.Add("player", new Dictionary<string, SpriteAnimation>());
            spriteAnimations["player"].Add("stand", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/stand"), 3, 10));
            spriteAnimations["player"].Add("move", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 5, 20));
            spriteAnimations["player"].Add("jump", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/jump"), 2, 3));
            spriteAnimations["player"].Add("attack", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/attack"), 1, 15));
            spriteAnimations["player"].Add("crouch", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/crouch"), 1, 20));
            spriteAnimations["player"].Add("hurt", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/hurt"), 1, 10));
            spriteAnimations["player"].Add("moveLeft", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 1, 20));
            spriteAnimations["player"].Add("moveRight", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 1, 20));

            spriteAnimations["player"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1 });
            spriteAnimations["player"]["move"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });
            spriteAnimations["player"]["jump"].setSpriteSequence(new List<int>() { 0, 1 });
            spriteAnimations["player"]["attack"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["crouch"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["hurt"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["moveLeft"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });
            spriteAnimations["player"]["moveRight"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });


            player.spriteAnimations = spriteAnimations["player"];

            // TEMP - debugging purpose
            player.load();
            enemy.load();
            data = new Color[100 * 10];
            //progressBar();

        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.B))
            {
                if (boundingBox) boundingBox = false;
                else boundingBox = true;
            }
            if (state.IsKeyDown(Keys.Escape)) gameStateManager.Change("worldmap");
            if (state.IsKeyDown(Keys.Space))
            {
                jumping = true;
            }
            if (state.IsKeyUp(Keys.Space))
            {
                jumping = false;
                
            }
            foreach (var map in _currentStage.GameObjects)
            {
                if (state.IsKeyUp(Keys.Left))
                {
                    //map.Position = new Vector2(map.Position.X - 2f, map.Position.Y);

                }
                if (state.IsKeyUp(Keys.Right))
                {
                    //map.Position = new Vector2(map.Position.X + 2f, map.Position.Y);
                }
            }
            player.HandleInput(state);
        }


        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            player.Update(gameTime);
            enemy.Update(gameTime);
            checkCollisions();
            // Check if player is finished
            if (finished)
            {
                // Fade animation and leave stage
                fadeAndLeave();
            }

            // Check if boss is defeated for waterDispenser purposes
            if (bossDefeated)
            {
                // Land waterDispenser
                if (waterDispenser.Position.Y + waterDispenser.Size.Y < Floor.Position.Y)
                { 
                    waterDispenser.Position = new Vector2(waterDispenser.Position.X, waterDispenser.Position.Y + dropSpeed);
                }
                // Play sound on landing
                else if (waterDispenser.Position.Y + waterDispenser.Size.Y == Floor.Position.Y)
                {
                    if(!waterDispenserLanded)
                    {
                        MediaPlayer.Play(waterDispenserLandingSound);
                        MediaPlayer.IsRepeating = false;
                    }
                    waterDispenserLanded = true;
                }
            }
            if (_currentStage.enemies != null)
            {
                _currentStage.spawnEnemies(player.Position);
                _currentStage.checkHealth();
                _currentStage.moveEnemies(player.Position);
                _currentStage.checkInRange(player.Position);
                _currentStage.checkProgress();
            }
            if (_currentStage.killedEnemies >= _currentStage.totalEnemies && end == false)
            {
                _currentStage.enemies.Clear();
                spawnBoss();
                end = true;

                
            }
            //bossHealth(); 


            checkWaterCollision();
        }

        bool boundingBox = false;
        public void Draw(GameTime gameTime)
        {
            if (_currentStage.progressBar != null)
            {
                //cam.Pos = new Vector2(500.0f, 200.0f);
                // Begin drawing and disable AA for pixally art
                //spriteBatch.Begin(SpriteSortMode.Deferred,
                //    BlendState.AlphaBlend,
                //    SamplerState.PointClamp,
                //    null, null, null, matrix);
                // Begin drawing and disable AA for pixally art
                spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                null, null, null, camera.Transform);
                foreach (GameObject bgitem in _currentStage.bg)
                {
                    bgitem.Draw(spriteBatch, gameTime);
                }
                //spriteBatch.Draw(map, map.Bounds, Color.White);
                //spriteBatch.Draw(_currentStage.stageBackground, _currentStage.stageBackground.Bounds, Color.White);

                //spriteBatch.Draw(rect, coor, Color.White);
                spriteBatch.Draw(_currentStage.progressBar, new Rectangle((int)player.Position.X - 40, 30, _currentStage.progressBar.Width / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Red);


                //Draw the box around the health bar
                spriteBatch.Draw(_currentStage.progressBar, new Rectangle((int)player.Position.X - 40, 30, _currentStage.progressBar.Width / 2, 44), new Rectangle(0, 0, _currentStage.progressBar.Width / 2, 44), Color.White);
                //spriteBatch.Draw(_currentStage.progressBar, new Rectangle(20,30, _currentStage.progressBar.Width, 44), new Rectangle(0, 45, _currentStage.progressBar.Width, 44), Color.Gray);
                if (_currentStage.killedEnemies < _currentStage.totalEnemies)
                {
                    spriteBatch.Draw((_currentStage.progressBar), new Rectangle((int)player.Position.X - 40, 30, 0 + (int)(_currentStage.killedEnemies) * 5 / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Orange);
                }
                if (_currentStage.killedEnemies >= _currentStage.totalEnemies)
                {
                    spriteBatch.Draw((_currentStage.progressBar), new Rectangle((int)player.Position.X - 40, 30, 0 + (int)(_currentStage.killedEnemies) * 5 / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Green);
                }
                spriteBatch.Draw(player.healthTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y - 100, player.healthTexture.Width / 4, 10), new Rectangle(0, 45, player.healthTexture.Width / 4, 10), Color.Red);
                spriteBatch.Draw(player.healthTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y - 100, player.healthTexture.Width / 4, 10), new Rectangle(0, 0, player.healthTexture.Width / 4, 10), Color.White);
                spriteBatch.Draw((player.healthTexture), new Rectangle((int)player.Position.X, (int)player.Position.Y - 100, 0 + (int)(player.health) + 20, 10), new Rectangle(0, 45, player.healthTexture.Width / 2, 10), Color.Green);

                foreach (AActor enemy in _currentStage.enemies)
                {
                    spriteBatch.Draw(enemy.healthTexture, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, enemy.healthTexture.Width / 4, 10), new Rectangle(0, 45, enemy.healthTexture.Width / 4, 10), Color.Red);
                    spriteBatch.Draw(enemy.healthTexture, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, enemy.healthTexture.Width / 4, 10), new Rectangle(0, 0, enemy.healthTexture.Width / 4, 10), Color.White);
                    spriteBatch.Draw((enemy.healthTexture), new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, 0 + (int)(enemy.health) + 20, 10), new Rectangle(0, 45, enemy.healthTexture.Width / 2, 10), Color.Green);
                }

                player.Draw(spriteBatch, gameTime);
                //enemy.Draw(spriteBatch);
                if (boundingBox)
                {
                    spriteBatch.Draw(pixel, enemy.BoundingBox, Color.White);
                    spriteBatch.Draw(pixel, player.BoundingBox, Color.White);
                    spriteBatch.Draw(pixel, Floor.BoundingBox, Color.White);
                    spriteBatch.Draw(pixel, waterDispenser.BoundingBox, Color.White);
                }

                foreach (GameObject obc in _currentStage.GameObjects)
                {
                    obc.Draw(spriteBatch, gameTime);
                }
                if (_currentStage.enemies.Count > 0)
                {
                    foreach (AActor enemy in _currentStage.enemies)
                    {
                        enemy.Draw(spriteBatch, gameTime);
                    }
                }

                if (bossDefeated)
                {
                    waterDispenser.Draw(spriteBatch, gameTime);
                }

                if (finished)
                {
                    spriteBatch.Draw(fader, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(0, 0, 0, MathHelper.Clamp(aplhaValue, 0, 255)));
                }

                spriteBatch.End();

            }
        }

        public void Leaving()
        {
            spriteAnimations.Clear();
            content.Unload();
        }


        private void checkCollisions()
        {
            checkFloorCollisions();
            checkEnemyCollisions();
            checkGameObjColl();
            checkWaterCollision();
        }

        /// <summary>
        /// Check wether player has collision with waterDispenser
        /// If is -> stage is finished
        /// </summary>
        private void checkWaterCollision()
        {
            if(player.BoundingBox.Intersects(waterDispenser.BoundingBox))
            {
                finished = true;
            }
        }

        /// <summary>
        /// Fade animation for fading out of stage
        /// </summary>
        private void fadeAndLeave()
        {
            aplhaValue += fadeIncrement;

            if(aplhaValue == 100)
            {
                MediaPlayer.Play(slurp);
                MediaPlayer.IsRepeating = false;
            }

            // Change to worldmap when fading is max
            if(aplhaValue >= 255)
            {
                gameStateManager.Change("worldmap");
            }
        }


        private void checkFloorCollisions()
        {
            /*
            if (player.BoundingBox.Intersects(Floor.BoundingBox))
            {
                Rectangle overlap = Rectangle.Intersect(player.BoundingBox, Floor.BoundingBox);
                Debug.WriteLine(overlap);
                player.Position = new Vector2(player.Position.X, Floor.Position.Y - overlap.Height);
                Debug.WriteLine("Floor collision");
            }


            if (enemy.BoundingBox.Intersects(Floor.BoundingBox))
            {
                Rectangle overlap = Rectangle.Intersect(enemy.BoundingBox, Floor.BoundingBox);
                Debug.WriteLine(overlap);
                enemy.Position = new Vector2(enemy.Position.X, Floor.Position.Y - overlap.Height);
                Debug.WriteLine("Floor collision");
            }
            */

        }
        /// <summary>
        /// methode om the checken of de player en enemies op de game objects bewegen
        /// </summary>

        private void checkGameObjColl()
        {
            //float y = player.Position.Y;
            foreach (GameObject obj in _currentStage.GameObjects)
            {
                if (player.BoundingBox.Intersects(obj.BoundingBox))
                foreach (AActor enemy in _currentStage.enemies)
                {
                    if (enemy.BoundingBox.Intersects(obj.BoundingBox))
                    {
                        //Rectangle overlap = Rectangle.Intersect(enemy.BoundingBox, obj.BoundingBox);
                        enemy.Position = new Vector2(enemy.Position.X, obj.Position.Y);
                    }
                }

                
            }
        }

        /// <summary>
        /// methode om te checken of de enemies met de player collision hebben.
        /// </summary>
        private void checkEnemyCollisions()
        {
            foreach (AActor enemy in _currentStage.enemies)
            {
                if (player.BoundingBox.Intersects(enemy.BoundingBox))
                {
                        enemy.actionStateMachine.Change("attack");
                        //enemy.coolDown();
                    if (player.actionStateMachine.Current is AttackAction)
                    {
                        enemy.health = (enemy.health + (enemy.defense * (int)0.5) + 13) - player.attack;// -= player.attack;                           
                        if (player.health > 100)
                        {
                            player.health = 100;
                        }
                        else if (player.health < 100)
                        {
                            player.health += enemy.defense / 2;
                        }
                    }
                    else if (enemy.actionStateMachine.Current is AttackAction)
                    {
                        if (player.actionStateMachine.Current is JumpAction) { }
                        else if (player.actionStateMachine.Current is AttackAction) { enemy.health -= player.attack; }
                        else
                        {
                            //Debug.WriteLine("Ouch!");
                            //player hurt action moet hier komen
                            player.health -= enemy.attack;
                        }
                    }
                }


                else
                {
                    enemy.actionStateMachine.Change("stand");
                }
            }

        }



        //fields voor de progress bar
        Texture2D rect;
        Color[] data;
        Vector2 coor;
        
        /// <summary>
        /// WORK IN PROGRESS!
        /// Methode om de progress van de game aan te geven. 
        /// voor elke enemy die gedood is moet de progressbar incrementen, tot het maximale enemies van de stage.
        /// 
        /// </summary>
        public void progressBar()
        {
            //WERKT NOG NIET!!!
            rect =  new Texture2D(graphics, 100, 10);
            coor = new Vector2(250, 10);
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            rect.SetData(data);
        }


        
        private void loadCameraSettings()
        {
            camera = new Camera2D();
            camera.Focus = player;
            camera.Scale = 1;

            map = content.Load<Texture2D>("Images/stages/stage_1/map");
            mapSize = new Point(500, 350);
            //Point screenSize = graphics.Viewport.Bounds.Size;
            //var scaleX = (float)screenSize.X / mapSize.X;
            //var scaleY = (float)screenSize.Y / mapSize.Y;
            //matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            
        }

        public void spawnBoss()
        {
            Vector2 spawnloc = new Vector2(player.Position.X + 500, player.Position.Y);
            _currentStage.spawnBoss(3, spawnloc);        
        }

        public void bossHealth()
        {
            for (int i = 0; i < _currentStage.enemies.Count; i++)
            {
                Boss b = (Boss)_currentStage.enemies[i];
                if (b.health <= 0)
                {
                    bossDefeated = true;
                }
            }
            
        }

    }
}
