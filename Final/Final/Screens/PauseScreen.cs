
#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Final;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Final
{
    class PauseScreen : GameScreen
    {
        #region Fields
        GameScreen backgroundScreen;
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
//      A 32x32 block with the color/alpha value to draw over the whole pause screen
        Texture2D pauseBlock;
        #endregion

		public PauseScreen(GameScreen backgroundScreen)
		{
            previousKeyboardState = Keyboard.GetState();
		}

        public override void LoadContent()
        {
            pauseBlock = Load<Texture2D>("textures/pauseBlock");
            base.LoadContent();
        }

        public override void HandleInput(InputState input)
        {
            currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P)) ExitScreen();
            previousKeyboardState = currentKeyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(pauseBlock, new Rectangle(0, 0, 1280, 720), Color.White);
            ScreenManager.SpriteBatch.End();
        }
	}
}
