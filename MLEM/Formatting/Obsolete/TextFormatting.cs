using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Font;
using MLEM.Misc;
using MLEM.Textures;

namespace MLEM.Formatting {
    [Obsolete("Use the new text formatting system in MLEM.Formatting instead")]
    public static class TextFormatting {

        public static readonly Dictionary<string, FormattingCode> FormattingCodes = new Dictionary<string, FormattingCode>(StringComparer.OrdinalIgnoreCase);
        private static Regex formatRegex;

        static TextFormatting() {
            SetFormatIndicators('[', ']');

            // style codes
            FormattingCodes["regular"] = new FormattingCode(TextStyle.Regular);
            FormattingCodes["italic"] = new FormattingCode(TextStyle.Italic);
            FormattingCodes["bold"] = new FormattingCode(TextStyle.Bold);
            FormattingCodes["shadow"] = new FormattingCode(TextStyle.Shadow);

            // color codes
            var colors = typeof(Color).GetProperties();
            foreach (var color in colors) {
                if (color.GetGetMethod().IsStatic)
                    FormattingCodes[color.Name] = new FormattingCode((Color) color.GetValue(null));
            }

            // animations
            FormattingCodes["unanimated"] = new FormattingCode(TextAnimation.Default);
            FormattingCodes["wobbly"] = new FormattingCode(TextAnimation.Wobbly);
            FormattingCodes["typing"] = new FormattingCode(TextAnimation.Typing);
        }

        public static void SetFormatIndicators(char opener, char closer) {
            // escape the opener and closer so that any character can be used
            var op = "\\" + opener;
            var cl = "\\" + closer;
            // find any text that is surrounded by the opener and closer
            formatRegex = new Regex($"{op}[^{op}{cl}]*{cl}");
        }

        public static string RemoveFormatting(this string s, GenericFont font) {
            return formatRegex.Replace(s, match => {
                var code = FromMatch(match);
                return code != null ? code.GetReplacementString(font) : match.Value;
            });
        }

        public static FormattingCodeCollection GetFormattingCodes(this string s, GenericFont font) {
            var codes = new FormattingCodeCollection();
            var codeLengths = 0;
            foreach (Match match in formatRegex.Matches(s)) {
                var code = FromMatch(match);
                if (code == null)
                    continue;
                var index = match.Index - codeLengths;
                var data = new FormattingCodeData(code, match, index);
                if (codes.TryGetValue(index, out var curr)) {
                    curr.Add(data);
                } else {
                    codes.Add(index, new List<FormattingCodeData> {data});
                }
                codeLengths += match.Length - code.GetReplacementString(font).Length;
            }
            return codes;
        }

        public static void DrawFormattedString(this GenericFont regularFont, SpriteBatch batch, Vector2 pos, string unformattedText, FormattingCodeCollection formatting, Color color, float scale, GenericFont boldFont = null, GenericFont italicFont = null, float depth = 0, TimeSpan timeIntoAnimation = default, FormatSettings formatSettings = null) {
            var settings = formatSettings ?? FormatSettings.Default;
            var currColor = color;
            var currFont = regularFont;
            var currStyle = TextStyle.Regular;
            var currAnim = TextAnimation.Default;
            var animStart = 0;

            var innerOffset = new Vector2();
            var formatIndex = 0;
            foreach (var c in unformattedText) {
                // check if the current character's index has a formatting code
                if (formatting.TryGetValue(formatIndex, out var codes)) {
                    foreach (var data in codes) {
                        var code = data.Code;
                        // if so, apply it
                        switch (code.CodeType) {
                            case FormattingCode.Type.Color:
                                currColor = code.Color.CopyAlpha(color);
                                break;
                            case FormattingCode.Type.Style:
                                switch (code.Style) {
                                    case TextStyle.Regular:
                                        currFont = regularFont;
                                        break;
                                    case TextStyle.Bold:
                                        currFont = boldFont ?? regularFont;
                                        break;
                                    case TextStyle.Italic:
                                        currFont = italicFont ?? regularFont;
                                        break;
                                }
                                currStyle = code.Style;
                                break;
                            case FormattingCode.Type.Icon:
                                code.Icon.SetTime(timeIntoAnimation.TotalSeconds * code.Icon.SpeedMultiplier % code.Icon.TotalTime);
                                batch.Draw(code.Icon.CurrentRegion, new RectangleF(pos + innerOffset, new Vector2(regularFont.LineHeight * scale)), color, 0, Vector2.Zero, SpriteEffects.None, depth);
                                break;
                            case FormattingCode.Type.Animation:
                                currAnim = code.Animation;
                                animStart = formatIndex;
                                break;
                        }
                    }
                }

                var cSt = c.ToString();
                if (c == '\n') {
                    innerOffset.X = 0;
                    innerOffset.Y += regularFont.LineHeight * scale;
                } else {
                    if (currStyle == TextStyle.Shadow)
                        currAnim(settings, currFont, batch, unformattedText, formatIndex, animStart, cSt, pos + innerOffset + settings.DropShadowOffset * scale, settings.DropShadowColor, scale, depth, timeIntoAnimation);
                    currAnim(settings, currFont, batch, unformattedText, formatIndex, animStart, cSt, pos + innerOffset, currColor, scale, depth, timeIntoAnimation);
                    innerOffset.X += regularFont.MeasureString(cSt).X * scale;
                    formatIndex++;
                }
            }
        }

        private static FormattingCode FromMatch(Capture match) {
            var rawCode = match.Value.Substring(1, match.Value.Length - 2);
            FormattingCodes.TryGetValue(rawCode, out var val);
            return val;
        }

    }
}