
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Final
{
	class GameplayScreen : GameScreen
	{
//      Descriptive names, I know
        Animation animation;
        public Game game;
        GameState state;
        public Player player;
        World world;
        Lantern lantern;
        public Boolean paused;
        float intensity = 5.0f;

        //the map to be updated and drawn
        Map activeMap;
        EffectParameter lightParam;

        List<Entity> entities;
        //List<Lantern> lanterns;

        public GameplayScreen(Game game)
        {
            this.game = game;
            world = new World((Game1)(game), this);
            animation = new Animation();
            player = new Player(new PlayerState(new Vector2(400, 400), new Vector2()));
            entities = new List<Entity>();
            lantern = new Lantern(player.playerState.position, player, true);
        }

        public GameplayScreen(Game game, GameState state)
        {
            this.game = game;
            this.state = state;
            world = new World((Game1)(game), this, state.mapX, state.mapY);
            animation = new Animation();
            player = new Player(new PlayerState(new Vector2(400, 400), new Vector2()));
            entities = new List<Entity>();
            lantern = new Lantern(player.playerState.position, player, true);
        }

        Effect lighting;
        Texture2D black;
        SpriteFont font;
        public override void LoadContent()
        {
            GameState state = ((Game1)(game)).state;
            player.Initialize(Vector2.Zero, game);
            player.pickUpLantern(lantern);

            world.loadArrays();

            activeMap = world.getMap(0, 0);

            loadEntities();
            lighting = game.Content.Load<Effect>("lighting");
            black = game.Content.Load<Texture2D>("black");
            font = game.Content.Load<SpriteFont>("Fonts/plain");

            base.LoadContent();
        }

//      This method finds each impassable block in the map's block array and stores them in a list to check for collisions each frame.
        public void loadEntities()
        {
            entities.Clear();
            for (int y = 0; y < 22; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                    if (activeMap.blocks[x, y].value == 1)
                    {
                        entities.Add(activeMap.blocks[x, y]);
                    }
                }
            }
        }

        int framecount = 0;
        int frametime = 0;
        float fps = 0;
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;

//          This code just gets the current FPS (I read somewhere online that this method ISN'T how you're supposed to calculate FPS, so maybe I should re-write this
            frametime += gameTime.ElapsedGameTime.Milliseconds;
            framecount++;
            if (frametime > 1000)
            {
                fps = framecount;
                frametime = 1;
                framecount = 1;
            }

//          Updates the map if the player exits to the left, right, top, or bottom
            #region Player Off-Screen Handling
            try
            {
                if (player.getRect().Left > 1280)
                {
                    player.playerState.position.X = 0;
                    activeMap.Dispose();
                    activeMap = world.getMap(1, 0);
                    loadEntities();
                }

                if (player.getRect().Right < 0)
                {
                    player.playerState.position.X = 1280 - 64;
                    activeMap.Dispose();
                    activeMap = world.getMap(-1, 0);
                    loadEntities();
                }

                if (player.getRect().Bottom < 0)
                {
                    player.playerState.position.Y = 720 - 128;
                    activeMap.Dispose();
                    activeMap = world.getMap(0, -1);
                    loadEntities();
                }

                if (player.getRect().Top > 720)
                {
                    player.playerState.position.Y = 0;
                    activeMap.Dispose();
                    activeMap = world.getMap(0, 1);
                    loadEntities();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Environment.Exit(0);
            }
            #endregion Player Off-Screen Handling

            player.Update(gameTime);
            lantern.update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        Vector2 LightSrc;
        public override void Draw(GameTime gameTime)
        {

//          Here, we're getting the position of the lantern and passing it into the pixel shader to apply the lighting effect
            #region Lighting
            LightSrc = new Vector2((lantern.pos.X + 32) / 1280.0f, ((lantern.pos.Y + 64) / 720.0f));
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lightParam = lighting.Parameters["lightPos"];
            lighting.Parameters["intensity"].SetValue(intensity);
            lightParam.SetValue(LightSrc);
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, lighting);
            ScreenManager.SpriteBatch.Draw(black, new Rectangle(0, 0, 1280, 720), Color.White);
            ScreenManager.SpriteBatch.End();
            #endregion Lighting


//          Just draws useful stuff to the screen
            #region Debug Drawing
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, "FPS: " + fps + "\nOn Ground: " + player.playerState.onGround +
            "\nWorld X: " + world.currentX + "\nWorld Y: " + world.currentY, new Vector2(20.0f, 20.0f), Color.White);
            ScreenManager.SpriteBatch.End();
            #endregion Debug Drawing
        }

        public override void HandleInput(InputState input)
        {
            if (player.currentKeyboardState.IsKeyDown(Keys.P) && !player.previousKeyboardState.IsKeyDown(Keys.P))
            {
                paused = !paused;
                ScreenManager.AddScreen(new PauseScreen(this));
            }
            HandleCollisions();
        }

        /*  Right now, collisions are done with a grid of tiles that are either passable or impassable. I set this system up
         *  in the early stages of development and have since then decided to eventually re-work this to make it more efficient
         *  and allow for more organic collision shapes. It's not worth looking over this and it's a miracle it actually works.
         */
        #region Collision Handling


        public bool horizontallyAligned(Rectangle a, Rectangle b)
        {
            return (a.Top < b.Top && a.Bottom > b.Top) || (a.Top < b.Bottom && a.Bottom > b.Bottom);
        }

        public bool verticallyAligned(Rectangle a, Rectangle b)
        {
            return (a.Left < b.Right && a.Right > b.Right) || (a.Left < b.Left && a.Right > b.Left);
        }

        public bool separatedByVerticalAxis(Rectangle a, Rectangle b)
        {
            if (a.Right <= b.Left || a.Left >= b.Right) return true;
            return false;
        }

        public bool separatedByHorizontalAxis(Rectangle a, Rectangle b)
        {
            if (a.Top >= b.Bottom || a.Bottom <= b.Top) return true;
            return false;
        }

        public bool colliding(Rectangle a, Rectangle b)
        {
            if (separatedByVerticalAxis(a, b) || separatedByHorizontalAxis(a, b)) return false;
            return true;
        }

