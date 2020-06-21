using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.UIComponents
{
    public class PictureBox : Component
    {
        public PictureBox(Texture2D texture, Point position) : base(texture, null, position) { }
    }
}