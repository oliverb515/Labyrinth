using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final
{
    class JumpMap : EventMap
    {

        const int X_INDEX = 6;
        const int Y_INDEX = 0;

        public JumpMap(Game1 game, World world, Player player)
                : base(game, world, player, X_INDEX, Y_INDEX, false)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (player.Position.Y > 710) player.Position = Vector2.Zero;
        }

    }
}
