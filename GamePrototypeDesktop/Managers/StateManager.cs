using System;
using System.Collections.Generic;
using System.Linq;
using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePrototypeDesktop.Managers
{
    /*
     * Class for handling state management.
     *
     * Definition of state is given in States/GameState.cs.
     * It uses singleton pattern to store game states and content manager
     * which could be accessed from anywhere.
     */
    public class StateManager
    {
        private static StateManager _instance;
        private readonly List<GameState> _gameStates;
        private Camera _camera;
        
        private StateManager() => _gameStates = new List<GameState>();
        
        public ContentManager ContentManager { private set; get; }
        
        public static StateManager Instance => _instance ?? (_instance = new StateManager());

        public void LoadContent(ContentManager contentManager, Camera camera)
        {
            ContentManager = contentManager;
            _camera = camera;
        }

        public void UnloadContent()
        {
            foreach (var state in _gameStates)
                state.UnloadContent();
        }

        public void AddState(GameState gameState)
        {
            _gameStates.Add(gameState);
            gameState.Initialize();
            
            if (ContentManager != null)
                gameState.LoadContent();
            else
                throw new ArgumentException("Content manager is not specified");
        }
 
        public void RemoveState()
        {
            if (_gameStates.Count != 0)
            {
                GetCurrentState().UnloadContent();
                _gameStates.RemoveAt(_gameStates.Count - 1);
            }
        }

        // Maybe in future it's will be needed to draw not only one screen.
        // And seems like it doesn't redraw other states so they just disappear
        // TODO: I think better draw every state, but update only last one
        public void Draw(SpriteBatch spriteBatch)
        {
            GraphicsManager.Instance.PrepareGraphics();
            spriteBatch.Begin(transformMatrix:_camera.GetViewMatrix());
            
            foreach (var state in _gameStates)
                state.Draw(spriteBatch);
            
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (_gameStates.Count != 0)
                GetCurrentState().Update(gameTime);
        }

        public GameState GetCurrentState() => _gameStates[_gameStates.Count - 1];

        public List<GameState> GetAllStates() => _gameStates;
    }
}