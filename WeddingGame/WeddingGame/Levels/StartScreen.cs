using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WeddingGame.Engine;

namespace WeddingGame.Levels
{
   public class StartScreen : VPLevel
   {
      private Texture2D _background;

      public override void Update( GameTime gameTime )
      {
         while ( TouchPanel.IsGestureAvailable )
         {
            var gesture = TouchPanel.ReadGesture();

            if ( gesture.GestureType == GestureType.Tap || gesture.GestureType == GestureType.DoubleTap )
            {
               VPLevelManager.SetLevel( typeof( InstructionsScreen ) );
            }
         }
      }

      public override void Draw( GameTime gameTime )
      {
         DrawingHelper.Draw( _background,
            new Rectangle( 0, 0, DrawingHelper.Graphics.PreferredBackBufferWidth, DrawingHelper.Graphics.PreferredBackBufferHeight ),
            Color.White );
      }

      public override void LoadContent()
      {
         _background = DrawingHelper.GetTexture( "background" );
      }

      public override void UnloadContent()
      {
      }
   }
}
