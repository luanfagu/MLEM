using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MLEM.Textures;
using MLEM.Ui.Style;

namespace MLEM.Ui.Elements {
    /// <summary>
    /// A checkbox element to use inside of a <see cref="UiSystem"/>.
    /// A checkbox can be checked by pressing it and will stay checked until it is pressed again.
    /// For a checkbox that causes neighboring checkboxes to be deselected automatically, use <see cref="RadioButton"/>.
    /// </summary>
    public class Checkbox : Element {

        /// <summary>
        /// The texture that this checkbox uses for drawing.
        /// </summary>
        public StyleProp<NinePatch> Texture;
        /// <summary>
        /// The texture that this checkbox uses when it is hovered.
        /// If this is null, the default <see cref="Texture"/> is used.
        /// </summary>
        public StyleProp<NinePatch> HoveredTexture;
        /// <summary>
        /// The color that this checkbox uses for drawing when it is hovered.
        /// </summary>
        public StyleProp<Color> HoveredColor;
        /// <summary>
        /// The texture that the checkbox uses when it <see cref="IsDisabled"/>.
        /// If this is null, it uses its default <see cref="Texture"/>.
        /// </summary>
        public StyleProp<NinePatch> DisabledTexture;
        /// <summary>
        /// The color that the checkbox uses for drawing when it <see cref="IsDisabled"/>.
        /// </summary>
        public StyleProp<Color> DisabledColor;
        /// <summary>
        /// The texture that is rendered on top of this checkbox when it is <see cref="Checked"/>.
        /// </summary>
        public StyleProp<TextureRegion> Checkmark;
        /// <summary>
        /// The label <see cref="Paragraph"/> that displays next to this checkbox
        /// </summary>
        public Paragraph Label;
        /// <summary>
        /// The width of the space between this checkbox and its <see cref="Label"/>
        /// </summary>
        public StyleProp<float> TextOffsetX;
        /// <summary>
        /// Whether or not this checkbox is currently checked.
        /// </summary>
        public bool Checked {
            get => this.isChecked;
            set {
                if (this.isChecked != value) {
                    this.isChecked = value;
                    this.OnCheckStateChange?.Invoke(this, this.isChecked);
                }
            }
        }
        /// <summary>
        /// Set this property to true to mark the checkbox as disabled.
        /// A disabled checkbox cannot be moused over, selected or toggled.
        /// </summary>
        public bool IsDisabled {
            get => this.isDisabled;
            set {
                this.isDisabled = value;
                this.CanBePressed = !value;
                this.CanBeSelected = !value;
            }
        }
        /// <summary>
        /// An event that is invoked when this checkbox's <see cref="Checked"/> property changes
        /// </summary>
        public CheckStateChange OnCheckStateChange;

        private bool isChecked;
        private bool isDisabled;

        /// <summary>
        /// Creates a new checkbox with the given settings
        /// </summary>
        /// <param name="anchor">The checkbox's anchor</param>
        /// <param name="size">The checkbox's size</param>
        /// <param name="label">The checkbox's label text</param>
        /// <param name="defaultChecked">The default value of <see cref="Checked"/></param>
        public Checkbox(Anchor anchor, Vector2 size, string label, bool defaultChecked = false) : base(anchor, size) {
            this.isChecked = defaultChecked;
            this.OnPressed += element => this.Checked = !this.Checked;

            if (label != null) {
                this.Label = new Paragraph(Anchor.CenterLeft, 0, label);
                this.AddChild(this.Label);
            }
        }

        /// <inheritdoc />
        protected override Vector2 CalcActualSize(RectangleF parentArea) {
            var size = base.CalcActualSize(parentArea);
            if (this.Label != null) {
                this.Label.Size = new Vector2((size.X - size.Y) / this.Scale - this.TextOffsetX, 1);
                this.Label.PositionOffset = new Vector2(size.Y / this.Scale + this.TextOffsetX, 0);
            }
            return size;
        }

        /// <inheritdoc />
        public override void Draw(GameTime time, SpriteBatch batch, float alpha, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, Effect effect, Matrix matrix) {
            var tex = this.Texture;
            var color = Color.White * alpha;
            if (this.IsDisabled) {
                tex = this.DisabledTexture.OrDefault(tex);
                color = (Color) this.DisabledColor * alpha;
            } else if (this.IsMouseOver) {
                tex = this.HoveredTexture.OrDefault(tex);
                color = (Color) this.HoveredColor * alpha;
            }

            var boxDisplayArea = new RectangleF(this.DisplayArea.Location, new Vector2(this.DisplayArea.Height));
            batch.Draw(tex, boxDisplayArea, color, this.Scale);
            if (this.Checked)
                batch.Draw(this.Checkmark, boxDisplayArea, Color.White * alpha);
            base.Draw(time, batch, alpha, blendState, samplerState, depthStencilState, effect, matrix);
        }

        /// <inheritdoc />
        protected override void InitStyle(UiStyle style) {
            base.InitStyle(style);
            this.Texture = this.Texture.OrStyle(style.CheckboxTexture);
            this.HoveredTexture = this.HoveredTexture.OrStyle(style.CheckboxHoveredTexture);
            this.HoveredColor = this.HoveredColor.OrStyle(style.CheckboxHoveredColor);
            this.DisabledTexture = this.DisabledTexture.OrStyle(style.CheckboxDisabledTexture);
            this.DisabledColor = this.DisabledColor.OrStyle(style.CheckboxDisabledColor);
            this.Checkmark = this.Checkmark.OrStyle(style.CheckboxCheckmark);
            this.TextOffsetX = this.TextOffsetX.OrStyle(style.CheckboxTextOffsetX);
        }

        /// <summary>
        /// A delegate used for <see cref="Checkbox.OnCheckStateChange"/>
        /// </summary>
        /// <param name="box">The checkbox whose checked state changed</param>
        /// <param name="checced">The new value of <see cref="Checkbox.Checked"/></param>
        public delegate void CheckStateChange(Checkbox box, bool checced);

    }
}