using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Font;
using MonoGame.Extended.BitmapFonts;

namespace MLEM.Extended.Font {
    public class GenericBitmapFont : IGenericFont {

        public readonly BitmapFont Font;

        public GenericBitmapFont(BitmapFont font) {
            this.Font = font;
        }

        public static implicit operator GenericBitmapFont(BitmapFont font) {
            return new GenericBitmapFont(font);
        }

        public Vector2 MeasureString(string text) {
            return this.Font.MeasureString(text);
        }

        public Vector2 MeasureString(StringBuilder text) {
            return this.Font.MeasureString(text);
        }

        public void DrawString(SpriteBatch batch, string text, Vector2 position, Color color) {
            batch.DrawString(this.Font, text, position, color);
        }

        public void DrawString(SpriteBatch batch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            batch.DrawString(this.Font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteBatch batch, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            batch.DrawString(this.Font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color) {
            batch.DrawString(this.Font, text, position, color);
        }

        public void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            batch.DrawString(this.Font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            batch.DrawString(this.Font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

    }
}