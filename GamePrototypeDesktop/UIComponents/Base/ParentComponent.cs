using System;
using System.Collections.Generic;
using System.Linq;
using GamePrototypeDesktop.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.UIComponents.Base
{
    public class ParentComponent : Component
    {
        public readonly ComponentContainer Children;

        public ParentComponent()
        {
            Children = new ComponentContainer(StateManager.Instance.ContentManager);
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
         
            foreach (var component in Children)
                component.LoadContent(contentManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var component in Children)
                component.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var component in Children)
                component.Update(gameTime);
        }
        
    }
}