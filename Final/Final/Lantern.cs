using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final
{
    class Lantern
    {
        public Vector2 pos;
        public Vector2 vel;
        public bool beingCarried;
        public bool falling;
        Player player;

        public Lantern(Vector2 pos, Player p, bool carried)
        {
            this.pos = pos;
            player = p;
            beingCarried = carried;
        }

        public void drop()
        {
            vel = new Vector2();
            beingCarried = false;
            falling = true;
        }

        public void pickUp()
        {
            beingCarried = true;
        }

        public void update(GameTime time)
        {

            if (falling)
            {
                pos += (vel * time.ElapsedGameTime.Milliseconds);
                //gravity
                if (vel.Y < 0.5f)
                    vel.Y += 0.002f * time.ElapsedGameTime.Milliseconds;
            }
            if (beingCarried) pos = player.playerState.position;

        }


    }
}
