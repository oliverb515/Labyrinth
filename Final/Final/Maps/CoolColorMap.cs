using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Final
{
    class CoolColorMap : EventMap
    {
        const int X_INDEX = 4;
        const int Y_INDEX = 0;

//      0 - feather not collected   1 - feather being collected     2 - feather collected
        int collectionState;

        Texture2D altar;
        Texture2D white;
        Texture2D feather;
        SpriteFont font;
        GameTime benchTime;
        Flash flash;

        Vector2 featherDrawLocation = new Vector2(192, 520);

//      When the feather is being collected, this variable determines how white to make the screen (it will increase from 0 to 1 then back to 0)
        double collectionAlpha = 0;

        public CoolColorMap(Game1 game, World world, Player player)
            : base(game, world, player, X_INDEX, Y_INDEX)
        {
            if (game.state.firstFeatherCollected)
                collectionState = 2;
            else
                collectionState = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (collectionState != 1) base.Update(gameTime);
            if (collectionState == 0 && currentKeyboardState.IsKeyDown(Keys.A) && !previousKeyboardState.IsKeyDown(Keys.A))
            {
                collectionState = 1;
                flash = new Flash(game);
                game.state.firstFeatherCollected = true;
                player.playerState.numFeathers += 1;
            }
            if (collectionState == 1)
            {
                if (flash == null || flash.alpha > 1) collectionState = 2;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            sb.Begin();
            sb.Draw(altar, new Vector2(192, 580), Color.White);
            if (collectionState < 2) drawFirstCollectionState();
            else if (collectionState == 1) drawSecondCollectionState(gameTime);
            sb.End();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            altar = Game.Content.Load<Texture2D>("textures/altar");
            font = Game.Content.Load<SpriteFont>("Fonts/plain");
            white = Game.Content.Load<Texture2D>("textures/white");
            feather = Game.Content.Load<Texture2D>("textures/featherBlob");
        }

        private void drawFirstCollectionState() {
            sb.DrawString(font, "'a' - Remove the feather from the altar", new Vector2(250, 425), Color.FromNonPremultiplied(228, 228, 228, 256 - (int)Math.Abs(player.Position.X-300)));
            sb.Draw(feather, featherDrawLocation, Color.White);
        }

        private void drawSecondCollectionState(GameTime gameTime)
        {
            sb.Draw(white, new Rectangle(0, 0, 1280, 720), Color.FromNonPremultiplied(0, 0, 0, (int)(collectionAlpha*255)));
        }
    }
}
