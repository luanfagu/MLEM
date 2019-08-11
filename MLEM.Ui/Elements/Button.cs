using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Textures;
using MLEM.Ui.Style;

namespace MLEM.Ui.Elements {
    public class Button : Element {

        public NinePatch Texture;
        public NinePatch HoveredTexture;
        public Color HoveredColor;
        public AutoScaledText Text;

        public Button(Anchor anchor, Vector2 size, string text = null) : base(anchor, size) {
            if (text != null) {
                this.Text = new AutoScaledText(Anchor.Center, Vector2.One, text) {
                    IgnoresMouse = true
                };
                this.AddChild(this.Text);
            }
        }

        public override void Draw(GameTime time, SpriteBatch batch, float alpha) {
            var tex = this.Texture;
            var color = Color.White * alpha;
            if (this.IsMouseOver) {
                if (this.HoveredTexture != null)
                    tex = this.HoveredTexture;
                color = this.HoveredColor * alpha;
            }
            batch.Draw(tex, this.DisplayArea, color, this.Scale);
            base.Draw(time, batch, alpha);
        }

        protected override void InitStyle(UiStyle style) {
            base.InitStyle(style);
            this.Texture = style.ButtonTexture;
            this.HoveredTexture = style.ButtonHoveredTexture;
            this.HoveredColor = style.ButtonHoveredColor;
        }

    }
}