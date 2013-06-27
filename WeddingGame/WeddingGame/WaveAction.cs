using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeddingGame.Engine;

namespace WeddingGame
{
   public enum ActionType
   {
      FlickUp,
      FlickDown,
      FlickLeft,
      FlickRight,
      Tap,
   }

   public enum ActionLocation
   {
      Left,
      Middle,
      Right
   }

   public struct ActionObj
   {
      public ActionLocation ActionLocation { get; set; }
      public ActionType ActionType { get; set; }
   }

   public class WaveAction
   {
      private List<ActionObj> _actions = new List<ActionObj>();
      private static Random _random = new Random();

      private Texture2D _tapButton;
      private Texture2D _upArrow;
      private Texture2D _downArrow;
      private Texture2D _leftArrow;
      private Texture2D _rightArrow;

      public void AddAction()
      {
         var addedValue = false;
         while ( !addedValue )
         {
            var values = GetValues<ActionLocation>();
            var randomLocation = (ActionLocation) values.GetValue( _random.Next( values.Length ) );

            var doWork = true;
            foreach ( var a in _actions )
            {
               if ( a.ActionLocation == randomLocation )
               {
                  doWork = false;
               }
            }

            if ( doWork )
            {
               var values2 = GetValues<ActionType>();
               var randomAction = (ActionType) values2.GetValue( _random.Next( values2.Length ) );

               _actions.Add( new ActionObj
               {
                  ActionLocation = randomLocation,
                  ActionType = randomAction
               } );

               addedValue = true;
            }
         }
      }

      public virtual void Draw( GameTime gameTime )
      {
         var textColor = Color.White;
         float middleH = DrawingHelper.Graphics.PreferredBackBufferHeight / 2.0f;

         foreach ( var a in _actions )
         {
            var drawLoc = new Vector2( 0, 275 );
            switch ( a.ActionLocation )
            {
               case ActionLocation.Left:
               {
                  drawLoc.X = 100;
                  break;
               }
               case ActionLocation.Middle:
               {
                  drawLoc.X = 366;
                  break;
               }
               case ActionLocation.Right:
               {
                  drawLoc.X = 633;
                  break;
               }
            }

            var image = _tapButton;
            switch ( a.ActionType )
            {
               case ActionType.FlickUp:
               {
                  image = _upArrow;
                  break;
               }
               case ActionType.FlickDown:
               {
                  image = _downArrow;
                  break;
               }
               case ActionType.FlickLeft:
               {
                  image = _leftArrow;
                  break;
               }
               case ActionType.FlickRight:
               {
                  image = _rightArrow;
                  break;
               }
               case ActionType.Tap:
               {
                  image = _tapButton;
                  break;
               }
            }

            DrawingHelper.Draw( image, new Rectangle( (int) drawLoc.X, (int) drawLoc.Y, image.Width, image.Height ), Color.White );
         }
      }

      private static T[] GetValues<T>()
      {
         Type enumType = typeof( T );
         if ( !enumType.IsEnum )
            throw new ArgumentException( "Type '" + enumType.Name + "' is not an enum" );

         FieldInfo[] fields = enumType.GetFields();
         int literalCount = 0;
         for ( int i = 0; i < fields.Length; i++ )
            if ( fields[i].IsLiteral == true )
               literalCount++;

         T[] arr = new T[literalCount];
         int pos = 0;
         for ( int i = 0; i < fields.Length; i++ )
            if ( fields[i].IsLiteral == true )
            {
               arr[pos] = (T) fields[i].GetValue( enumType );
               pos++;
            }

         return arr;
      }

      public void ActionOccurred( ActionLocation location, ActionType type )
      {
         ActionObj? action = null;
         foreach ( var a in _actions )
         {
            if ( a.ActionLocation == location && a.ActionType == type )
            {
               action = a;
               break;
            }
         }

         if ( action != null )
         {
            _actions.Remove( action.Value );
         }
      }

      public bool ActionCompleted()
      {
         return _actions.Count == 0;
      }

      public int ActionCount()
      {
         return _actions.Count;
      }

      public void LoadContent()
      {
         _tapButton = DrawingHelper.GetTexture( "Tap" );
         _upArrow = DrawingHelper.GetTexture( "Up" );
         _downArrow = DrawingHelper.GetTexture( "Down" );
         _leftArrow = DrawingHelper.GetTexture( "Left" );
         _rightArrow = DrawingHelper.GetTexture( "Right" );
      }
   }
}
