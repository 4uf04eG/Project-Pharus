using System;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;

namespace GamePrototypeDesktop.UIComponents
{
    public class Dialog : ParentComponent
    {
        public EventHandler OnOkClick;
        public EventHandler OnCancelClick;
        
        public Dialog()
        {
            Position = new Point(GraphicsManager.Instance.VirtualWidth / 2,
                GraphicsManager.Instance.VirtualHeight / 2);
            BackgroundColor = Color.Aqua;
            Padding = new LayoutSpace(0, 0, 0, 60);

            var btn1 = new Button
            {
                Text = "Да",
                Position = new Point(GraphicsManager.Instance.VirtualWidth / 2 + 20,
                    GraphicsManager.Instance.VirtualHeight / 2 + 50)
            };
            btn1.OnMouseClick += (sender, args) => OnOkClick.Invoke(this, new EventArgs());
            Children.Add(btn1);
            
            var btn2 = new Button
            {
                Text = "Нет",
                Position = new Point(GraphicsManager.Instance.VirtualWidth / 2 + 70,
                    GraphicsManager.Instance.VirtualHeight / 2 + 50)
            };
            btn2.OnMouseClick += (sender, args) => OnCancelClick.Invoke(this, new EventArgs());
            Children.Add(btn2);
        }
    }
}