using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MLEM.Font {
    /// <summary>
    /// Represents a font with additional abilities.
    /// <seealso cref="GenericSpriteFont"/>
    /// </summary>
    public abstract class GenericFont {

        /// <summary>
        /// The bold version of this font.
        /// </summary>
        public abstract GenericFont Bold { get; }

        /// <summary>
        /// The italic version of this font.
        /// </summary>
        public abstract GenericFont Italic { get; }

        ///<inheritdoc cref="SpriteFont.LineSpacing"/>
        public abstract float LineHeight { get; }

        ///<inheritdoc cref="SpriteFont.MeasureString(string)"/>
        public abstract Vector2 MeasureString(string text);

        ///<inheritdoc cref="SpriteFont.MeasureString(StringBuilder)"/>
        public abstract Vector2 MeasureString(StringBuilder text);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, string text, Vector2 position, Color color);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

        ///<inheritdoc cref="SpriteBatch.DrawString(SpriteFont,string,Vector2,Color,float,Vector2,float,SpriteEffects,float)"/>
        public abstract void DrawString(SpriteBatch batch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

        /// <summary>
        /// Returns whether this generic font supports the given character 
        /// </summary>
        /// <param name="c">The character</param>
        /// <returns>Whether this generic font supports the character</returns>
        public abstract bool HasCharacter(char c);

        /// <summary>
        /// Draws a string with the given text alignment.
        /// </summary>
        /// <param name="batch">The sprite batch to use</param>
        /// <param name="text">The string to draw</param>
        /// <param name="position">The position of the top left corner of the string</param>
        /// <param name="align">The alignment to use</param>
        /// <param name="color">The color to use</param>
        public void DrawString(SpriteBatch batch, string text, Vector2 position, TextAlign align, Color color) {
            this.DrawString(batch, text, position, align, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }

        ///<inheritdoc cref="DrawString(SpriteBatch,string,Vector2,TextAlign,Color)"/>
        public void DrawString(SpriteBatch batch, string text, Vector2 position, TextAlign align, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            this.DrawString(batch, text, position, align, color, rotation, origin, new Vector2(scale), effects, layerDepth);
        }

        ///<inheritdoc cref="DrawString(SpriteBatch,string,Vector2,TextAlign,Color)"/>
        public void DrawString(SpriteBatch batch, string text, Vector2 position, TextAlign align, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) {
            switch (align) {
                case TextAlign.Center:
                case TextAlign.CenterBothAxes:
                    var (w, h) = this.MeasureString(text);
                    position.X -= w / 2;
                    if (align == TextAlign.CenterBothAxes)
                        position.Y -= h / 2;
                    break;
                case TextAlign.Right:
                    position.X -= this.MeasureString(text).X;
                    break;
            }
            this.DrawString(batch, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Truncates a string to a given width. If the string's displayed area is larger than the maximum width, the string is cut off.
        /// Optionally, the string can be cut off a bit sooner, adding the <see cref="ellipsis"/> at the end instead.
        /// </summary>
        /// <param name="text">The text to truncate</param>
        /// <param name="width">The maximum width, in display pixels based on the font and scale</param>
        /// <param name="scale">The scale to use for width measurements</param>
        /// <param name="fromBack">If the string should be truncated from the back rather than the front</param>
        /// <param name="ellipsis">The characters to add to the end of the string if it is too long</param>
        /// <returns>The truncated string, or the same string if it is shorter than the maximum width</returns>
        public string TruncateString(string text, float width, float scale, bool fromBack = false, string ellipsis = "") {
            var total = new StringBuilder();
            var ellipsisWidth = this.MeasureString(ellipsis).X * scale;
            for (var i = 0; i < text.Length; i++) {
                if (fromBack) {
                    total.Insert(0, text[text.Length - 1 - i]);
                } else {
                    total.Append(text[i]);
                }

                if (this.MeasureString(total).X * scale + ellipsisWidth >= width) {
                    if (fromBack) {
                        return total.Remove(0, 1).Insert(0, ellipsis).ToString();
                    } else {
                        return total.Remove(total.Length - 1, 1).Append(ellipsis).ToString();
                    }
                }
            }
            return total.ToString();
        }

        /// <summary>
        /// Splits a string to a given maximum width, adding newline characters between each line.
        /// Also splits long words.
        /// </summary>
        /// <param name="text">The text to split into multiple lines</param>
        /// <param name="width">The maximum width that each line should have</param>
        /// <param name="scale">The scale to use for width measurements</param>
        /// <returns>The split string, containing newline characters at each new line</returns>
        public string SplitString(string text, float width, float scale) {
            var total = new StringBuilder();
            foreach (var line in text.Split('\n')) {
                var curr = new StringBuilder();
                foreach (var word in line.Split(' ')) {
                    if (this.MeasureString(word).X * scale >= width) {
                        if (curr.Length > 0) {
                            total.Append(curr).Append('\n');
                            curr.Clear();
                        }
                        var wordBuilder = new StringBuilder();
                        for (var i = 0; i < word.Length; i++) {
                            wordBuilder.Append(word[i]);
                            if (this.MeasureString(wordBuilder.ToString()).X * scale >= width) {
                                total.Append(wordBuilder.ToString(0, wordBuilder.Length - 1)).Append('\n');
                                wordBuilder.Remove(0, wordBuilder.Length - 1);
                            }
                        }
                        curr.Append(wordBuilder).Append(' ');
                    } else {
                        curr.Append(word).Append(' ');
                        if (this.MeasureString(curr.ToString()).X * scale >= width) {
                            var len = curr.Length - word.Length - 1;
                            if (len > 0) {
                                total.Append(curr.ToString(0, len)).Append('\n');
                                curr.Remove(0, len);
                            }
                        }
                    }
                }
                total.Append(curr).Append('\n');
            }
            return total.ToString(0, total.Length - 2);
        }

        /// <summary>
        /// Returns a string made up of the given content characters that is the given length long when displayed.
        /// </summary>
        /// <param name="width">The width that the string should have if the scale is 1</param>
        /// <param name="content">The content that the string should contain. Defaults to a space.</param>
        /// <returns></returns>
        public string GetWidthString(float width, char content = ' ') {
            var strg = content.ToString();
            while (this.MeasureString(strg).X < width)
                strg += content;
            return strg;
        }

    }

    /// <summary>
    /// An enum that represents the text alignment options for <see cref="GenericFont.DrawString(SpriteBatch,string,Vector2,TextAlign,Color)"/>
    /// </summary>
    public enum TextAlign {

        /// <summary>
        /// The text is aligned as normal
        /// </summary>
        Left,
        /// <summary>
        /// The position passed represents the center of the resulting string in the x axis
        /// </summary>
        Center,
        /// <summary>
        /// The position passed represents the right edge of the resulting string
        /// </summary>
        Right,
        /// <summary>
        /// The position passed represents the center of the resulting string, both in the x and y axes
        /// </summary>
        CenterBothAxes

    }
}