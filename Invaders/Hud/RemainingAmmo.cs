﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invaders.Fonts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Invaders.Hud
{
    public class RemainingAmmo : HudComponent
    {
        private string TextTemplate { get; set; }

        private Font Font { get; set; }

        private WeaponInventory WeaponInventory { get; set; }
        private RemainingAmmo(HudComponentDefinition definition, Font font, string textTemplate, WeaponInventory weaponInventory) :
            base(definition)
        {
            Font = font;
            TextTemplate = textTemplate;
            WeaponInventory = weaponInventory;
        }

        public static RemainingAmmo CreateFromData(dynamic jsonData, 
            ContentManager contentManager, 
            GraphicsDevice graphicsDevice, 
            WeaponInventory weaponInventory, 
            GameWorld gameWorld,
            GameData gameData,
            HealthBarFactory healthBarFactory,
            Player player)
        {
            var definition = HudComponentDefinition.Create(jsonData);

            var textTemplate = (string) jsonData["textTemplate"];
            var fontName = (string)jsonData["fontName"];

            var font = FontFactory.Instance.GetFont(fontName);

            return new RemainingAmmo(definition, font, textTemplate, weaponInventory);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 viewport)
        {
            var selectedWeapon = WeaponInventory.GetSelectedWeapon();
            var ammo = "NA";
            if(selectedWeapon != null)
            {
                ammo = selectedWeapon.RemainingAmmo.ToString();
            }
            var text = TextTemplate.Replace("{remainingAmmo}", ammo);
            Font.DrawString(spriteBatch, text, Location);
        }
    }
}
