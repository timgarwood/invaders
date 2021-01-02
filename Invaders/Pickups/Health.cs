﻿using Box2DX.Dynamics;
using Box2DX.Collision;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Box2DX.Common;
using Microsoft.Xna.Framework;
using System;

namespace Invaders.Pickups
{
    public class Health : Pickup
    {
        public Health(
            GameData gameData,
            GameUtils gameUtils,
            World _world, 
            Texture2D _texture, 
            Shape _shape, 
            Body _rigidBody,
            int hp,
            float scale) : base(gameData, gameUtils, _world, _texture, _shape, _rigidBody, scale)
        {
            Hp = hp;
        }

        public int Hp { get; private set; }
    }
}
