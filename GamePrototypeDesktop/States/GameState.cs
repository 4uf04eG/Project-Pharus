using System.Collections.Generic;
using System.Linq;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.UIComponents;
using GamePrototypeDesktop.UIComponents.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.States
{
   /*
    * Base class for every game state.
    * 
    * Game state is an abstraction which represents state which game right now in.
    * States, for instance, are title menu, gameplay state
    * and even little scenes like window with characters relations.
    * There could be several loaded states in the same time.
    */
    public abstract class GameState
    {
        protected readonly ComponentContainer Children;
        protected readonly ContentManager ContentManager;
        protected readonly Camera Camera;
        
        protected GameState(Camera camera)
        {
            Camera = camera;
            ContentManager = new ContentManager(
                StateManager.Instance.ContentManager.ServiceProvider, "Content");
            Children = new ComponentContainer(ContentManager);
        }

        public virtual void Initialize() { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent()
        {
            ContentManager.Unload();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var component in Children)
                component.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in Children.ToList())
                component.Update(gameTime);
        }

        protected void AddComponent(Component component)
        {
         
            Children.Add(component);
        }

        protected T CreateComponent<T>() where T : Component, new()
        {
            var component = new T();
      
            Children.Add(component);
            return component;
        }

        protected T LoadComponent<T>(string xmlPath) where T : Component
        {
            var component = XmlManager<T>.Load(xmlPath, GetType());
            component.LoadContent(ContentManager);
            Children.Add(component);
            return component;
        }
    }
}