using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WeddingGame
{
   public delegate void WaveCompleteEventHandler( string waveName );
   public delegate void GameCompletedEventHandler( bool won, double accuracy );

   public enum GameState
   {
      None,
      BetweenMove,
      WaitingForMove,
      Done
   }

   public class WaveManager
   {
      private Queue<Wave> _waves = new Queue<Wave>();
      private Wave _currentWave = null;
      private bool _started = false;
      private bool _running = false;
      private GameTimer timer = new GameTimer();

      public event WaveCompleteEventHandler WaveComplete;
      public event GameCompletedEventHandler GameComplete;

      private GameState _state;
      private TimeSpan _timeToWait;
      private TimeSpan _timerStarted;
      private WaveAction _currentAction;
      private TimeSpan _waveStarted;

      public void Start()
      {
         _started = true;
         _state = GameState.None;
      }

      public void AddWave( Wave wave )
      {
         _waves.Enqueue( wave );
      }

      public void AddWaves( Wave[] waves )
      {
         var waveList = new List<Wave>( waves );
         waveList.ForEach( w => _waves.Enqueue( w ) );
      }

      private void StartNextWave( GameTime gameTime )
      {
         if ( _waves.Count > 0 )
         {
            _waveStarted = gameTime.TotalGameTime;
            _currentWave = _waves.Dequeue();
            SetTimerForNextAction( _currentWave.TimeBetweenActions, gameTime );
            _state = GameState.BetweenMove;
         }
         else
         {
            _state = GameState.Done;
            GameComplete( true, 1.0 );
         }
      }

      private void SetTimerForNextAction( TimeSpan timeBetweenActions, GameTime gameTime )
      {
         _timeToWait = timeBetweenActions;
         _timerStarted = gameTime.TotalGameTime;
      }

      public virtual void Update( GameTime gameTime )
      {
         if ( !_started )
         {
            return;
         }

         if ( !_running )
         {
            _running = true;
            StartNextWave( gameTime );
         }

         if ( _currentWave != null )
         {
            if ( ( gameTime.TotalGameTime - _waveStarted ) >= _currentWave.Length )
            {
               StartNextWave( gameTime );
            }

            if ( _state == GameState.WaitingForMove && FinishedAction() )
            {
               _state = GameState.BetweenMove;
               SetTimerForNextAction( _currentWave.TimeBetweenActions, gameTime );
            }
            else
            {
               if ( ( gameTime.TotalGameTime - _timerStarted ) >= _timeToWait )
               {
                  switch ( _state )
                  {
                     case GameState.BetweenMove:
                     {
                        _state = GameState.WaitingForMove;
                        _currentAction = _currentWave.GetAction();
                        SetTimerForNextAction( _currentWave.TimeToWait, gameTime );

                        break;
                     }
                     case GameState.WaitingForMove:
                     {
                        _state = GameState.Done;
                        GameComplete( false, 1.0 );

                        break;
                     }
                  }
               }
            }
         }
      }

      private bool FinishedAction()
      {
         return false;
      }

      public virtual void Draw( GameTime gameTime )
      {
         if ( !_running )
         {
            return;
         }
      }

      public virtual void LoadContent()
      {
      }

      public virtual void UnloadContent()
      {
      }
   }
}
