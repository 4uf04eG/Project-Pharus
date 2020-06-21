using System;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePrototypeDesktop.UIComponents
{
    public class TitleMenuEntry : Component
    {
        public TitleMenuEntry()
        {
            FontPath = "Fonts/MainMenuFont";
            //TexturePath = "Backgrounds/Untitled";
            TextColor = Color.White;
            TextGravity = Gravity.Center;
      
            Padding = new LayoutSpace(15);
            Margin = new LayoutSpace(15);
            IsDisplayingBounds = true;
            
            OnMouseHover += (sender, args) => Alpha = 0.5f; 
            OnMouseLeave += (sender, args) => Alpha = 1.0f;
        }
    }
}