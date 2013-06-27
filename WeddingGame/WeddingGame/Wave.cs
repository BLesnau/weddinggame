using System;
using Microsoft.Xna.Framework;

namespace WeddingGame
{
   public class Wave
   {
      public string Name = string.Empty;
      public TimeSpan Length = TimeSpan.Zero;
      public TimeSpan TimeBetweenActions = TimeSpan.Zero;
      public TimeSpan TimeToWait = TimeSpan.Zero;
      public bool CanShowTwoActions = false;

      public Wave( string name, double lengthSeconds, double secondsBetweenActions, double secondsToWait, bool canDoTwoActions )
      {
         Name = name;
         Length = TimeSpan.FromSeconds( lengthSeconds );
         TimeBetweenActions = TimeSpan.FromSeconds( secondsBetweenActions );
         TimeToWait = TimeSpan.FromSeconds( secondsToWait );
         CanShowTwoActions = canDoTwoActions;
      }

      public virtual void Update( GameTime gameTime )
      {
      }

      public virtual void Draw( GameTime gameTime )
      {
      }

      public virtual void LoadContent()
      {
      }

      public virtual void UnloadContent()
      {
      }
   }
}
