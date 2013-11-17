
#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Final
{
    class MainMenuScreen : MenuScreen
    {
        Texture2D titleTexure;
        Texture2D tree;
        Texture2D menuBG;
        Texture2D black;
        //412, 385
        Effect lighting;
        Vector2[] lights;
        SoundEffect menuSong;
        Vector2 LightSrc;
        double intensity;
        bool soundHelper = true;

        #region Initialization
        public MainMenuScreen()
            : base(String.Empty)
        {
            IsPopup = true;
            // Create our menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Play");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            LightSrc = new Vector2(0.321875f, 0.534722f);
            intensity = 10.0f;
        }
        #endregion

        #region Overrides
        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                var position = entry.Position;

                position.Y += 60;

                entry.Position = position;
            }
        }
        public override void LoadContent()
        {
            //tree = Load<Texture2D>("textures/tree");
            menuBG = Load<Texture2D>("textures/menu");
            lighting = Load<Effect>("lighting");
            titleTexure = Load<Texture2D>("textures/title");
            black = Load<Texture2D>("black");
            lights = new Vector2[1];
            lights[0] = new Vector2((412) / 1280.0f, ((385) / 720.0f));

            base.LoadContent();

        }
        #endregion

        #region Event Handlers for Menu Items
        /// <summary>
        /// Handles "Play" menu item selection
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen(ScreenManager.Game));
        }

        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        /// 
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Handles "Select Background Music" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectBackgroundMusicMenuEntrySelected(object sender, EventArgs e)
        {
            var backgroundScreen = new BackgroundScreen();

            ScreenManager.AddScreen(backgroundScreen);
            ScreenManager.AddScreen(new MusicSelectionScreen(backgroundScreen));
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {

            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();
            spriteBatch.Draw(menuBG, Vector2.Zero, Color.White);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                //bool isSelected = IsActive && (i == selectedEntry);

                //menuEntry.Draw(this, false, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            //spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
            //                       titleOrigin, titleScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Labyrinth", new Vector2(470, 60), Color.White);

            spriteBatch.End();
            //lighting.Parameters["lightPos"].SetValue(LightSrc);
            double sinVal = Math.Sin(gameTime.TotalGameTime.TotalMilliseconds * 0.0014816);
            intensity = 3.5*(2 + sinVal);
            lighting.Parameters["intensity"].SetValue((float)(intensity));
            lighting.Parameters["lightPos"].SetValue(LightSrc);
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, lighting);
            ScreenManager.SpriteBatch.Draw(black, new Rectangle(0, 0, 1280, 720), Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}