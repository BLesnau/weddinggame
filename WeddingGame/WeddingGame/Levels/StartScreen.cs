using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeddingGame.Engine;

namespace WeddingGame.Levels
{
   public class StartScreen : VPLevel
   {
      private Texture2D _background;

      public override void Update( GameTime gameTime )
      {

      }

      public override void Draw( GameTime gameTime )
      {
         DrawingHelper.Draw( _background, 
            new Rectangle( 0, 0, DrawingHelper.Graphics.PreferredBackBufferWidth, DrawingHelper.Graphics.PreferredBackBufferHeight ), 
            Color.White );
      }

      public override void LoadContent()
      {
         _background = DrawingHelper.GetTexture( "Something" );
      }

      public override void UnloadContent()
      {
         _background.Dispose();
      }
   }
}
