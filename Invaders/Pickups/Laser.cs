﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.Pickups
{
    public class Laser : Pickup 
    {
        public string ProjectileName { get; private set; }
        public int StartingAmmo { get; private set; }

        public string Name { get; private set; }

        public Laser(GameData gameData,
            GameUtils gameUtils,
            World world, 
            Texture2D texture, 
            Shape shape, 
            Body rigidBody, 
            string projectileName, 
            int startingAmmo, 
            string name, 
            float scale)
            : base(gameData, gameUtils, world, texture, shape, rigidBody, scale)
        {
            ProjectileName = projectileName;
            StartingAmmo = startingAmmo;
            Name = name;
        }
    }
}
