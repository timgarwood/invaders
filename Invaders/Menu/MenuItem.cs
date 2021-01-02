﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Invaders.Menu
{
    public class MenuItem
    {
        public MenuItemDefinition Definition { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; private set; }

        /// <summary>
        /// the menu to load when this MenuItem is selected
        /// </summary>
        public Menu Menu { get; set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 origin)
        {
            spriteBatch.Draw(Texture, origin);
            Position = origin;
        }
    }
}
