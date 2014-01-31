using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Final
{
    class World
    {
        public Game1 game;
        public GameplayScreen screen;

        //2-dimensional array of the file names of the collision maps
        String[,] collisionMaps;
        //2-dimensional array of the file names of background textures
        String[,] backgrounds;

        int worldWidth = 11;
        int worldHeight = 4;

        public int currentX { get; set; }
        public int currentY { get; set; }

        public World(Game1 game, GameplayScreen gps)
        {
            this.game = game;
            screen = gps;
            currentX = 0;
            currentY = 0;

            collisionMaps = new String[worldWidth, worldHeight];
            backgrounds = new String[worldWidth, worldHeight];
        }

        public World(Game1 game, GameplayScreen gps, int x, int y)
        {
            this.game = game;
            screen = gps;
            currentX = x;
            currentY = y;

            collisionMaps = new String[worldWidth, worldHeight];
            backgrounds = new String[worldWidth, worldHeight];
        }

        public void loadArrays()
        {
            using (StreamReader colMapsStreamReader = new StreamReader("res/colMaps/master.txt")) {
            using (StreamReader bgStreamReader = new StreamReader("res/bgMASTER.txt")) {
                for (int y = 0; y < worldHeight; y++)
                {
                    String line1 = colMapsStreamReader.ReadLine();
                    String line2 = bgStreamReader.ReadLine();
                    String[] strings1 = line1.Split(' ');
                    String[] strings2 = line2.Split(' ');
                    for (int x = 0; x < worldWidth; x++)
                    {
                        collisionMaps[x, y] = strings1[x];
                        backgrounds[x, y] = strings2[x];
                    }

                }
            }
            }
        }

        #region Old Methods
        /*public Map getMapAbove()
        {
            return (true) ? new Map(this, currentX, --currentY) : null;
        }

        public Map getMapBelow()
        {
            return new Map(this, currentX, ++currentY);
        }

        public Map getMapToLeft()
        {
            return new Map(this, --currentX, currentY);
        }

        public Map getMapToRight()
        {
            return new Map(this, ++currentX, currentY);
        }*/
        #endregion

        public Map getMap(int horizontalTransition, int verticalTransition)
        {
            currentX += horizontalTransition;
            currentY += verticalTransition;
            game.state.mapX = currentX;
            game.state.mapY = currentY;
            string json = JsonConvert.SerializeObject(game.state, Formatting.Indented);
            File.WriteAllText("res/data.json", json);

            if (backgrounds[currentX, currentY].StartsWith("e"))
            {
                if (currentX == 4 && currentY == 2) return new FirstFeatherMap(game, this, screen.player, screen.player.Position);
                else if (currentX == 4 && currentY == 0) return new CoolColorMap(game, this, screen.player);
                else if (currentX == 4 && currentY == 1) return new PostCabMap(game, this, screen.player);
                else if (currentX == 6 && currentY == 0) return new JumpMap(game, this, screen.player);
                else if (currentX == 8 && currentY == 1) return new BellroomMap(game, this, screen.player);
                else return null;

            }
            else return new Map(game, this, screen.player, currentX, currentY, backgrounds[currentX, currentY].EndsWith("o"));
        }

        public String getBackgroundFilename(int x, int y)
        {
            return backgrounds[x, y];
        }

        public String getCollisionMapFilename(int x, int y)
        {
            return collisionMaps[x, y];
        }
    }
}
