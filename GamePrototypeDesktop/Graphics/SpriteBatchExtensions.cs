using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.Graphics
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D _baseTexture;
        
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            var length = (end - start).Length();
            var rotation = (float) Math.Atan2(end.Y - start.Y, end.X - start.X);
            spriteBatch.Draw(_baseTexture, start, null, color, rotation, 
                Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle bounds, Color color, int thickness = 1)
        {
            var texture = GetBaseTexture(spriteBatch);
            var topLeftHorizontal = new Rectangle(bounds.Left, bounds.Top, bounds.Width, thickness);
            var topLeftVertical = new Rectangle(bounds.Left, bounds.Top, thickness, bounds.Height);
            var bottomLeft = new Rectangle(bounds.Left, bounds.Bottom - thickness, bounds.Width, thickness);
            var topRight = new Rectangle(bounds.Right - thickness, bounds.Top, thickness, bounds.Height);

            spriteBatch.Draw(texture, topLeftHorizontal, color);
            spriteBatch.Draw(texture, topLeftVertical, color);
            spriteBatch.Draw(texture, bottomLeft, color);
            spriteBatch.Draw(texture, topRight, color);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            spriteBatch.Draw(GetBaseTexture(spriteBatch), bounds, color);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            
        }

        private static Texture2D GetBaseTexture(GraphicsResource resource)
        {
            if (_baseTexture == null)
            {
                _baseTexture = new Texture2D(resource.GraphicsDevice, 1, 1);
                _baseTexture.SetData(new [] {Color.White});
            }

            return _baseTexture;
        }
    }
}