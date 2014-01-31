using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final
{
    class BellroomMap : EventMap
    {
        const int X_INDEX = 8;
        const int Y_INDEX = 1;

        SoundEffect bellE;
        SoundEffect bellB;
        SoundEffect bellC;
        SoundEffect bellG;

        SoundEffect forestSong;

        SpriteFont font;

        Boolean displayText;

        int degree;

        public BellroomMap(Game1 game, World world, Player player)
                : base(game, world, player, X_INDEX, Y_INDEX, false)
        {
            degree = 0;
            displayText = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (player.Position.X < 700) displayText = false;
            if (currentKeyboardState.IsKeyDown(Keys.A) && !previousKeyboardState.IsKeyDown(Keys.A)) {
                if (player.Position.X >= 840 && player.Position.X < 1000)
                {
                    if (degree == 3) degree = 4;
                    bellG.Play();
                }
                if (player.Position.X >= 650 && player.Position.X <= 800) {
                    bellE.Play();
                    if (degree >=0) degree = 1;
                }
                if (player.Position.X >= 455 && player.Position.X <= 615) 
                {
                    if (degree == 2) degree = 3;
                    bellC.Play();
                }
                if (player.Position.X >= 260 && player.Position.X <= 420)
                {
                    if (degree == 1) degree = 2;
                    bellB.Play();
                }
            }

            if (degree == 4)
            {
                forestSong.Play();
                degree = -1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            sb.Begin();
            if (displayText) sb.DrawString(font, "'a' - Chime a bell", new Vector2(1050, 425), Color.FromNonPremultiplied(228, 228, 228, 256 - (int)Math.Abs(player.Position.X - 1000)));
            sb.End();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            bellE = Game.Content.Load<SoundEffect>("sound/bellE");
            bellB = Game.Content.Load<SoundEffect>("sound/bellB");
            bellC = Game.Content.Load<SoundEffect>("sound/bellC");
            bellG = Game.Content.Load<SoundEffect>("sound/bellG");
            forestSong = Game.Content.Load<SoundEffect>("sound/Forest Song");
            font = Game.Content.Load<SpriteFont>("Fonts/plain");
        }
    }
}
