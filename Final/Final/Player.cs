using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final
{
    class Player : Entity
    {
        // Animation representing the player
        public Animation[] animations;

        public Rectangle lastRect;

        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;
        GameTime gameTime;

        Lantern lantern;

        bool idle;
        bool facingRight;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position {
            get { return playerState.position; }
            set { playerState.position = value; }
        }
        public Vector2 Velocity
        {
            get { return playerState.velocity; }
            set { playerState.velocity = value; }
        }

        // State of the player
        public enum PlayerAction
        {
            IdleRight,
            IdleLeft,
            WalkingLeft,
            WalkingRight,
            Jumping,
            Falling
        }

        public PlayerAction playerAction;
        public PlayerState playerState;

        public Player(PlayerState ps)
        {
            playerState = ps;
        }

        // Initialize the player
        public void Initialize(Vector2 position, Game game)
        {
            animations = new Animation[4];
            animations[0] = new Animation();
            animations[0].Initialize(game, position, 2, 1000, Color.White, 1f, true, this, "idle right", 1, 2);
            animations[1] = new Animation();
            animations[1].Initialize(game, position, 2, 1000, Color.White, 1f, true, this, "idle left", 1, 2);
            animations[2] = new Animation();
            animations[2].Initialize(game, position, 6, 100, Color.BlueViolet, 1f, true, this, "walking left", 2, 3);
            animations[3] = new Animation();
            animations[3].Initialize(game, position, 6, 100, Color.BlueViolet, 1f, true, this, "walking right", 2, 3);

            playerAction = PlayerAction.IdleRight;

            lastRect = getRect();

            idle = true;
            facingRight = true;

        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
//          update gameTime
            this.gameTime = gameTime;
//          simulates friction
            if (playerState.onGround) playerState.velocity /= (0.073f*(float)(gameTime.ElapsedGameTime.Milliseconds));
//          update animation
            animations[(int)playerAction].Update(gameTime);
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            animations[(int)playerAction].Draw(spriteBatch, Position);
        }

        public override Rectangle getRect()
        {
            return new Rectangle((int)playerState.position.X, (int)playerState.position.Y, 64, 128);
        }

        public void HandleInput(InputState input)
        {

            int elapsed = gameTime.ElapsedGameTime.Milliseconds;

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            idle = true;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (playerState.velocity.X > -0.2f) playerState.velocity.X -= 0.004f*elapsed;
                playerAction = PlayerAction.WalkingLeft;
                idle = false;
                facingRight = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (playerState.velocity.X < 0.2f) playerState.velocity.X += 0.004f*elapsed;
                playerAction = PlayerAction.WalkingRight;
                idle = false;
                facingRight = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (playerState.velocity.Y > -0.5f)
                    playerState.velocity.Y += -0.0003f * elapsed*playerState.numFeathers;
                //playerAction = PlayerAction.Jumping;
                playerState.rising = true;
                idle = false;
            }
            else
            {
                playerState.rising = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                //playerState.velocity.Y += playerMoveSpeed;
                //playerAction = PlayerAction.Falling;
                idle = false;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Z) && !previousKeyboardState.IsKeyDown(Keys.Z))
            {
                lanternToggle();
            }
            if (currentKeyboardState.IsKeyDown(Keys.X) && !previousKeyboardState.IsKeyDown(Keys.X))
            {
                if (playerState.onGround)
                    jump();
            }
            if (idle)
            {
                if (facingRight)
                    playerAction = PlayerAction.IdleRight;
                else
                    playerAction = PlayerAction.IdleLeft;
            }
        }

        public void pickUpLantern(Lantern l)
        {
            lantern = l;
        }
        public void pickUpLantern()
        {
            lantern.pickUp();
        }
        public void dropLantern()
        {
            lantern.drop();
        }
        public void lanternToggle()
        {
            if (lantern.beingCarried) lantern.drop();
            else lantern.pickUp();
        }
        public void jump()
        {
            playerState.velocity.Y = -0.6f;
        }
    }
}
