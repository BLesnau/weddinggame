using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using WeddingGame.Engine;

namespace WeddingGame.Levels
{
   public class MainGame : VPLevel
   {
      private TimeSpan _gestureTimeout = TimeSpan.FromSeconds( 5.0 );
      private TimeSpan _lastGestureTime = TimeSpan.FromSeconds( 0 );

      private GestureType _previousGesture = GestureType.None;

      private Vector2 _tapLocation = Vector2.Zero;

      private Vector2 _startVerticalDrag = Vector2.Zero;
      private Vector2 _endVerticalDrag = Vector2.Zero;
      private float _lengthVerticalDrag = 0;
      private int _directionVerticalDrag = 0;

      private Vector2 _startHorizontalDrag = Vector2.Zero;
      private Vector2 _endHorizontalDrag = Vector2.Zero;
      private float _lengthHorizontalDrag = 0;
      private int _directionHorizontalDrag = 0;

      private GestureType _previousAction = GestureType.None;

      private WaveManager _waveManager = null;
      private double _leftBarrier = 0;
      private double _rightBarrier = 0;

      private bool _lost = false;
      private bool _won = false;

      public override void Update( GameTime gameTime )
      {
         if ( _waveManager == null )
         {
            _leftBarrier = DrawingHelper.Graphics.PreferredBackBufferWidth / 3.0;
            _rightBarrier = _leftBarrier * 2;

            _waveManager = new WaveManager();
            _waveManager.AddWaves( new Wave[]
            {
               new Wave("Wave 1", 10, 2, 2, false),
               new Wave("Wave 2", 30, 2, 2, true)
            } );

            _waveManager.WaveComplete += WaveComplete;
            _waveManager.GameComplete += GameComplete;

            _waveManager.Start();
         }
         else
         {
            _waveManager.Update( gameTime );
         }

         if ( /*_previousGesture == GestureType.Flick ||*/ ( ( _lastGestureTime + _gestureTimeout ) < gameTime.TotalGameTime ) )
         {
            _previousGesture = GestureType.None;

            ResetTap();

            ResetHorizontalDrag();

            ResetVerticalDrag();

            _previousAction = GestureType.None;
         }

         while ( TouchPanel.IsGestureAvailable )
         {
            var gesture = TouchPanel.ReadGesture();

            switch ( gesture.GestureType )
            {
               case GestureType.Tap:
               {
                  ResetHorizontalDrag();
                  ResetVerticalDrag();

                  _tapLocation = gesture.Position;

                  _previousAction = gesture.GestureType;

                  _waveManager.ActionOccurred( GetLocation( _tapLocation ), ActionType.Tap );

                  break;
               }
               case GestureType.Flick:
               {
                  switch ( _previousGesture )
                  {
                     case GestureType.VerticalDrag:
                     {
                        _previousAction = _previousGesture;

                        _waveManager.ActionOccurred( GetLocation( _startVerticalDrag ), _directionVerticalDrag > 0 ? ActionType.FlickDown : ActionType.FlickUp );

                        break;
                     }
                     case GestureType.HorizontalDrag:
                     {
                        _previousAction = _previousGesture;

                        _waveManager.ActionOccurred( GetLocation( _startHorizontalDrag ), _directionHorizontalDrag > 0 ? ActionType.FlickRight : ActionType.FlickLeft );

                        break;
                     }
                  }

                  break;
               }
               case GestureType.VerticalDrag:
               {
                  ResetTap();
                  ResetHorizontalDrag();

                  if ( _previousGesture != GestureType.VerticalDrag )
                  {
                     _startVerticalDrag = gesture.Position;
                  }

                  _endVerticalDrag = gesture.Position;

                  var diff = ( _endVerticalDrag - _startVerticalDrag ).Y;
                  var newDirection = 0;
                  if ( diff < 0 )
                  {
                     newDirection = -1;
                  }
                  else if ( diff > 0 )
                  {
                     newDirection = 1;
                  }
                  else
                  {
                     newDirection = 0;
                  }

                  if ( newDirection != 0 && newDirection != _directionVerticalDrag )
                  {
                     _startVerticalDrag = gesture.Position;
                     _endVerticalDrag = gesture.Position;
                  }

                  _directionVerticalDrag = newDirection;

                  _lengthVerticalDrag = ( _endVerticalDrag - _startVerticalDrag ).Length();

                  break;
               }
               case GestureType.HorizontalDrag:
               {
                  ResetTap();
                  ResetVerticalDrag();

                  if ( _previousGesture != GestureType.HorizontalDrag )
                  {
                     _startHorizontalDrag = gesture.Position;
                  }

                  _endHorizontalDrag = gesture.Position;

                  var diff = ( _endHorizontalDrag - _startHorizontalDrag ).X;
                  var newDirection = 0;
                  if ( diff < 0 )
                  {
                     newDirection = -1;
                  }
                  else if ( diff > 0 )
                  {
                     newDirection = 1;
                  }
                  else
                  {
                     newDirection = 0;
                  }

                  if ( newDirection != 0 && newDirection != _directionHorizontalDrag )
                  {
                     _startHorizontalDrag = gesture.Position;
                     _endHorizontalDrag = gesture.Position;
                  }

                  _directionHorizontalDrag = newDirection;

                  _lengthHorizontalDrag = ( _endHorizontalDrag - _startHorizontalDrag ).Length();

                  break;
               }
            }

            _previousGesture = gesture.GestureType;
            _lastGestureTime = gameTime.TotalGameTime;
         }
      }

