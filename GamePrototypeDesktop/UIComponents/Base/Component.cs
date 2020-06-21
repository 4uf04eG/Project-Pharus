using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePrototypeDesktop.UIComponents.Base
{
    /*
     * Class from which derived ui elements like buttons and others.
     * 
     * It can be loaded using xml or class constructor.
     * Loading it xml requires call of function LoadContent to load graphic elements.
     */
    //TODO: Add xml support or remove FontPath and TexturePath fields
    public abstract class Component
    {
        public float Alpha;
        public string FontPath;
        public string TexturePath;
        
        public bool CanFocus;
        public bool IsVisible;
        public bool IsDisplayingBounds;
        
        public Color TextColor;
        public Gravity TextGravity;
        public Color BackgroundColor;

        public EventHandler OnMouseClick;
        public EventHandler OnMouseHover;
        public EventHandler OnMouseLeave;
        public EventHandler OnMouseWheel;
        public EventHandler OnMouseUp;
        public EventHandler OnMouseDown;
        public EventHandler OnFocus;
        //TODO: Handle last four unhandled events

        protected Texture2D Texture;
        protected SpriteFont Font;
        protected Vector2 Origin;
        protected Rectangle FullRectangle;
        protected Rectangle DrawRectangle;

        private MouseState _lastMouseState;
        private bool _isInvalidSizes;
        private bool _isSizeCalculated;
        
        private bool _isHovering;
        private bool _isFocused;
        
        private Point _position;
        private Vector2 _scale;
        private string _text;

        private LayoutSpace _padding;
        private LayoutSpace _margin;
        private int _width;
        private int _height;
        
        protected Component()
        {
            TexturePath = Text = string.Empty;
            TextColor = Color.Black;
            FontPath = "Fonts/Default";
            Padding = new LayoutSpace(3);
            Margin = new LayoutSpace(1);
            TextGravity = Gravity.None;
            Position = Point.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            CanFocus = true;
            IsVisible = true;
            _isInvalidSizes = true;
        }

        protected Component(Texture2D texture, SpriteFont spriteFont, Point position) : this()
        {
            Position = position;
            Texture = texture;
            Font = spriteFont;
        }
        
        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                _isInvalidSizes = true;
            }
        }

        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _isInvalidSizes = true;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _isInvalidSizes = true;
            }
        }
        
        public LayoutSpace Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                _isInvalidSizes = true;
            }
        }

        public LayoutSpace Margin
        {
            get => _margin;
            set
            {
                _margin = value;
                _isInvalidSizes = true;
            }
        }

        public int Width
        {
            get
            {
                if (!_isSizeCalculated)
                {
                    _isSizeCalculated = true;
                    CalculateComponentSizes();
                }

                return _width;
            }
            protected set
            {
                _width = value;
                _isInvalidSizes = true;
            }
        }

        public int Height 
        { 
            get
            {
                if (!_isSizeCalculated)
                {
                    _isSizeCalculated = true;
                    CalculateComponentSizes();
                }

                return _height;
            }
            protected set
            {
                _height = value;
                _isInvalidSizes = true;
            } 
        }
        
        public virtual void LoadContent(ContentManager contentManager)
        {
            if (!string.IsNullOrEmpty(TexturePath))
                Texture = contentManager.Load<Texture2D>(TexturePath);

            Font = contentManager.Load<SpriteFont>(FontPath);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            
            if (BackgroundColor != null)
                spriteBatch.FillRectangle(DrawRectangle, BackgroundColor);
            
            if (Texture != null) 
                spriteBatch.Draw(Texture, DrawRectangle, Color.White * Alpha);

            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                Vector2 position;
                
                if (TextGravity.HasFlag(Gravity.Left))
                    position = new Vector2(DrawRectangle.X, Origin.Y);
                else if (TextGravity.HasFlag(Gravity.Right))
                    position = new Vector2(DrawRectangle.X + DrawRectangle.Width - Font.MeasureString(Text).X, Origin.Y);
                else
                    position = Origin;
                
                spriteBatch.DrawString(Font, Text, position, TextColor * Alpha);
            }
            
            if (IsDisplayingBounds)
            {
                spriteBatch.DrawRectangle(DrawRectangle, Color.Yellow, 2);
                spriteBatch.DrawRectangle(FullRectangle, Color.Red, 2);
            }
        }

        /*
         * Method which should be called on update of state
         *
         * At default implementation in handles mouse events like on click and on hover.
         * 
         * To check if mouse hovers component if gets state of mouse and with coordinates info creates point.
         * After it checks if this point is in rectangle of component. If so it invokes the event on hover
         * and if left mouse is clicked but in previous draw it was released, invokes action on click.
         *
         * If mouse not hovers rectangle but in previous draw it have, it sets hovering flag false
         * and invokes action on leave.
         */
        public virtual void Update(GameTime gameTime)
        {
            if (_isInvalidSizes)
                CalculateComponentSizes();
            
            HandleMouseEvents();
        }

        public virtual void Invalidate()
        {
            _isInvalidSizes = true;
        }
        
        private void HandleMouseEvents()
        {
            var mouseState = Mouse.GetState();
            var mousePoint = GraphicsManager.Instance.ConvertScreenToVirtualPosition(
                new Point(mouseState.X, mouseState.Y));

            if (DrawRectangle.Contains(mousePoint))
            {
                _isHovering = true;
                OnMouseHover?.Invoke(this, new EventArgs());

                if (_lastMouseState.LeftButton == ButtonState.Released &&
                    mouseState.LeftButton == ButtonState.Pressed)
                    OnMouseClick?.Invoke(this, new EventArgs());
            }
            else if (_isHovering)
            {
                _isHovering = false;
                OnMouseLeave?.Invoke(this, new EventArgs());
            }

            _lastMouseState = mouseState;
        }

        private void CalculateComponentSizes()
        {
            var (x, y) = new Point
            {
                X = (int) Math.Max(Font?.MeasureString(Text).X ?? 0, Texture?.Width ?? 0),
                Y = (int) Math.Max(Font?.MeasureString(Text).Y ?? 0, Texture?.Height ?? 0)
            };
            var width = Width == 0 ? (int) (x * Scale.X) + Padding.Horizontal : Width;
            var height = Height == 0 ? (int) (y * Scale.Y) + Padding.Vertical : Height;
            
            DrawRectangle = new Rectangle((int) Position.X - Padding.Left, (int) Position.Y - Padding.Top,
                width, height);
            FullRectangle = new Rectangle(DrawRectangle.X - Margin.Left , DrawRectangle.Y - Margin.Top,
                DrawRectangle.Width + Margin.Horizontal, DrawRectangle.Height + Margin.Vertical);
            
            Origin.X = DrawRectangle.X + (float) DrawRectangle.Width / 2 - Font?.MeasureString(Text).X / 2 ?? 0.0f;
            Origin.Y = DrawRectangle.Y + (float) DrawRectangle.Height / 2 - Font?.MeasureString(Text).Y / 2 ?? 0.0f;
            Width = DrawRectangle.Width;
            Height = DrawRectangle.Height;
            _isInvalidSizes = false;
        }
    }
}
