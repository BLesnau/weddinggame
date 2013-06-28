using Microsoft.Xna.Framework.Audio;
using WeddingGame.Engine;

namespace WeddingGame
{
   public static class SoundHelper
   {
      private static SoundEffect _theme;
      private static SoundEffectInstance _themeIntance;

      private static SoundEffect _vow;
      private static SoundEffectInstance _vowInst1;
      private static SoundEffectInstance _vowInst2;
      private static SoundEffectInstance _vowInst3;

      private static SoundEffectInstance _vowLoop1;
      private static SoundEffectInstance _vowLoop2;
      private static SoundEffectInstance _vowLoop3;

      public static void LoadContent()
      {
         _theme = DrawingHelper.Content.Load<SoundEffect>( "ThemeSong" );
         _vow = DrawingHelper.Content.Load<SoundEffect>( "Vow" );

         _vowInst1 = _vow.CreateInstance();
         _vowInst2 = _vow.CreateInstance();
         _vowInst3 = _vow.CreateInstance();

         _vowLoop1 = _vow.CreateInstance();
         _vowLoop2 = _vow.CreateInstance();
         _vowLoop3 = _vow.CreateInstance();
         _vowLoop1.IsLooped = true;
         _vowLoop2.IsLooped = true;
         _vowLoop3.IsLooped = true;
      }

      public static void PlayTheme()
      {
         _themeIntance = _theme.CreateInstance();
         _themeIntance.IsLooped = true;
         _themeIntance.Play();
      }

      public static void PlayVow( ActionLocation loc )
      {
         if ( loc == ActionLocation.Left )
         {
            _vowInst1.Play();
            _vowInst1.Play();
         }

         if ( loc == ActionLocation.Middle )
         {
            _vowInst2.Play();
            _vowInst2.Play();
         }

         if ( loc == ActionLocation.Right )
         {
            _vowInst3.Play();
            _vowInst3.Play();
         }
      }

      public static void LoopVows()
      {
         _vowLoop1.Stop();
         _vowLoop2.Stop();
         _vowLoop3.Stop();

         _vowLoop1.Play();
         _vowLoop2.Play();
         _vowLoop3.Play();
      }

      public static void UnloopVows()
      {
         _vowLoop1.Stop();
         _vowLoop2.Stop();
         _vowLoop3.Stop();
      }
   }
}
