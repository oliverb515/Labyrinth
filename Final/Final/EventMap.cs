using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Final
{
//  This class use to serve a purpose, but now it really doesn't as maps that need to be handled differently can just inherit directly from Map.cs
    abstract class EventMap:Map
    {
        protected EventMap(Game1 game, World world, Player player, int x, int y, bool hasOverlay) : base(game, world, player, x, y, hasOverlay)
        {

        }

    }
}
