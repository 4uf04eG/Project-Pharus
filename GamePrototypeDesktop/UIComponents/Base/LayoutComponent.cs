using System;
using System.Collections.Generic;
using GamePrototypeDesktop.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.UIComponents.Base
{
    public abstract class LayoutComponent : ParentComponent
    {
        public Orientation Orientation;
        public Gravity Gravity;
        public int Spacing; // Ignoring when gravity is fill

        private bool _isChildrenPlaced;
        
        public LayoutComponent()
        {
            Orientation = Orientation.Horizontal;
            Gravity = Gravity.None;
            Spacing = 2;
            Width = GraphicsManager.Instance.VirtualWidth;
            Height = GraphicsManager.Instance.VirtualHeight;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (!_isChildrenPlaced)
            {
                PlaceChildren();
                _isChildrenPlaced = true;
            }
        }
        
        private Point CalculateChildrenSize()
        {
            var width = 0;
            var height = 0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    width = Math.Max(width, child.Width + child.Padding.Horizontal + child.Margin.Horizontal);
                    height += child.Height + child.Padding.Horizontal + child.Margin.Vertical + Spacing;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    width += child.Width + child.Padding.Horizontal + child.Margin.Horizontal + Spacing;
                    height = Math.Max(height, child.Height + child.Padding.Horizontal + child.Margin.Vertical);
                }
            }

            return new Point(width, height);
        }
        
        private void PlaceChildren()
        {
            if (Gravity != Gravity.Fill)
            {
                var (width, height) = CalculateChildrenSize();
                var x = GetHorizontalPosition(width);
                var y = GetVerticalPosition(height);
                
                Position = new Point(Margin.Left + x, Margin.Top + y);
                Width = width;
                Height = height;
            }
        }

        private int GetHorizontalPosition(int childrenWidth)
        {
            if (Gravity.HasFlag(Gravity.Left) || Gravity.HasFlag(Gravity.Fill))
                return Position.X;
            if (Gravity.HasFlag(Gravity.Right))
                return Position.X - childrenWidth;
            if (Gravity.HasFlag(Gravity.CenterHorizontal))
                return Position.X + Width / 2 - childrenWidth / 2;

            throw new Exception("Invalid horizontal gravity");
        }

        private int GetVerticalPosition(int childrenHeight)
        {
            if (Gravity.HasFlag(Gravity.Top) || Gravity.HasFlag(Gravity.Fill))
                return Position.Y;
            if (Gravity.HasFlag(Gravity.Bottom))
                return Position.Y - childrenHeight;
            if (Gravity.HasFlag(Gravity.CenterVertical))
                return Position.Y + Height / 2 - childrenHeight / 2;

            throw new Exception("Invalid vertical gravity");
        }
        
    }
}