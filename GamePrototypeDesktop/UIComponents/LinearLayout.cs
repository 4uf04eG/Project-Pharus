using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;

namespace GamePrototypeDesktop.UIComponents
{
    public class LinearLayout : LayoutComponent
    {
        public LinearLayout(Orientation orientation)
        {
            Orientation = orientation;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}