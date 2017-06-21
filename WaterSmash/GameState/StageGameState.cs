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
            } else
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
            Debug.WriteLine(player);

            spriteAnimations.Add("enemy", new Dictionary<string, SpriteAnimation>());
            spriteAnimations["enemy"].Add("stand", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/stand"), 3, 10));
            spriteAnimations["enemy"].Add("attack", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/attack"), 1, 15));
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

            spriteAnimations["player"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1});
            spriteAnimations["player"]["move"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3} );
            spriteAnimations["player"]["jump"].setSpriteSequence(new List<int>() { 0, 1 });
            spriteAnimations["player"]["attack"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["crouch"].setSpriteSequence(new List<int>() { 0 });
            spriteAnimations["player"]["hurt"].setSpriteSequence(new List<int>() { 0 });

            player.spriteAnimations = spriteAnimations["player"];

            // TEMP - debugging purpose
            player.load();
            enemy.load();
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.B))
            {
                if (boundingBox) boundingBox = false;
                else boundingBox = true;
            }
            if (state.IsKeyDown(Keys.Escape)) gameStateManager.Change("worldmap");
            player.HandleInput(state);
        }


        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            enemy.Update(gameTime);
            checkCollisions();
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

            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            if (boundingBox)
            {
                spriteBatch.Draw(pixel, enemy.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, player.BoundingBox, Color.White);
                spriteBatch.Draw(pixel, Floor.BoundingBox, Color.White);
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

        }

        private void checkFloorCollisions()
        {
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
            
        }

        private void checkEnemyCollisions()
        {

            if (player.BoundingBox.Intersects(enemy.BoundingBox))
            {
                if (player.actionStateMachine.Current is AttackAction)
                {
                    enemy.health -= player.attack;
                }
                else if (enemy.actionStateMachine.Current is AttackAction)
                {
                    if (player.actionStateMachine.Current is JumpAction) { }
                    else if (player.actionStateMachine.Current is AttackAction) { }
                    else
                    {
                        Debug.WriteLine("Ouch!");
                        player.actionStateMachine.Change("jump");
                    }
                }

            }
        }



    }
}
