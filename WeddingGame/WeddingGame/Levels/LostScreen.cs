using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WeddingGame.Engine;

namespace WeddingGame.Levels
{
   public class LostScreen : VPLevel
   {
      private Texture2D _background;
      private Head _brettHead = null;
      private Head _laynaHead = null;
      private Head _dunkHead = null;

      public override void Update( GameTime gameTime )
      {
         while ( TouchPanel.IsGestureAvailable )
         {
            var gesture = TouchPanel.ReadGesture();

            if ( gesture.GestureType == GestureType.Tap || gesture.GestureType == GestureType.DoubleTap )
            {
               VPLevelManager.SetLevel( typeof( StartScreen ) );
            }
         }
      }

      public override void Draw( GameTime gameTime )
      {
         DrawingHelper.Draw( _background,
            new Rectangle( 0, 0, DrawingHelper.Graphics.PreferredBackBufferWidth, DrawingHelper.Graphics.PreferredBackBufferHeight ),
            Color.White );

         _dunkHead.Draw( gameTime );
         _brettHead.Draw( gameTime );
         _laynaHead.Draw( gameTime );

         DrawingHelper.DrawText( "You finished", 40, 25, Color.White, 1.75f );
         DrawingHelper.DrawText( "your vows!!!", 40, 60, Color.White, 1.75f );
         DrawingHelper.DrawText( "(tap to continue)", 57, 110, Color.White, 1.0f );

         //DrawingHelper.DrawText( "You did not", 65, 5, Color.White, 1.75f );
         //DrawingHelper.DrawText( "finish your vows!!!", 5, 40, Color.White, 1.75f );

         //DrawingHelper.DrawText( 57.ToString() +  " vows said", 75, 90, Color.White, 1.5f );

         //DrawingHelper.DrawText( "(tap to continue)", 85, 145, Color.White, 1.0f );
      }

      public override void LoadContent()
      {
         _background = DrawingHelper.GetTexture( "background_nofaces" );

         if ( _brettHead == null )
         {
            _brettHead = new Head( "BrettNormal", "BrettOpen", new Vector2( 430, 115 ), new Vector2( 425, 120 ) );
            _laynaHead = new Head( "LaynaNormal", "LaynaOpen", new Vector2( 300, 120 ), new Vector2( 290, 125 ) );
            _dunkHead = new Head( "DunkNormal", "DunkOpen", new Vector2( 363, 45 ), new Vector2( 330, 50 ) );

            _brettHead.LoadContent();
            _laynaHead.LoadContent();
            _dunkHead.LoadContent();

            _brettHead.ShakeForever();
            _laynaHead.ShakeForever();
            _dunkHead.ShakeForever();
         }
      }

      public override void UnloadContent()
      {
      }
   }
}