//      
        public Vector2 minimumTranslation(Rectangle a, Rectangle b)
        {
            Vector2 mtd = new Vector2();

            float left = (b.Left - a.Right);
            float right = (b.Right - a.Left);
            float top = (b.Bottom - a.Top);
            float bottom = (b.Top - a.Bottom);

            if (Math.Abs(left) < right)
                mtd.X = left;
            else mtd.X = right;

            if (Math.Abs(top) < bottom)
                mtd.Y = top;
            else mtd.Y = bottom;

            if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
                mtd.Y = 0;
            else mtd.X = 0;

            return mtd;
        }

        // returns true if the first rectangle is falling on top of the second rectangle
        public bool fallingCollision(Rectangle a, Rectangle b)
        {
            return (a.Bottom >= b.Top && a.Bottom <= b.Bottom && a.Bottom <= b.Bottom && verticallyAligned(a, b));
        }

        public void handleVerticalCollisions(Rectangle eRect)
        {
            //falling collision
            if (player.getRect().Bottom >= eRect.Top && player.getRect().Bottom <= eRect.Bottom && player.getRect().Bottom <= eRect.Bottom && (verticallyAligned(player.getRect(), eRect)))
            {
                if (player.playerState.velocity.Y > 0)
                player.playerState.velocity.Y = 0;
                player.playerState.position.Y = eRect.Top - 128;
                player.playerState.onGround = true;
            }

            //rising collision
            if (player.getRect().Top <= eRect.Bottom && player.getRect().Top >= eRect.Top && player.getRect().Bottom >= eRect.Bottom && (verticallyAligned(player.getRect(), eRect)))
            {
                player.playerState.velocity.Y = 0;
                player.playerState.position.Y = eRect.Bottom+1;
            }
        }

        public void handleHorizontalCollisions(Rectangle eRect)
        {
            // PLAYER -> ENTITY collision
            if (player.getRect().Right > eRect.Left && player.getRect().Right < eRect.Right && player.getRect().Left < eRect.Left && horizontallyAligned(player.getRect(), eRect))
            {
                player.playerState.position.X = eRect.Left - 64;
                player.playerState.velocity.X = 0;
            }

            // ENTITY <- PLAYER collision
            if (player.getRect().Left < eRect.Right && player.getRect().Left > eRect.Left && player.getRect().Right > eRect.Right && horizontallyAligned(player.getRect(), eRect))
            {
                player.playerState.position.X = eRect.Right;
                player.playerState.velocity.X = 0;
            }
        }

        public LineSegment[] getDeltaVectors(Rectangle a, Rectangle b)
        {
            LineSegment[] result = new LineSegment[4];

            //top left corner
            result[0] = new LineSegment(new Vector2(a.Left, a.Top), new Vector2(b.Left, b.Top));
            //top right corner
            result[1] = new LineSegment(new Vector2(a.Right, a.Top), new Vector2(b.Right, b.Top));
            //bottom left corner
            result[2] = new LineSegment(new Vector2(a.Left, a.Bottom), new Vector2(b.Left, b.Bottom));
            //bottom right corner
            result[3] = new LineSegment(new Vector2(a.Right, a.Bottom), new Vector2(b.Right, b.Bottom));

            return result;
        }

        public LineSegment[] getSidesOfRectangle(Rectangle r)
        {
            LineSegment[] result = new LineSegment[4];
            result[0] = new LineSegment(new Vector2(r.Left, r.Top), new Vector2(r.Right, r.Top));
            result[1] = new LineSegment(new Vector2(r.Right, r.Top), new Vector2(r.Right, r.Bottom));
            result[2] = new LineSegment(new Vector2(r.Left, r.Bottom), new Vector2(r.Right, r.Bottom));
            result[3] = new LineSegment(new Vector2(r.Left, r.Top), new Vector2(r.Left, r.Bottom));
            return result;
        }



        public void HandleCollisions() 
        {
            player.playerState.onGround = false;
            foreach (Entity e in entities)
            {
                Rectangle eRect = e.getRect();
                //Preliminary Tests: These will return if there is no intersection between the player and the entity
                //if (!player.getRect().Intersects(eRect)) continue;
                if (fallingCollision(new Rectangle((int) (lantern.pos.X), (int) (lantern.pos.Y), 64, 64), eRect)) {
                    lantern.pos.Y = eRect.Top-96;
                    lantern.falling = false;
                }
                if ((player.getRect().Right < eRect.Left || player.getRect().Left > eRect.Right)) continue;
                if ((player.getRect().Top > eRect.Bottom || player.getRect().Bottom < eRect.Top)) continue;

                if (!player.playerState.onGround && separatedByVerticalAxis(player.lastRect, eRect) && separatedByHorizontalAxis(player.lastRect, eRect))
                    player.playerState.position += minimumTranslation(player.getRect(), eRect);
                else if (separatedByVerticalAxis(player.lastRect, eRect)) 
                {
                        handleHorizontalCollisions(eRect);
                        handleVerticalCollisions(eRect);
                }
                else if (separatedByHorizontalAxis(player.lastRect, eRect)) 
                {
                    handleVerticalCollisions(eRect);
                    handleHorizontalCollisions(eRect);
                }
            }
            player.lastRect = player.getRect();
        }

        #endregion Collision Handling

        // A simple helper to draw shadowed text.
        void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            ScreenManager.SpriteBatch.DrawString(font, text,
                new Vector2(position.X + 1, position.Y + 1), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, text, position, color);
        }

        // A simple helper to draw shadowed text.
        void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
        {
            ScreenManager.SpriteBatch.DrawString(font, text,
                new Vector2(position.X + 1, position.Y + 1),
                Color.Black, 0, new Vector2(0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.DrawString(font, text, position,
                color, 0, new Vector2(0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
        }

        private void PauseCurrentGame()
        {
            // TODO: Display pause screen
        }
    }
}