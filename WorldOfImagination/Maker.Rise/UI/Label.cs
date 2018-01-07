﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maker.Rise.UI
{
    public class Label : Control
    {
        public string Text { get; set; } = "Label";
        public SpriteFont Font { get; set; }

        public Label()
        {
            Font = EngineRessources.FontBebas;
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var textSize = Font.MeasureString(Text);
            spriteBatch.DrawString(Font, Text, new Vector2((int) (Bound.X + Bound.Width / 2 - textSize.X / 2),
                    (int) (Bound.Y + Bound.Height / 2 - textSize.Y / 2)),
                Color.White);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}