using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

      private Head _brettHead = null;
      private Head _laynaHead = null;
      private Head _dunkHead = null;
      private Texture2D _background = null;

      ActionLocation? _actionLocation = null;

      public override void Update( GameTime gameTime )
      {
         if ( _waveManager == null )
         {
            _leftBarrier = DrawingHelper.Graphics.PreferredBackBufferWidth / 3.0;
            _rightBarrier = _leftBarrier * 2;
            var waveTime = 30;

            _waveManager = new WaveManager();
            _waveManager.AddWaves( new Wave[]
            {               
               new Wave("Wave 1", waveTime, 3, 3, false, false),           
               new Wave("Wave 2", waveTime, 2, 2, true, false),
               new Wave("Wave 3", waveTime, 1.5, 1.5, true, true),
               new Wave("Wave 4", waveTime, 1.0, 1.0, false, false),
               new Wave("Wave 5", waveTime, 1.0, 1.0, true, false),
               new Wave("Wave 6", waveTime, 1.0, 1.0, true, true),
            } );

            _waveManager.WaveComplete += WaveComplete;
            _waveManager.GameComplete += GameComplete;

            _waveManager.Start();
         }
         else
         {
            if ( _waveManager.Update( gameTime ) )
            {
               Counter.Add();

               if ( _actionLocation != null )
               {
                  ShakeHead( _actionLocation.Value );
                  _actionLocation = null;
               }
            }
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

                  _actionLocation = GetLocation( _tapLocation );

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

                        _actionLocation = GetLocation( _startVerticalDrag );

                        break;
                     }
                     case GestureType.HorizontalDrag:
                     {
                        _previousAction = _previousGesture;

                        _waveManager.ActionOccurred( GetLocation( _startHorizontalDrag ), _directionHorizontalDrag > 0 ? ActionType.FlickRight : ActionType.FlickLeft );

                        _actionLocation = GetLocation( _startHorizontalDrag );

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

      private void ShakeHead( ActionLocation loc )
      {
         if ( loc == ActionLocation.Left )
         {
            _laynaHead.Shake();
         }
         else if ( loc == ActionLocation.Middle )
         {
            _dunkHead.Shake();
         }
         else if ( loc == ActionLocation.Right )
         {
            _brettHead.Shake();
         }
      }

      public override void Draw( GameTime gameTime )
      {
         if ( _won )
         {
            VPLevelManager.SetLevel( typeof( WonScreen ) );
         }
         if ( _lost )
         {
            VPLevelManager.SetLevel( typeof( LostScreen ) );
         }

         DrawingHelper.Graphics.GraphicsDevice.Clear( Color.DarkSlateGray );

         DrawingHelper.Draw( _background,
            new Rectangle( 0, 0, DrawingHelper.Graphics.PreferredBackBufferWidth, DrawingHelper.Graphics.PreferredBackBufferHeight ),
            Color.White );

         _dunkHead.Draw( gameTime );
         _brettHead.Draw( gameTime );
         _laynaHead.Draw( gameTime );

         if ( _waveManager != null )
         {
            _waveManager.Draw( gameTime );
         }

         //if ( _previousAction == GestureType.VerticalDrag )
         //{
         //   var debugText = "Action: {0}\nStart: {1},{2}\nEnd: {3},{4}\nLength: {5}\nDirection: {6}";
         //   DrawingHelper.DrawText( string.Format( debugText, "Flick Vertical", _startVerticalDrag.X, _startVerticalDrag.Y, _endVerticalDrag.X, _endVerticalDrag.Y, _lengthVerticalDrag, _directionVerticalDrag ),
         //                           10, 10, Color.White );
         //}
         //else if ( _previousAction == GestureType.HorizontalDrag )
         //{
         //   var debugText = "Action: {0}\nStart: {1},{2}\nEnd: {3},{4}\nLength: {5}\nDirection: {6}";
         //   DrawingHelper.DrawText( string.Format( debugText, "Flick Horizontal", _startHorizontalDrag.X, _startHorizontalDrag.Y, _endHorizontalDrag.X, _endHorizontalDrag.Y, _lengthHorizontalDrag, _directionHorizontalDrag ),
         //                           10, 10, Color.White );
         //}
         //else if ( _previousAction == GestureType.Tap )
         //{
         //   var debugText = "Action: {0}\nLocation: {1},{2}";
         //   DrawingHelper.DrawText( string.Format( debugText, "Tap", _tapLocation.X, _tapLocation.Y ),
         //                           10, 10, Color.White );
         //}
      }

      public override void LoadContent()
      {
         Counter.Reset();

         _previousGesture = GestureType.None;
         _tapLocation = Vector2.Zero;
         _startVerticalDrag = Vector2.Zero;
         _endVerticalDrag = Vector2.Zero;
         _lengthVerticalDrag = 0;
         _directionVerticalDrag = 0;
         _startHorizontalDrag = Vector2.Zero;
         _endHorizontalDrag = Vector2.Zero;
         _lengthHorizontalDrag = 0;
         _directionHorizontalDrag = 0;
         _previousAction = GestureType.None;
         _waveManager = null;
         _leftBarrier = 0;
         _rightBarrier = 0;
         _lost = false;
         _won = false;
         _brettHead = null;
         _laynaHead = null;
         _dunkHead = null;
         _background = null;

         _background = DrawingHelper.GetTexture( "background_nofaces" );

         if ( _brettHead == null )
         {
            _brettHead = new Head( "BrettNormal", "BrettOpen", new Vector2( 430, 115 ), new Vector2( 425, 120 ) );
            _laynaHead = new Head( "LaynaNormal", "LaynaOpen", new Vector2( 300, 120 ), new Vector2( 290, 125 ) );
            _dunkHead = new Head( "DunkNormal", "DunkOpen", new Vector2( 363, 45 ), new Vector2( 330, 50 ) );

            _brettHead.LoadContent();
            _laynaHead.LoadContent();
            _dunkHead.LoadContent();
         }
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
