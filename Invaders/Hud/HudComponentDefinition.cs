﻿using System;

namespace Invaders.Hud
{
    public class HudComponentDefinition
    {
        public HorizontalAlignment HorizontalAlignment;

        public VerticalAlignment VerticalAlignment;

        public string Name { get; set; }

        public string HAlign
        {
            set
            {
                Enum.TryParse<HorizontalAlignment>(value, out HorizontalAlignment);
            }
        }

        public string VAlign
        {
            set
            {
                Enum.TryParse<VerticalAlignment>(value, out VerticalAlignment);
            }
        }

        /// <summary>
        /// amount to shift the component by from where it is horizontally justified
        /// </summary>
        public float HorizontalPercentage { get; set; }

        /// <summary>
        /// amount to shift the component by from where it is vertically justified
        /// </summary>
        public float VerticalPercentage { get; set; }

        /// <summary>
        /// width of the component
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// height of the component
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// creates a HudComponentDefinition from the given dynamic object
        /// </summary>
        /// <param name="jsonData"></param>
        public static HudComponentDefinition Create(dynamic d)
        {
            var def = new HudComponentDefinition(); 
            def.HorizontalPercentage = (float)d["horizontalPercentage"]; 
            def.VerticalPercentage = (float)d["verticalPercentage"];
            def.HAlign = (string)d["hAlignment"];
            def.VAlign = (string)d["vAlignment"];
            def.Width = (int)d["width"];
            def.Height = (int)d["height"];
            def.Name = (string)d["name"];
            return def;
        }
    }
}