      public override void Draw( GameTime gameTime )
      {
         if ( _won )
         {
            VPLevelManager.SetLevel( typeof(  WonScreen) );
         }
         if ( _lost )
         {
            VPLevelManager.SetLevel( typeof( LostScreen ) );
         }

         if ( _waveManager != null )
         {
            _waveManager.Draw( gameTime );
         }

         DrawingHelper.Graphics.GraphicsDevice.Clear( Color.DarkSlateGray );

         if ( _previousAction == GestureType.VerticalDrag )
         {
            var debugText = "Action: {0}\nStart: {1},{2}\nEnd: {3},{4}\nLength: {5}\nDirection: {6}";
            DrawingHelper.DrawText( string.Format( debugText, "Flick Vertical", _startVerticalDrag.X, _startVerticalDrag.Y, _endVerticalDrag.X, _endVerticalDrag.Y, _lengthVerticalDrag, _directionVerticalDrag ),
                                    10, 10, Color.White );
         }
         else if ( _previousAction == GestureType.HorizontalDrag )
         {
            var debugText = "Action: {0}\nStart: {1},{2}\nEnd: {3},{4}\nLength: {5}\nDirection: {6}";
            DrawingHelper.DrawText( string.Format( debugText, "Flick Horizontal", _startHorizontalDrag.X, _startHorizontalDrag.Y, _endHorizontalDrag.X, _endHorizontalDrag.Y, _lengthHorizontalDrag, _directionHorizontalDrag ),
                                    10, 10, Color.White );
         }
         else if ( _previousAction == GestureType.Tap )
         {
            var debugText = "Action: {0}\nLocation: {1},{2}";
            DrawingHelper.DrawText( string.Format( debugText, "Tap", _tapLocation.X, _tapLocation.Y ),
                                    10, 10, Color.White );
         }
      }

      public override void LoadContent()
      {
      }

      public override void UnloadContent()
      {
      }

      private void WaveComplete( string waveName )
      {
         waveName.GetHashCode();
      }

      private void GameComplete( bool won, double accuracy )
      {
         if ( won )
         {
            _won = true;
         }
         else
         {
            _lost = true;
         }
      }

      private ActionLocation GetLocation( Vector2 loc )
      {
         if ( loc.X <= _leftBarrier )
         {
            return ActionLocation.Left;
         }

         if ( loc.X > _leftBarrier && loc.X <= _rightBarrier )
         {
            return ActionLocation.Middle;
         }

         if ( loc.X > _rightBarrier && loc.X <= DrawingHelper.Graphics.PreferredBackBufferWidth )
         {
            return ActionLocation.Right;
         }

         return ActionLocation.Left;
      }

      private void ResetHorizontalDrag()
      {
         _startHorizontalDrag = Vector2.Zero;
         _endHorizontalDrag = Vector2.Zero;
         _lengthHorizontalDrag = 0;
         _directionHorizontalDrag = 0;
      }

      private void ResetVerticalDrag()
      {
         _startVerticalDrag = Vector2.Zero;
         _endVerticalDrag = Vector2.Zero;
         _lengthVerticalDrag = 0;
         _directionVerticalDrag = 0;
      }

      private void ResetTap()
      {
         _tapLocation = Vector2.Zero;
      }
   }
}
