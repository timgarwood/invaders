﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Box2DX.Common;

namespace Invaders
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, Vec2 cameraOrigin, Vector2 viewport);
    }
}
