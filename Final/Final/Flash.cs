using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Final
{
    class Flash : DrawableGameComponent
    {
        Effect flashEffect;
        Texture2D white;
        SpriteBatch sb;
        bool rising;
        public float alpha;

        public Flash(Game1 game) 
            : base(game)
        {
            if (game == null) Environment.Exit(0);
            this.DrawOrder = 2;
            game.Components.Add(this);
            sb = new SpriteBatch(GraphicsDevice);
            alpha = 0;
            rising = true;
        }

        public override void Draw(GameTime gameTime)
        {
         
            base.Draw(gameTime);
            flashEffect.Parameters["alpha"].SetValue(alpha);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, flashEffect);
            sb.Draw(white, new Rectangle(0, 0, 1280, 720), Color.White);
            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (alpha > 1) rising = false;
            if (alpha < 0) Dispose(true);
            if (rising) alpha += 0.5f*((float)gameTime.ElapsedGameTime.TotalSeconds);
            else alpha -= 0.5f*((float)gameTime.ElapsedGameTime.TotalSeconds);
            System.Diagnostics.Debug.WriteLine(alpha);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            flashEffect = Game.Content.Load<Effect>("FlashEffect");
            white = Game.Content.Load<Texture2D>("textures/white");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Game.Components.Remove(this);
        }
    }
}