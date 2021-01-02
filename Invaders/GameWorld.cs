﻿using System;
using System.Linq;
using System.Collections.Generic;
using Box2DX.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq.Expressions;
using Invaders.Pickups;
using Invaders.Weapons;

namespace Invaders
{
    public class GameWorld : IDrawable
    {
        /// <summary>
        /// list of game objects
        /// </summary>
        private List<GameObject> _gameObjects;
        private List<GameObject> _pendingAddGameObjects;
        private List<GameObject> _pendingRemoveGameObjects;

        private List<Action<GameObject>> _gameObjectAdded;
        private List<Action<GameObject>> _gameObjectRemoved;

        private bool Updating { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public GameWorld()
        {
            _gameObjectAdded = new List<Action<GameObject>>();
            _gameObjectRemoved = new List<Action<GameObject>>();
            _gameObjects = new List<GameObject>();
            _pendingAddGameObjects = new List<GameObject>();
            _pendingRemoveGameObjects = new List<GameObject>();
        }

        /// <summary>
        /// adds the game object to the game world
        /// </summary>
        /// <param name="obj"></param>
        public void AddGameObject(GameObject obj)
        {
            if(Updating)
            {
                if (!_pendingAddGameObjects.Contains(obj))
                {
                    _pendingAddGameObjects.Add(obj);
                }
            }
            else if (!_gameObjects.Contains(obj))
            {
                _gameObjects.Add(obj);
                _gameObjectAdded.ForEach(a => a.Invoke(obj));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            obj.Active = false;
            if(Updating)
            {
                if (!_pendingRemoveGameObjects.Contains(obj))
                {
                    _pendingRemoveGameObjects.Add(obj);
                }
            }
            else if(_gameObjects.Contains(obj))
            {
                _gameObjects.Remove(obj);
                obj.Dispose();
                _gameObjectRemoved.ForEach(a => a.Invoke(obj));
            }
        }

        /// <summary>
        /// Returns a list of game objects that are of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetGameObjects<T>()
        {
            //TODO:  this might be super inefficient
            return _gameObjects.OfType<T>().ToList();
        }

        /// <summary>
        /// get all objects that satisfy a condition
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public ICollection<GameObject> GetAll(Func<GameObject, bool> expr)
        {
            return _gameObjects.Where(expr).ToList();
        }

        /// <summary>
        /// remove all objects from the game
        /// </summary>
        public void SetUpForNewGame()
        {
            var whitelist = new Type[] { typeof(Alien), typeof(Health), typeof(Laser), typeof(Projectile) };
            var objs = _gameObjects.Where(x =>
            {
                return whitelist.Contains(x.GetType());
            }).ToList();

            objs.ForEach(x =>
            {
                if (!x.PendingDispose)
                {
                    x.Dispose();
                    _gameObjects.Remove(x);
                }
            });

        }

        /// <summary>
        /// registers the given callback for knowing when a game object is added
        /// </summary>
        /// <param name="a"></param>
        public void RegisterGameObjectAdded(Action<GameObject> a)
        {
            if (!_gameObjectAdded.Contains(a))
            {
                _gameObjectAdded.Add(a);
            }
        }

        /// <summary>
        /// registers the given callback for knowing when a game object is added
        /// </summary>
        /// <param name="a"></param>
        public void RegisterGameObjectRemoved(Action<GameObject> a)
        {
            if(!_gameObjectRemoved.Contains(a))
            {
                _gameObjectRemoved.Add(a);
            }
        }

        /// <summary>
        /// draw routine, draws all the registered game objects
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="cameraOrigin"></param>
        /// <param name="viewport"></param>
        public void Draw(SpriteBatch spriteBatch, Vec2 cameraOrigin, Vector2 viewport)
        {
            foreach(var obj in _gameObjects)
            {
                obj.DrawShadow(spriteBatch, cameraOrigin);
            }

            foreach(var obj in _gameObjects)
            {
                obj.Draw(spriteBatch, cameraOrigin, viewport);
            }

        }

        /// <summary>
        /// Update all the objects in the world
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Updating = true;
            foreach(var obj in _gameObjects)
            {
                obj.Update(gameTime);
            }
            Updating = false;

            _gameObjects.AddRange(_pendingAddGameObjects);
            _pendingAddGameObjects.Clear();
            foreach(var obj in _pendingRemoveGameObjects)
            {
                obj.Dispose();
            }

            _gameObjects.RemoveAll(x => _pendingRemoveGameObjects.Contains(x));
            _pendingRemoveGameObjects.Clear();
        }
    }
}
