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

        #region Fields
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
        // In case the animation is not active, but still needs to be drawn, use this boolean
        bool shouldDraw;
        int rows, cols;

        Player player;
        #endregion

        public void Initialize(Game game, Vector2 position, int frameCount,
int frametime, Color color, float scale, bool looping, Player player, String fn, int r, int c)
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
            rows = r;
            cols = c;
            FrameHeight = 128;
            FrameWidth = 64;


            Looping = looping;
            if (looping == false) shouldDraw = true;
            Position = position;
            spriteStrip = content.Load<Texture2D>(fileName);

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;


            // Set the Animation to active by default
            Active = true;
        }

        public void Initialize(Game game, Vector2 position, int frameCount,
int frametime, float scale, bool looping, Player player, Texture2D image, int r, int c, int frameWidth, int frameHeight)
        {
            content = game.Content;
            this.player = player;
            // Keep a local copy of the values passed in
            this.color = Color.White;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            rows = r;
            cols = c;


            Looping = looping;
            if (looping == false) shouldDraw = true;
            Position = position;
            spriteStrip = image;

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
                    currentFrame--;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                    else
                        currentFrame = 0;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame%cols * FrameWidth, (currentFrame%rows)*FrameHeight, FrameWidth, FrameHeight);
        }
        //#endregion


        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            // Only draw the animation when we are active
            if (Active || shouldDraw)
            {
                spriteBatch.Draw(spriteStrip, new Rectangle((int)pos.X, (int)pos.Y, FrameWidth, FrameHeight), sourceRect, Color.White);
            }
        }

    }
}
