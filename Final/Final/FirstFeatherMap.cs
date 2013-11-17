using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace Final
{
//  Misleading title. This is just the first map where something more than nothing happens.
    class FirstFeatherMap : EventMap
    {
        SoundEffect spooky;
        SoundEffect click;
        const int X_INDEX = 4;
        const int Y_INDEX = 2;
        SpriteFont font;
        bool collected = false;
        Vector2 playerPosition;
        Texture2D cabthing;
        Vector2 cabThingPos;
        bool inCabThing;

        public FirstFeatherMap(Game1 game, World world, Player player, Vector2 playPos) : base(game, world, player, 4, 2)
        {
            playerPosition = playPos;
            inCabThing = false;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds;
            player.playerState.position += (player.playerState.velocity * elapsed);

            //gravity
            if (player.playerState.velocity.Y < 1.0f)
                player.playerState.velocity.Y += 0.001f * elapsed;
            if (!collected && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                spooky.Play();
                collected = true;
                game.state.feathers += 1;
            }
            if (player.Position.X > 860 && player.Position.X < 1039 && player.playerState.onGround && player.currentKeyboardState.IsKeyDown(Keys.A))
            {
                inCabThing = true;
                click.Play();
            }
            if (inCabThing)
            {
                cabThingPos.Y -= 0.1f * gameTime.ElapsedGameTime.Milliseconds;
                player.playerState.position.Y = (cabThingPos.Y+194);
                player.playerState.velocity.X = 0;
            }
            HandleInput();
        }

        public new void HandleInput()
        {
            if (!inCabThing) base.HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            sb.Begin();
            if (!collected) sb.DrawString(font, "Remove the feather from the altar?\n    'a' - Yes\n    's' - No", new Vector2(600, 300), Color.FromNonPremultiplied(128, 128, 128, 256-(int)Math.Abs(600 - player.Position.X)));
            sb.Draw(cabthing, cabThingPos, Color.White);
            sb.End();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spooky = Game.Content.Load<SoundEffect>("sound/spooky");
            font = Game.Content.Load<SpriteFont>("Fonts/plain");
            cabthing = Game.Content.Load<Texture2D>("textures/cabthing");
            click = Game.Content.Load<SoundEffect>("sound/mechanical click");
            cabThingPos = new Vector2(860, 322);
        }
    }
}
