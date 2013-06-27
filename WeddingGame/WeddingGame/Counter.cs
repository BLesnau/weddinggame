namespace WeddingGame
{
   public static class Counter
   {
      public static int Count = 0;

      public static void Add()
      {
         Count++;
      }

      public static void Reset()
      {
         Count = 0;
      }
   }
}
