using System;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MouseCursor = Microsoft.Xna.Framework.Input.MouseCursor;

namespace GamePrototypeDesktop
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera _mainCamera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _mainCamera = new Camera();
            Content.RootDirectory = "Content";
            //_graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Project Pharus";
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (sender, args) =>
                GraphicsManager.Instance.OnWindowResize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width - 100;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height - 100;
            _graphics.ApplyChanges();
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsManager.Instance.Initialize(_graphics, this, _spriteBatch, _mainCamera);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            StateManager.Instance.LoadContent(Content, _mainCamera);
            StateManager.Instance.AddState(new MainMenu(_mainCamera));
            var texture = Content.Load<Texture2D>("cursor");
            Mouse.SetCursor(MouseCursor.FromTexture2D(texture, 0, 0));
        }

        protected override void UnloadContent()
        {
            StateManager.Instance.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            */
            StateManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            StateManager.Instance.Draw(new SpriteBatch(GraphicsDevice));
            base.Draw(gameTime);
        }
    }
}