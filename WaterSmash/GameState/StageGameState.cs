﻿using System;
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

        private Vector2 bossSpawnloc;


        /// <summary>
        /// Camera for following the player around
        /// </summary>
        private Camera2D camera;

        bool end = false;


        private float totalStageTime;
        SpriteFont timeTextFont;

        private bool jumping = false;

        //

        Texture2D pixel;
        GameObject Floor;
        Enemy enemy;
        

        AActor player;

        Dictionary<string, Dictionary<string, SpriteAnimation>> spriteAnimations;

        SpriteBatch spriteBatch;
        Texture2D map;

        GameObject bossDrop;

        Point mapSize;
        Matrix matrix;

        /// <summary>
        /// Hold wether player has finished the staged (interacted with waterDispenser)
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

            timeTextFont = content.Load<SpriteFont>("Font/StageName");
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
            player.actionStateMachine.Change("stand");


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
            totalStageTime += gameTime.ElapsedGameTime.Milliseconds;
            Debug.WriteLine("ms " + totalStageTime);

            camera.Update(gameTime);
            player.Update(gameTime);
       //     enemy.Update(gameTime);

            // Check if player is finished
            if(finished)
            {
                // Fade animation and leave stage
                fadeAndLeave();
            }

            // Check if boss is defeated for waterDispenser purposes
            if (_currentStage.bossDefeated)
            {
                // Land waterDispenser
                if (_currentStage.waterDispenser.Position.Y + _currentStage.waterDispenser.Size.Y < Floor.Position.Y - 10)
                {
                    _currentStage.waterDispenser.Position = new Vector2(_currentStage.waterDispenser.Position.X, _currentStage.waterDispenser.Position.Y + _currentStage.dropSpeed);
                }
                // Play sound on landing
                else if (_currentStage.waterDispenser.Position.Y + _currentStage.waterDispenser.Size.Y == Floor.Position.Y)
                {
                    if(!_currentStage.waterDispenserLanded)
                    {
                        MediaPlayer.Play(_currentStage.waterDispenserLandingSound);
                        MediaPlayer.IsRepeating = false;
                    }
                    _currentStage.waterDispenserLanded = true;
                }
            }
        
            _currentStage.spawnEnemies(player.Position);
            _currentStage.checkHealth(end, bossSpawnloc);
            _currentStage.moveEnemies(player.Position);
            _currentStage.checkInRange(player.Position);
            _currentStage.checkProgress();
           
            if (_currentStage.killedEnemies >= _currentStage.totalEnemies && end == false)
            {
                _currentStage.enemies.Clear();
                spawnBoss();
                end = true;
            }

            checkCollisions();
        }

        bool boundingBox = false;
        public void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            SamplerState.PointClamp,
                            null, null, null, camera.Transform);
            foreach (GameObject bgitem in _currentStage.bg)
            {
                bgitem.Draw(spriteBatch, gameTime);
            }

            //spriteBatch.Draw(rect, coor, Color.White);
            //spriteBatch.Draw(_currentStage.progressBar, new Rectangle((int)player.Position.X - 40, 30, _currentStage.progressBar.Width/2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width/2, 44), Color.Red);
            spriteBatch.Draw(_currentStage.progressBar, new Rectangle((int)camera.Position.X + 150, 30, _currentStage.progressBar.Width / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Red);


            //Draw the box around the health bar
            spriteBatch.Draw(_currentStage.progressBar, new Rectangle((int)camera.Position.X + 150, 30, _currentStage.progressBar.Width/2, 44), new Rectangle(0, 0, _currentStage.progressBar.Width/2, 44), Color.White);
            if (_currentStage.killedEnemies < _currentStage.totalEnemies)
            {
                spriteBatch.Draw((_currentStage.progressBar), new Rectangle((int)camera.Position.X + 150, 30, 0 + (int)(_currentStage.killedEnemies) * 5 / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Orange);
            }
            if (_currentStage.killedEnemies >= _currentStage.totalEnemies)
            {
                spriteBatch.Draw((_currentStage.progressBar), new Rectangle((int)camera.Position.X + 150, 30, 0 + (int)(_currentStage.killedEnemies) * 5 / 2, 44), new Rectangle(0, 45, _currentStage.progressBar.Width / 2, 44), Color.Green);
            }
            spriteBatch.Draw(player.healthTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y -100, player.healthTexture.Width / 4, 10), new Rectangle(0, 45, player.healthTexture.Width / 4, 10), Color.Red);
            spriteBatch.Draw(player.healthTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y - 100, player.healthTexture.Width / 4, 10), new Rectangle(0, 0, player.healthTexture.Width / 4, 10), Color.White);
            spriteBatch.Draw((player.healthTexture), new Rectangle((int)player.Position.X, (int)player.Position.Y - 100, 0 + (int)(player.health) +20, 10), new Rectangle(0, 45, player.healthTexture.Width / 2, 10), Color.Green);

            foreach (Enemy enemy in _currentStage.enemies)
            {
                spriteBatch.Draw(enemy.healthTexture, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, enemy.healthTexture.Width / 4, 10), new Rectangle(0, 45, enemy.healthTexture.Width / 4, 10), Color.Red);
                spriteBatch.Draw(enemy.healthTexture, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, enemy.healthTexture.Width / 4, 10), new Rectangle(0, 0, enemy.healthTexture.Width / 4, 10), Color.White);
                spriteBatch.Draw((enemy.healthTexture), new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y - 100, 0 + (int)(enemy.health) + 20, 10), new Rectangle(0, 45, enemy.healthTexture.Width / 2, 10), Color.Green);
            }

            player.Draw(spriteBatch, gameTime);
            if (boundingBox)
            {
                spriteBatch.Draw(pixel, enemy.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, player.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, Floor.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, _currentStage.waterDispenser.BoundingBox, Color.White);
            }

            foreach (GameObject obc in _currentStage.GameObjects)
            {
                obc.Draw(spriteBatch, gameTime);
            }
            if (_currentStage.enemies.Count > 0)
            {
                foreach (Enemy enemy in _currentStage.enemies)
                {
                    this.enemy = enemy;
                    enemy.Draw(spriteBatch, gameTime);
                }
            }

            if (_currentStage.bossDefeated)
            {
                _currentStage.waterDispenser.Draw(spriteBatch, gameTime);
                //_currentStage.droppedItem.Draw(spriteBatch, gameTime);
            }

            if (finished)
            { 
                spriteBatch.Draw(fader, new Rectangle((int)player.Position.X - 2000, 0, graphics.Viewport.Width + 2000, graphics.Viewport.Height), new Color(0, 0, 0, MathHelper.Clamp(aplhaValue,0,255)));
            }
            spriteBatch.DrawString(timeTextFont, getPlayTime(), new Vector2(camera.Position.X + 230, 40), Color.White);
            spriteBatch.End();


        }

        public void Leaving()
        {
            totalStageTime = 0;
            finished = false;
            end = false;
            aplhaValue = 1;
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
            if(player.BoundingBox.Intersects(_currentStage.waterDispenser.BoundingBox))
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
                MediaPlayer.Play(_currentStage.slurp);
                MediaPlayer.IsRepeating = false;
            }

            // Change to worldmap when fading is max
            if(aplhaValue >= 255)
            {
                gameStateManager.Change("worldmap", getPlayTime());
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
                foreach (Enemy enemy in _currentStage.enemies)
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
            foreach (Enemy enemy in _currentStage.enemies)
            {
                if (player.BoundingBox.Intersects(enemy.BoundingBox))
                {
                    if(enemy.isOnCooldown() == false)
                    {
                        enemy.actionStateMachine.Change("attack");
                        //enemy.coolDown();
                    }
                        if(player.actionStateMachine.Current is AttackAction)
                        {
                            enemy.health = (enemy.health + (enemy.defense *(int)0.5)+13) - player.attack;// -= player.attack;                           
                            if(player.health >100)
                            {
                                player.health = 100;
                            }
                            else if(player.health <100)
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
            bossSpawnloc = new Vector2(player.Position.X + 500, player.Position.Y);
            _currentStage.spawnBoss(3, bossSpawnloc);        
        }


        /// <summary>
        /// Calculates the elapsed time and makes it human readable in mm:ss:ms
        /// </summary>
        private string getPlayTime()
        {
            int milliseconds = (int)((totalStageTime % 1000) / 100);
            int seconds = (int)((totalStageTime / 1000) % 60);
            int minutes = (int)((totalStageTime / (1000 * 60)) % 60);
            return minutes + "'" + seconds + "'" + milliseconds;
        }

    }

}
