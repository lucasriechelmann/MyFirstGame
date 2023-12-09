﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Enums;
using MyFirstGame.States;
using MyFirstGame.States.Base;

namespace MyFirstGame
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;
        BaseGameState _currentGameState;
        private const int DESIGNED_RESOLUTION_WIDTH = 640;
        private const int DESIGNED_RESOLUTION_HEIGHT = 480;
        private const float DESIGNED_RESOLUTIONS_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float) DESIGNED_RESOLUTION_HEIGHT;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice,
                DESIGNED_RESOLUTION_WIDTH,
                DESIGNED_RESOLUTION_HEIGHT,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            _renderScaleRectangle = GetScaleRectangle();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            SwitchGameState(new SplashState());
        }
        protected override void UnloadContent()
        {
            _currentGameState?.UnloadContent(Content);
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentGameState?.HandleInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Render to the render target
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _currentGameState.Render(_spriteBatch);
            
            _spriteBatch.End();

            //Now render the scaled content
            _graphics.GraphicsDevice.SetRenderTarget(null);
            _graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

            if (actualAspectRatio < DESIGNED_RESOLUTIONS_ASPECT_RATIO)
            {
                var presentHeight = (int)(Window.ClientBounds.Width / DESIGNED_RESOLUTIONS_ASPECT_RATIO + variance);
                var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

                return new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }

            var presentWidth = (int)(Window.ClientBounds.Height / DESIGNED_RESOLUTIONS_ASPECT_RATIO + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

            return new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }
        private void SwitchGameState(BaseGameState gameState)
        {
            if(_currentGameState is not null)
            {
                _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
                _currentGameState.OnEventNotification -= CurrentGameState_OnEventNotification;
                _currentGameState?.UnloadContent(Content);
            }
            
            _currentGameState = gameState;
            _currentGameState.LoadContent(Content);
            _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += CurrentGameState_OnEventNotification;


        }
        private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
        {
            SwitchGameState(e);
        }
        private void CurrentGameState_OnEventNotification(object sender, Events e)
        {
            switch (e)
            {
                case Events.GAME_QUIT:
                    Exit();
                    break;
            }
        }
    }
}