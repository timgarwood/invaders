﻿using Invaders.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders.Hud
{
    public class LivesRemaining : HudComponent
    {
        private Player Player { get; set; }

        private Font Font { get; set; }

        private string TextTemplate { get; set; }

        public LivesRemaining(HudComponentDefinition definition,
            Player player,
            Font font,
            string textTemplate) : base(definition)
        {
            Player = player;
            Font = font;
            TextTemplate = textTemplate;
        }

        public static LivesRemaining CreateFromData(dynamic jsonData,
            ContentManager contentManager,
            GraphicsDevice graphicsDevice,
            WeaponInventory weaponInventory,
            GameWorld gameWorld,
            GameData gameData,
            HealthBarFactory healthBarFactory,
            Player player)
        {
            var hudComponentDefinition = HudComponentDefinition.Create(jsonData);

            var textTemplate = (string)jsonData["textTemplate"];
            var fontName = (string)jsonData["fontName"];

            var font = FontFactory.Instance.GetFont(fontName);

            return new LivesRemaining(hudComponentDefinition, player, font, textTemplate);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 viewport)
        {
            var text = TextTemplate.Replace("{livesRemaining}", Player.LivesRemaining.ToString());
            Font.DrawString(spriteBatch, text, Location);
        }
    }
}
