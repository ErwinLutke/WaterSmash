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

        //RICKS CODE:@:@:@:@
        //




        private bool jumping = false;

        //

        Texture2D pixel;
        GameObject Floor;
        Enemy enemy;

        AActor player;

        Dictionary<string, Dictionary<string, SpriteAnimation>> spriteAnimations;

        SpriteBatch spriteBatch;
        Texture2D map;
        Point mapSize;
        Matrix matrix;

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

            map = content.Load<Texture2D>("Images/stages/stage_1/map");
            mapSize = new Point(500, 350);
            Point screenSize = graphics.Viewport.Bounds.Size;
            var scaleX = (float)screenSize.X / mapSize.X;
            var scaleY = (float)screenSize.Y / mapSize.Y;
            matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if (args.Length > 0)
            {
                _currentStage = _stages[args[0].ToString()];
                player = (Player)args[1];
            }
            else
            {
                _currentStage = new Stage();
                player = new Player();
            }


            Floor = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 270));
            enemy = new Enemy();
            enemy.health = 100;
            player.attack = 11;

            //   enemy = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 260));

            //player.position = new Vector2(screenSize.X / 20, screenSize.Y / 1.4f); // Set player starting position
            player.Position = new Vector2(25, Floor.Position.Y); // Set player starting position
            enemy.Position = new Vector2(125, Floor.Position.Y); // Set player starting position

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

            spriteAnimations["player"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1 });
            spriteAnimations["player"]["move"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });
            spriteAnimations["player"]["jump"].setSpriteSequence(new List<int>() { 0, 1 });
            spriteAnimations["player"]["attack"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["crouch"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["hurt"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["moveLeft"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });


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
            player.Update(gameTime);
            enemy.Update(gameTime);
            checkCollisions();
            _currentStage.spawnEnemies();
            _currentStage.checkHealth();
            _currentStage.moveEnemies(player.Position);
            _currentStage.checkInRange(player.Position);
            
        }

        bool boundingBox = false;
        public void Draw(GameTime gameTime)
        {
             // Begin drawing and disable AA for pixally art
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, matrix);

            spriteBatch.Draw(map, map.Bounds, Color.White);
            //spriteBatch.Draw(rect, coor, Color.White);
            spriteBatch.Draw(_currentStage.progressBar, new Rectangle(20, 30, _currentStage.progressBar.Width, 44), new Rectangle(0, 45, _currentStage.progressBar.Width, 44), Color.Red);


            //Draw the box around the health bar
            spriteBatch.Draw(_currentStage.progressBar, new Rectangle(20,30, _currentStage.progressBar.Width, 44), new Rectangle(0, 0, _currentStage.progressBar.Width, 44), Color.White);
            //spriteBatch.Draw(_currentStage.progressBar, new Rectangle(20,30, _currentStage.progressBar.Width, 44), new Rectangle(0, 45, _currentStage.progressBar.Width, 44), Color.Gray);

            spriteBatch.Draw((_currentStage.progressBar), new Rectangle(20,30, 0 +(int)(_currentStage.killedEnemies) *5, 44),new Rectangle(0, 45, _currentStage.progressBar.Width, 44), Color.Green);

            player.Draw(spriteBatch);
            //enemy.Draw(spriteBatch);
            if (boundingBox)
            {
                spriteBatch.Draw(pixel, enemy.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, player.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, Floor.BoundingBox, Color.White);
            }

            foreach (GameObject obc in _currentStage.GameObjects)
            {
                obc.Draw(spriteBatch);
            }
            if (_currentStage.enemies.Count > 0)
            {
                foreach (Enemy enemy in _currentStage.enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }

            spriteBatch.End();


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
                {
                    //Rectangle overlap = Rectangle.Intersect(player.BoundingBox, Floor.BoundingBox);

                    player.Position = new Vector2(player.Position.X, obj.Position.Y);

                }
                else if (!player.BoundingBox.Intersects(obj.BoundingBox))
                {
                    
                }
                else if(player.BoundingBox.Intersects(obj.BoundingBox) && jumping)
                {
                    player.actionStateMachine.Change("jump");
                }
                else
                {
                    Debug.WriteLine("no coll");
                }
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
                            enemy.health += enemy.defense - player.attack;// -= player.attack;
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
    }
}
