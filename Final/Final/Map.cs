using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Final
{
    class Map : DrawableGameComponent
    {
        public World world;
        public Block[,] blocks;
        public Player player;
        int[,] intBlocks;
        String colFileName;
        String bgFileName;
        Texture2D bg;
        Texture2D overlay;
        protected Game1 game;
        protected SpriteBatch sb;
        protected KeyboardState previousKeyboardState;
        protected KeyboardState currentKeyboardState;
        private bool hasOverlay;

//      The position of the player upon entering the map
        Vector2 entryLocation;

        public Map(Game1 game) : base(game) { }

        public Map(Game1 game, World world, Player player, int x, int y, bool hasOverlay) : base(game)
        {
            this.game = game;
            this.world = world;
            this.player = player;
            entryLocation = player.Position;
            colFileName = world.getCollisionMapFilename(x, y);
            bgFileName = world.getBackgroundFilename(x, y);
            this.DrawOrder = 0;
            game.Components.Add(this);
            this.hasOverlay = hasOverlay;
            try
            {
                loadMap();
            }
            catch (Exception e)
            {
                System.Environment.Exit(0);
            }
            previousKeyboardState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;

            currentKeyboardState = Keyboard.GetState();

//          update the player's position based on it's velocity
            player.playerState.position += (player.playerState.velocity * elapsed);

//          applies acceleration due to gravity
            if (player.playerState.velocity.Y < 1.0f)
                player.playerState.velocity.Y += 0.001f * elapsed;

            HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
                previousKeyboardState = currentKeyboardState;
                sb.Begin();
                sb.Draw(bg, new Rectangle(0, 0, 1280, 720), Color.White);
                player.Draw(sb);
                if (hasOverlay) sb.Draw(overlay, new Rectangle(0, 0, 1280, 720), Color.White);
                sb.End();
        }

        public void HandleInput()
        {

            if (player.playerState.onGround)
            {
                player.playerState.position.Y++;
            }
            player.HandleInput(null);
        }

//      This method is called to load the 2d array of passable/impassable blocks (stored as a .txt file) and the .png file used for the background
        public void loadMap()
        {
            blocks = new Block[40, 23];
            intBlocks = new int[40, 23];
            String path = "res/colMaps/" + colFileName + ".txt";
            using (StreamReader sr = new StreamReader(path))
            {
                for (int y = 0; y < 23; y++)
                {
                    String line = sr.ReadLine();
                    String[] ints = line.Split(' ');
                    for (int x = 0; x < 40; x++)
                    {
                        intBlocks[x,y] = int.Parse(ints[x]);
                        blocks[x, y] = new Block(intBlocks[x, y], x, y);
                    }
                }
            }
            bg = world.game.Content.Load<Texture2D>("maps/" + bgFileName);
            if (hasOverlay)
            {
                try
                {
                    overlay = world.game.Content.Load<Texture2D>("maps/" + bgFileName + "Overlay");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Overlay not found!");
                }
            }
        }
    }
}