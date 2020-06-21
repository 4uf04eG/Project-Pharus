using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GamePrototypeDesktop.UIComponents.Base
{
    public class ComponentContainer : IEnumerable<Component>
    {
        private readonly Dictionary<string, Component> _components;
        private readonly ContentManager _contentManager;

        public ComponentContainer(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _components = new Dictionary<string, Component>();
        } 

        public void Add(Component component, string tag = "")
        {
            component.LoadContent(_contentManager);

            if (string.IsNullOrEmpty(tag))
                tag = $"{component}|{component.GetHashCode()}";

            if (!_components.ContainsKey(tag))
                _components.Add(tag, component);
            else
                throw new Exception("Tried to add duplicate component tag");
        }

        public void Remove(string tag)
        {
            _components.Remove(tag);
        }
        
        public Component this[string str]
        {
            get => _components[str];
            set => _components[str] = value;
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return _components.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}