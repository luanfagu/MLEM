using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Font;

namespace MLEM.Ui.Elements {
    public class AutoScaledText : Element {

        private readonly IGenericFont font;
        private float scale;
        private string text;

        public string Text {
            get => this.text;
            set {
                this.text = value;
                this.SetDirty();
            }
        }
        public Color Color;

        public AutoScaledText(Anchor anchor, Vector2 size, string text, Color? color = null, IGenericFont font = null) : base(anchor, size) {
            this.Text = text;
            this.font = font ?? Paragraph.DefaultFont;
            this.Color = color ?? Color.White;
        }

        public override void ForceUpdateArea() {
            base.ForceUpdateArea();

            this.scale = 0;
            Vector2 measure;
            do {
                this.scale += 0.1F;
                measure = this.font.MeasureString(this.Text) * this.scale;
            } while (measure.X <= this.DisplayArea.Size.X && measure.Y <= this.DisplayArea.Size.Y);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Color color) {
            var pos = this.DisplayArea.Location.ToVector2() + this.DisplayArea.Size.ToVector2() / 2;
            this.font.DrawCenteredString(batch, this.Text, pos, this.scale, this.Color.CopyAlpha(color), true, true);
            base.Draw(time, batch, color);
        }

    }
}