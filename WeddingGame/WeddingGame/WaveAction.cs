using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
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

      public void AddAction()
      {
         var addedValue = false;
         while ( !addedValue )
         {
            var values = GetValues<ActionLocation>();
            var randomLocation = (ActionLocation) values.GetValue( _random.Next( values.Length ) );

            foreach ( var a in _actions )
            {
               if ( a.ActionLocation == randomLocation )
               {
                  continue;
               }
            }

            var values2 = GetValues<ActionType>();
            var randomAction = (ActionType) values2.GetValue( _random.Next( values2.Length ) );

            _actions.Add( new ActionObj { ActionLocation = randomLocation, ActionType = randomAction } );

            addedValue = true;
         }
      }

      public virtual void Draw( GameTime gameTime )
      {
         var textColor = Color.White;
         float middleH = DrawingHelper.Graphics.PreferredBackBufferHeight / 2.0f;

         foreach ( var a in _actions )
         {
            switch ( a.ActionLocation )
            {
               case ActionLocation.Left:
               {
                  var location = new Vector2( 5, middleH );

                  switch ( a.ActionType )
                  {
                     case ActionType.FlickUp:
                     {
                        DrawingHelper.DrawText( "FlickUp", location, textColor );
                        break;
                     }
                     case ActionType.FlickDown:
                     {
                        DrawingHelper.DrawText( "FlickDown", location, textColor );
                        break;
                     }
                     case ActionType.FlickLeft:
                     {
                        DrawingHelper.DrawText( "FlickLeft", location, textColor );
                        break;
                     }
                     case ActionType.FlickRight:
                     {
                        DrawingHelper.DrawText( "FlickRight", location, textColor );
                        break;
                     }
                     case ActionType.Tap:
                     {
                        DrawingHelper.DrawText( "Tap", location, textColor );
                        break;
                     }
                  }

                  break;
               }
               case ActionLocation.Middle:
               {
                  var location = new Vector2( DrawingHelper.Graphics.PreferredBackBufferWidth / 2.0f, middleH );

                  switch ( a.ActionType )
                  {
                     case ActionType.FlickUp:
                     {
                        DrawingHelper.DrawText( "FlickUp", location, textColor );
                        break;
                     }
                     case ActionType.FlickDown:
                     {
                        DrawingHelper.DrawText( "FlickDown", location, textColor );
                        break;
                     }
                     case ActionType.FlickLeft:
                     {
                        DrawingHelper.DrawText( "FlickLeft", location, textColor );
                        break;
                     }
                     case ActionType.FlickRight:
                     {
                        DrawingHelper.DrawText( "FlickRight", location, textColor );
                        break;
                     }
                     case ActionType.Tap:
                     {
                        DrawingHelper.DrawText( "Tap", location, textColor );
                        break;
                     }
                  }

                  break;
               }
               case ActionLocation.Right:
               {
                  var location = new Vector2( DrawingHelper.Graphics.PreferredBackBufferWidth - 100, middleH );

                  switch ( a.ActionType )
                  {
                     case ActionType.FlickUp:
                     {
                        DrawingHelper.DrawText( "FlickUp", location, textColor );
                        break;
                     }
                     case ActionType.FlickDown:
                     {
                        DrawingHelper.DrawText( "FlickDown", location, textColor );
                        break;
                     }
                     case ActionType.FlickLeft:
                     {
                        DrawingHelper.DrawText( "FlickLeft", location, textColor );
                        break;
                     }
                     case ActionType.FlickRight:
                     {
                        DrawingHelper.DrawText( "FlickRight", location, textColor );
                        break;
                     }
                     case ActionType.Tap:
                     {
                        DrawingHelper.DrawText( "Tap", location, textColor );
                        break;
                     }
                  }

                  break;
               }
            }
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
   }
}
