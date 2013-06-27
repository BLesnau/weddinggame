using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WeddingGame.Engine
{
   public static class DrawingHelper
   {
      public static SpriteBatch SpriteBatch { get; private set; }
      public static GraphicsDeviceManager Graphics { get; private set; }
      public static ContentManager Content { get; private set; }
      private static SpriteFont font;

      public static void Init( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ContentManager content )
      {
         SpriteBatch = spriteBatch;
         Graphics = graphics;
         Content = content;

         font = Content.Load<SpriteFont>( "DefaultFont" );
      }

      public static void Draw( Texture2D texture, Rectangle rect, Color color )
      {
         SpriteBatch.Draw( texture, rect, color );
      }

      public static Texture2D GetTexture( string assetName )
      {
         return Content.Load<Texture2D>( assetName );
      }

      public static void BeginDraw()
      {
         SpriteBatch.Begin();
      }

      public static void EndDraw()
      {
         SpriteBatch.End();
      }

      public static void DrawText( string text, float x, float y, Color color, float scale )
      {
         DrawText( text, new Vector2( x, y ), color, scale );
      }

      public static void DrawText( string text, Vector2 location, Color color, float scale )
      {
         SpriteBatch.DrawString( font, text, location, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0 );
      }
   }
}
