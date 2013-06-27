using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using WeddingGame.Engine;
using WeddingGame.Levels;

namespace WeddingGame
{
   public enum HeadState
   {
      Normal,
      Open,
      OpenForever
   }

   public class Head
   {
      private Texture2D _normal;
      private Texture2D _open;
      private Vector2 _normalPos = Vector2.Zero;
      private Vector2 _openPos = Vector2.Zero;

      private string _normalImageName = string.Empty;
      private string _openImageName = string.Empty;

      private HeadState _headState = HeadState.Normal;

      private TimeSpan _shakeTime = TimeSpan.FromSeconds( 2 );
      private TimeSpan _shakeStart = TimeSpan.Zero;

      private Vector2 _shakeSpot = Vector2.Zero;
      private TimeSpan _timeSinceShake = TimeSpan.Zero;
      private TimeSpan _timeBetweenShake = TimeSpan.FromSeconds( .1 );
      private bool _shook;
      private float _shakeMoveAmount = 25.0f;

      public Head( string normalImage, string openImage, Vector2 normalPos, Vector2 openPos )
      {
         _normalImageName = normalImage;
         _openImageName = openImage;
         _normalPos = normalPos;
         _openPos = openPos;
      }

      public void Update( GameTime gameTime )
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

      public void Draw( GameTime gameTime )
      {
         if ( _headState == HeadState.Normal )
         {
            DrawingHelper.Draw( _normal,
               new Rectangle( (int) _normalPos.X, (int) _normalPos.Y, _normal.Width, _normal.Height ),
               Color.White );
         }
         else if ( _headState == HeadState.Open )
         {
            if ( _shakeStart == TimeSpan.Zero )
            {
               _shakeStart = gameTime.TotalGameTime;
            }

            if ( ( gameTime.TotalGameTime - _shakeStart ) >= _shakeTime )
            {
               _headState = HeadState.Normal;
            }
            else
            {
               DrawingHelper.Draw( _open, new Rectangle( (int) _openPos.X, (int) _openPos.Y, _open.Width, _open.Height ), Color.White );

               //if ( ( gameTime.TotalGameTime - _timeSinceShake ) >= _timeBetweenShake )
               //{
               //   if ( _shook )
               //   {
               //      DrawingHelper.Draw( _open, new Rectangle( (int) _openPos.X, (int) _openPos.Y, _open.Width, _open.Height ), Color.White );
               //   }
               //   else
               //   {
               //      DrawingHelper.Draw( _open, new Rectangle( (int) _openPos.X, (int) ( _openPos.Y - _shakeMoveAmount ), _open.Width, _open.Height ),
               //                          Color.White );
               //   }

               //   _shook = !_shook;
               //   _timeSinceShake = gameTime.TotalGameTime;
               //}
            }
         }
         else if ( _headState == HeadState.OpenForever )
         {

         }
      }

      public void Shake()
      {
         _headState = HeadState.Open;
         _timeSinceShake = TimeSpan.Zero;
         _shook = false;
         _shakeStart = TimeSpan.Zero;
      }

      public void LoadContent()
      {
         _normal = DrawingHelper.GetTexture( _normalImageName );
         _open = DrawingHelper.GetTexture( _openImageName );
      }

      public void UnloadContent()
      {
      }
   }
}
