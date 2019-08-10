using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;

namespace MLEM.Ui.Elements {
    public class Panel : Element {

        public static NinePatch DefaultTexture;
        
        private readonly NinePatch texture;

        public Panel(Anchor anchor, Vector2 size, Point positionOffset, NinePatch texture = null) : base(anchor, size) {
            this.texture = texture ?? DefaultTexture;
            this.PositionOffset = positionOffset;
            this.ChildPadding = new Point(5);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Color color) {
            batch.Draw(this.texture, this.DisplayArea, color);
            base.Draw(time, batch, color);
        }

    }
}