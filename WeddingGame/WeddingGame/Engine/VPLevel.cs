using Microsoft.Xna.Framework;

namespace WeddingGame.Engine
{
   public abstract class VPLevel
   {
      protected VPLevel( )
      {
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
