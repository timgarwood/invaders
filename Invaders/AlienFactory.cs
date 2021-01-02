﻿using System;
using System.IO;
using Newtonsoft.Json;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NLog;
using Invaders.Animations;
using Invaders.Physics;

namespace Invaders
{
    public class AlienFactory
    {
        /// <summary>
        /// logger 
        /// </summary>
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private IList<AlienDefinition> _alienDefinitions = null;

        private World _physicsWorld;

        private ContentManager _contentManager;

        private AnimationFactory _animationFactory;

        private GameWorld GameWorld { get; set; }

        private GraphicsDevice GraphicsDevice { get; set; }

        private GameData GameData { get; set; }

        private GameUtils GameUtils { get; set; }

        private Player Player { get; set; }

        private HealthBarFactory HealthBarFactory { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="physicsWorld"></param>
        public AlienFactory(World physicsWorld, 
            GameData gameData,
            GameUtils gameUtils,
            ContentManager contentManager, 
            GameWorld gameWorld,
            AnimationFactory animationFactory, 
            GraphicsDevice graphicsDevice,
            HealthBarFactory healthBarFactory,
            Player player)
        {
            _physicsWorld = physicsWorld;
            GameData = gameData;
            GameUtils = gameUtils;
            _contentManager = contentManager;
            _animationFactory = animationFactory;
            GraphicsDevice = graphicsDevice;
            GameWorld = gameWorld;
            Player = player;
            HealthBarFactory = healthBarFactory;
        }

        /// <summary>
        /// loads alien definitions from the given json stream
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream)
        {
            using(stream)
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var defs = JsonConvert.DeserializeObject<AlienDefinition[]>(json);
                    _alienDefinitions = new List<AlienDefinition>(defs);
                }
            }
        }

        /// <summary>
        /// creates an instance of the given alien definition name
        /// </summary>
        /// <param name="name"></param>
        public Alien Create(string name)
        {
            if(_alienDefinitions != null)
            {
                var definition = _alienDefinitions.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if(definition == null)
                {
                    throw new Exception($"No Alien definition found for name {name}");
                }

                var rand = new Random((int)(DateTime.UtcNow - DateTime.MinValue).Ticks);
                var texture = _contentManager.Load<Texture2D>(definition.TextureName);
                var physicsSize = GameUtils.PhysicsVec(new Vector2(texture.Width * definition.Scale, texture.Height * definition.Scale));
                var minX = GameData.MaxXDimension * .1;
                var minY = GameData.MaxYDimension * .1;
                var maxX = GameData.MaxXDimension - minX;
                var maxY = GameData.MaxYDimension - minY;

                var loc = new Vec2(rand.Next((int)minX, (int)maxX), rand.Next((int)minY, (int)maxY));
                
                var shapeDef = new PolygonDef();
                shapeDef.Vertices = new Vec2[definition.Vertices.Length];
                for(var i = 0; i < definition.Vertices.Length; ++i)
                {
                    var x = definition.Vertices[i].X * definition.Scale;
                    var y = definition.Vertices[i].Y * definition.Scale;
                    shapeDef.Vertices[i] = GameUtils.PhysicsVec(new Vector2(x, y));
                }

                shapeDef.VertexCount = shapeDef.Vertices.Length;

                shapeDef.Density = definition.Density;
                shapeDef.Friction = definition.Friction;
                shapeDef.Filter.CategoryBits = CollisionCategory.Alien;
                shapeDef.Filter.MaskBits = (ushort)(CollisionCategory.Wall | CollisionCategory.Player | CollisionCategory.PlayerProjectile | CollisionCategory.Alien);

                var bodyDef = new BodyDef();
                bodyDef.IsBullet = true;
                bodyDef.Position.Set(loc.X, loc.Y);
                var body = _physicsWorld.CreateBody(bodyDef);
                var shape = body.CreateShape(shapeDef);

                body.SetMassFromShapes();

                var healthBar = HealthBarFactory.Create("AlienHealthBar", 100, 5);

                var gameObject = new Alien(_contentManager, _physicsWorld, GameData, GameUtils, definition, _animationFactory, GameWorld, texture, shape, body, Player, healthBar);
                GameWorld.AddGameObject(gameObject);
                return gameObject;
            }

            throw new Exception("Uninitialized definitions");
        }
    }
}
