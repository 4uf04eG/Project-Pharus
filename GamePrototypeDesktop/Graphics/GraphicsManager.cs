using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.Graphics
{
    public class GraphicsManager
    {
        public int VirtualWidth;
        public int VirtualHeight;
        public int ScreenWidth;
        public int ScreenHeight;
        
        private static GraphicsManager _instance;
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private Camera _camera;
        
        private Vector2 _scaleRatios;
        private bool _isFullScreen;
        
        private GraphicsManager() {}
        public static GraphicsManager Instance => _instance ?? (_instance = new GraphicsManager());

        public bool IsFullScreen
        {
            get => _isFullScreen;
            set 
            {
                _isFullScreen = value;
                _graphics.IsFullScreen = value;
            }
        }
        
        public Viewport Viewport { get; private set; }

        public Game Game { get; private set; }

        public void Initialize(GraphicsDeviceManager graphics, Game game, SpriteBatch spriteBatch, Camera camera)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _camera = camera;
            Game = game;
            
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            VirtualWidth = 1920;
            VirtualHeight = 1080;
        }

        public void PrepareGraphics()
        {
            SetupFullViewport();
            _graphics.GraphicsDevice.Clear(Color.Black);
            SetupVirtualScreenViewport();
            _graphics.GraphicsDevice.Clear(Color.Bisque);
        }

        public Vector2 ConvertScreenToVirtualPosition(Point screenPosition)
        {
            return _camera.ConvertScreenToVirtual(screenPosition.ToVector2());
        }

        public void BeginDraw()
        {
            PrepareGraphics();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix:GetScaleMatrix());
        }

        public void OnWindowResize(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            _camera.OnWindowSizeChanged();
        }

        public void SetScreenResolution(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            ApplyResolutionSettings();
        }

        //I think this is redundant and should be replaced 
        private void ApplyResolutionSettings()
        {
            if (_isFullScreen)
            {
                foreach (var displayMode in _graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
                {
                    if (displayMode.Width != ScreenWidth || displayMode.Height != ScreenHeight) continue;
                    _graphics.PreferredBackBufferWidth = ScreenWidth;
                    _graphics.PreferredBackBufferHeight = ScreenHeight;
                    _graphics.ApplyChanges();
                }
            }
            else if (ScreenWidth <= _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width 
                     && ScreenHeight <= _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height)
            {
                _graphics.PreferredBackBufferWidth = ScreenWidth;
                _graphics.PreferredBackBufferHeight = ScreenHeight;
                _graphics.ApplyChanges();
            }
        }
        
        private void SetupFullViewport()
        {
            _graphics.GraphicsDevice.Viewport = new Viewport()
            {
                X = 0, Y = 0,
                Width = ScreenWidth,
                Height = ScreenHeight,
            };
        }

        private void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = (float) VirtualWidth / VirtualHeight;
            var width = ScreenWidth;
            var height = (int) (width / targetAspectRatio + 0.5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                width = (int) (height * targetAspectRatio + 0.5f);
            }

            Viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height,
            };
            _scaleRatios = new Vector2(
                (float) Viewport.Width / VirtualWidth,
                (float) Viewport.Height / VirtualHeight
            );
            _graphics.GraphicsDevice.Viewport = Viewport;
        }

        public Matrix GetScaleMatrix()
        {
            return RecreateScaleMatrix();
        }
        
        private Matrix RecreateScaleMatrix()
        {
            return Matrix.CreateScale(_scaleRatios.X, _scaleRatios.Y, 1.0f);
        }
    }
}