using System;
using System.Collections.Generic;
using System.Reflection;

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

      public void AddAction()
      {
         var addedValue = false;
         while ( !addedValue )
         {
            var values = GetValues<ActionLocation>();
            var random = new Random();
            var randomLocation = (ActionLocation) values.GetValue( random.Next( values.Length ) );

            foreach ( var a in _actions )
            {
               if ( a.ActionLocation == randomLocation )
               {
                  continue;
               }
            }

            var values2 = GetValues<ActionType>();
            var randomAction = (ActionType) values2.GetValue( random.Next( values2.Length ) );

            _actions.Add( new ActionObj { ActionLocation = randomLocation, ActionType = randomAction } );

            addedValue = true;
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
   }
}
