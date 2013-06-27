using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using WeddingGame.Engine;
using WeddingGame.Levels;

namespace WeddingGame
{
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class Game1 : Microsoft.Xna.Framework.Game
   {
      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;

      public Game1()
      {
         graphics = new GraphicsDeviceManager( this );
         graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
         graphics.IsFullScreen = true;

         Content.RootDirectory = "Content";

         // Frame rate is 30 fps by default for Windows Phone.
         TargetElapsedTime = TimeSpan.FromTicks( 333333 );

         // Extend battery life under lock.
         InactiveSleepTime = TimeSpan.FromSeconds( 1 );

         TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick | GestureType.HorizontalDrag | GestureType.VerticalDrag;
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here

         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch( GraphicsDevice );

         DrawingHelper.Init( spriteBatch, graphics, Content );

         var startScreen = new StartScreen();
         var instructions = new InstructionsScreen();
         var mainGame = new MainGame();
         var lost= new LostScreen();
         var won = new WonScreen();
         VPLevelManager.Init( new List<VPLevel>
         {
            startScreen, instructions, mainGame, lost, won
         } );
         VPLevelManager.LoadContent();
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// all content.
      /// </summary>
      protected override void UnloadContent()
      {
         VPLevelManager.UnloadContent();
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update( GameTime gameTime )
      {
         // Allows the game to exit
         if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
            this.Exit();

         VPLevelManager.Update( gameTime );

         base.Update( gameTime );
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw( GameTime gameTime )
      {
         GraphicsDevice.Clear( Color.CornflowerBlue );

         DrawingHelper.BeginDraw();

         VPLevelManager.Draw( gameTime );

         DrawingHelper.EndDraw();

         base.Draw( gameTime );
      }
   }
}
