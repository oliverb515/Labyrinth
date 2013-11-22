using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final
{
    class PlayerState
    {

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;

        public bool onGround;
        public bool rising;

        public int numFeathers { get; set; }

        public PlayerState(Vector2 pos, Vector2 vel, int feathers)
        {
            position = pos;
            velocity = vel;
            numFeathers = feathers;
        }

    }
}
