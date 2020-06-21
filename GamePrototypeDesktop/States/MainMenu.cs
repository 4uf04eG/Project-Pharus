using System;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.UIComponents;
using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.States
{
    public class MainMenu : GameState
    {
        private TitleMenuEntry _titleMenuEntry;
        private LinearLayout _layout;
        private Texture2D _background;

        public MainMenu(Camera camera) : base(camera) { }

        public override void Initialize()
        {
            base.Initialize();
            
            _layout = new LinearLayout(Orientation.Vertical);
            _layout.Gravity = Gravity.Center;
            Children.Add(_layout);
            _titleMenuEntry = new TitleMenuEntry();
            
            _layout.Children.Add(new TitleMenuEntry
            {
                Text = "Новая игра",
            });
            
            _layout.Children.Add(new TitleMenuEntry()
            {
                Text = "Загрузить игру",
            });
            _layout.Children.Add(new TitleMenuEntry()
            {
                Text = "Настройки",
            });
            _layout.Children.Add(new TitleMenuEntry()
            {
                Text = "Выход",
                OnMouseClick = (sender, args) => StateManager.Instance.AddState(new ClosingDialog(Camera))
            });
        }

        public override void LoadContent()
        {
            _background = ContentManager.Load<Texture2D>("Black-liquid");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}