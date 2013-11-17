using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Final
{
    public class GameState
    {
        public int mapX { get; set; }
        public int mapY { get; set; }
        public int feathers { get; set; }
        public bool gameStarted { get; set; }
    }
}
