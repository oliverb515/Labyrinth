// Animation.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final
{
    class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        ContentManager content;


        // The scale used to display the sprite strip
        float scale;


        // The time since we last updated the frame
        int elapsedTime;


        // The time we display a frame until the next one
        int frameTime;


        // The number of frames that the animation contains
        int frameCount;


        // The index of the current frame we are displaying
        int currentFrame;


        // The color of the frame we will be displaying
        Color color;


        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();


        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();


        // Width of a given frame
        public int FrameWidth;


        // Height of a given frame
        public int FrameHeight;


        // The state of the Animation
        public bool Active;


        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;


        // Width of a given frame
        public Vector2 Position;

        //The name of the .png spritesheet to use for this animation
        String fileName;

        Player player;


        public void Initialize(Game game, Vector2 position, int frameCount,
int frametime, Color color, float scale, bool looping, Player player, String fn)
        {
            content = game.Content;
            this.player = player;
            // Keep a local copy of the values passed in
            this.color = color;
            //this.FrameWidth = frameWidth;
            //this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            fileName = fn;


            Looping = looping;
            Position = position;
            spriteStrip = content.Load<Texture2D>(fileName);

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;


            // Set the Animation to active by default
            Active = true;
        }
        public static Texture2D getTexture(String textureName, Game game)
        {
            return game.Content.Load<Texture2D>(textureName);
        }

        public void stop()
        {
            Active = false;
        }

        //#region Update
        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false)
                return;


            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;


                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }


                // Reset the elapsed time to zero
                elapsedTime = 0;
            }


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame%3 * 64, 128*(currentFrame/3), 64, 128);


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)Position.X - (int)(64 * scale) / 2,
            (int)Position.Y - (int)(128 * scale) / 2,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
        }
        //#endregion


        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, new Rectangle((int)player.Position.X, (int)player.Position.Y, 64, 128), sourceRect, Color.White);
            }
        }

    }
}
