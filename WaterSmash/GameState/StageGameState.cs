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
        private Generator generator;

        private GameStateManager gameStateManager;
        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        //RICKS CODE:@:@:@:@
        //

        public GameState.Camera2d cam = new GameState.Camera2d();



        private bool jumping = false;
        private List<GameObject> GameObjects = new List<GameObject>();//holds all map blocks
        private List<Enemy> enemies = new List<Enemy>();//holds all enemies
        private int killedEnemies = 0;//total killed enemies 

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
            generator = new Generator();

            // TEMP FOR GENERATOR TESTING
            //for(int i = 0; i < 10; i++)
            //{
            //    AEquipable cap = generator.generateEquipable(1);
            //    AEquipable label = generator.generateEquipable(2);
            //}

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
                player = new Player();
            }


            Floor = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 270));
            enemy = new Enemy();
            enemy.health = 100;
            player.attack = 23;

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
            generateMap();
            data = new Color[100 * 10];
            progressBar();

            x = Floor.Position.X;
            y = Floor.Position.Y;

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
            foreach (var map in GameObjects)
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
            spawnEnemies();
            checkHealth();
            moveEnemies();
            checkInRange();
        }

        bool boundingBox = false;
        public void Draw(GameTime gameTime)
        {
            cam.Pos = new Vector2(500.0f, 200.0f);
            // Begin drawing and disable AA for pixally art
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, matrix);

            spriteBatch.Draw(map, map.Bounds, Color.White);

            spriteBatch.Draw(rect, coor, Color.White);

            player.Draw(spriteBatch);
            //enemy.Draw(spriteBatch);
            if (boundingBox)
            {
                spriteBatch.Draw(pixel, enemy.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, player.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, Floor.BoundingBox, Color.White);
            }

            foreach (GameObject obc in GameObjects)
            {
                obc.Draw(spriteBatch);
            }
            if (enemies.Count > 0)
            {
                foreach (Enemy enemy in enemies)
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
        private void checkInRange()
        {
            foreach (var enemy in enemies)
            {

                //if (enemy.Position.X - player.Position.X < enemy.getSightRange())//left
                // if (player.Position.X - enemy.Position.X < enemy.getSightRange())//right
                if (player.Position.X - enemy.Position.X < enemy.getSightRange() && enemy.Position.X - player.Position.X < enemy.getSightRange())
                {
                    enemy.setInRange(true);
                }
                else
                {
                    enemy.setInRange(false);
                }
                //Debug.WriteLine("difference: " + (enemy.Position.X - player.Position.X));
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

        private void checkGameObjColl()
        {
            float y = player.Position.Y;
            foreach (var obj in GameObjects)
            {
                if (player.BoundingBox.Intersects(obj.BoundingBox) && jumping!=true)
                {
                    Rectangle overlap = Rectangle.Intersect(player.BoundingBox, obj.BoundingBox);
                    //Debug.WriteLine(overlap);
                    player.Position = new Vector2(player.Position.X, obj.Position.Y);//- overlap.Height);
                    //Debug.WriteLine("obj collision");
                }
                else if (!player.BoundingBox.Intersects(obj.BoundingBox))
                {
                    //player.Position = new Vector2(player.Position.X, player.Position.Y+1);
                }
                
                if (enemy.BoundingBox.Intersects(obj.BoundingBox))
                {
                    Rectangle overlap = Rectangle.Intersect(enemy.BoundingBox, obj.BoundingBox);
                    //Debug.WriteLine(overlap);
                    enemy.Position = new Vector2(enemy.Position.X, obj.Position.Y - overlap.Height);
                    //Debug.WriteLine("obj collision");
                }
            }
        }

        private void checkEnemyCollisions()
        {
            foreach (var enemy in enemies)
            {
                if (player.BoundingBox.Intersects(enemy.BoundingBox))
                {
                    //enemy.actionStateMachine.Change("attack");
                    if (player.actionStateMachine.Current is AttackAction)
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
                            //player.actionStateMachine.Change("jump");
                            player.health -= enemy.attack;
                        }
                    }

                }
            }
            
        }
        int map_x = 0;
        private void generateMap()
        {
            int maxBlox = 10;//max amount of blocks in game
            while (GameObjects.Count() < maxBlox)
            {

                Vector2 testvec = new Vector2(map_x, Floor.Position.Y - 20);
                if (map_x == 300)
                {
                    testvec = new Vector2(map_x, Floor.Position.Y + 40);
                }
                Texture2D testobj = content.Load<Texture2D>("Images/stages/testobj");
                
                GameObject test = new GameObject(testobj, testvec);
                GameObjects.Add(test);
                map_x = map_x + 100;

            }
        }
        float y; 
        float x;
        private void spawnEnemies()
        {
            int totalEnemies = 100;
            int maxEnemiesInGame = 2;


            while (killedEnemies<totalEnemies && enemies.Count()<maxEnemiesInGame)
            {
                Enemy dummie = new Enemy();
                dummie.attack = 10;
                dummie.defense = 10;
                dummie.health = 100;
                dummie.Position = new Vector2(x, y+10);
                dummie.spriteAnimations = spriteAnimations["enemy"];
                dummie.actionStateMachine.Change("stand");
                //y = y +10;
                x = x +250;
                //Debug.WriteLine("spawning: at y"+ y + ", x" + x );
                enemies.Add(dummie);
               
            }
        }

        private void checkHealth()
        {
            //Debug.WriteLine("viewport Width: " + graphics.Viewport.Width/2);
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].health < 0)
                {
                    //Debug.WriteLine("should die");
                    enemies.RemoveAt(i);
                }
            }
        }

        public void moveEnemies()
        {
            foreach (var enemy in enemies)
            {
                if(player.Position.X < enemy.Position.X && enemy.isInRange())
                {
                    enemy.HandleInput("moveLeft");//must be replaced with enemy.actionStateMachine.Change("action_here");
                    //enemy.actionStateMachine.Change("moveLeft");
                }
                if(player.Position.X > enemy.Position.X && enemy.isInRange())
                {
                    enemy.HandleInput("moveRight");//must be replaced with enemy.actionStateMachine.Change("action_here");

                    //enemy.Position = new Vector2(enemy.Position.X + 1, enemy.Position.Y);//must be replaced with enemy.actionStateMachine.Change("action_here");
                }
            }
        }
        Texture2D rect = new Texture2D(graphics, 100, 10);
        Color[] data;
        Vector2 coor = new Vector2(250,10);
        

        public void progressBar()
        {
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            rect.SetData(data);

            //spriteBatch.Draw(rect, coor, Color.White);
        }
           
    }
}
