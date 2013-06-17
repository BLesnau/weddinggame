using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WeddingGame.Engine
{
   public static class VPLevelManager
   {
      public static VPLevel CurrentLevel { get; private set; }

      private static List<VPLevel> _levels;

      public static void Init( List<VPLevel> levels )
      {
         _levels = levels;
         CurrentLevel = null;

         if ( levels.Any() )
         {
            SetLevel( levels[0].GetType() );
         }
      }

      public static bool SetLevel( Type levelType )
      {
         try
         {
            var level = _levels.Single( x => x.GetType() == levelType );
            if ( CurrentLevel != null )
            {
               CurrentLevel.UnloadContent();
            }

            CurrentLevel = level;
            CurrentLevel.LoadContent();

            return true;
         }
         catch ( Exception ex )
         {
            return false;
         }
      }

      public static void Update( GameTime gameTime )
      {
         _levels.ForEach( x => x.Update( gameTime ) );
      }

      public static void Draw( GameTime gameTime )
      {
         _levels.ForEach( x => x.Draw( gameTime ) );
      }

      public static void LoadContent()
      {
         _levels.ForEach( x => x.LoadContent() );
      }

      public static void UnloadContent()
      {
         _levels.ForEach( x => x.UnloadContent() );
      }
   }
}