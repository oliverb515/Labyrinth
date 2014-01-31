using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Final
{
    class PostCabMap : EventMap
    {
        const int X_INDEX = 4;
        const int Y_INDEX = 1;
        Vector2 cabThingPos;
        bool riding = true;
        bool lowering = false;
        Texture2D cabthing;
        Texture2D leverSheet;
        SpriteFont font;
        Animation leverAnimation;

        public PostCabMap(Game1 game, World world, Player player)
            : base(game, world, player, X_INDEX, Y_INDEX, false)
        {
            cabThingPos = new Vector2(860, 388);
            this.player.playerState.position.Y = 569;
            this.player.playerState.velocity = Vector2.Zero;
            leverAnimation = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (leverAnimation == null)
            {
                leverAnimation = new Animation();
                leverAnimation.Initialize(game, new Vector2(975, 643), 11, 50, 1, false, player, leverSheet, 1, 11, 77, 77);
            }
            base.Update(gameTime);
            if (cabThingPos.Y < 330 && riding) { riding = false; lowering = true; }
            if (riding)
            {
                cabThingPos.Y -= 0.1f * gameTime.ElapsedGameTime.Milliseconds;
                player.playerState.position.Y = cabThingPos.Y + 194;
                player.Velocity = Vector2.Zero;
            }
            else if (lowering)
            {
                leverAnimation.Update(gameTime);
                player.playerState.position.Y = 511;
                if (!leverAnimation.Active) lowering = false;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            sb.Begin();
            sb.Draw(cabthing, cabThingPos, Color.White);
            if (!riding) leverAnimation.Draw(sb, new Vector2(1025, 570));
            sb.End();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            cabthing = Game.Content.Load<Texture2D>("textures/cabthing");
            font = Game.Content.Load<SpriteFont>("Fonts/plain");
            leverSheet = Game.Content.Load<Texture2D>("textures/leverSheet");
        }
    }
}
