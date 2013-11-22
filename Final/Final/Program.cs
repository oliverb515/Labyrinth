using System;
using System.IO;
using Newtonsoft.Json;

namespace Final
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            GameState state;
            using (StreamReader r = new StreamReader("res/data.json"))
            {
                string json = r.ReadToEnd();
                state = JsonConvert.DeserializeObject<GameState>(json);
            }
            using (Game1 game = new Game1(state))
            {
                game.Run();
            }
        }
    }
}
