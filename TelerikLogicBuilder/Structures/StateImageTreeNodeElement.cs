using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Drawing;
using System.Globalization;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class StateImageTreeNodeElement : TreeNodeElement
    {
        public StateImageTreeNodeElement()
        {
        }

        public Image? StateImage { get; set; }

        public override void Synchronize()
        {
            base.Synchronize();
            this.ImageElement.Image = GetImage();
        }

        private Image GetImage()
        {
            if (this.StateImage == null)
                return this.Data.Image;

            if (this.Data.Image == null)
                return this.StateImage;

            int size = this.Data.Image.Width;
            
            if (this.Data.Image.Height != size)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1A6B297D-640D-4F5B-A317-11A70333F1AE}"));
            if (this.StateImage!.Width != size)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{11E511D5-BC1A-454A-BF27-A3F92B6A1F59}"));
            if (this.StateImage!.Height != size)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{EB64AADD-4161-49D7-95EE-50FCA904922C}"));

            Bitmap bitmap = new(size * 2, size);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(this.StateImage, 0, 0, size, size);
            graphics.DrawImage(this.Data.Image, size, 0, size, size);

            return bitmap;
        }

        protected override Type ThemeEffectiveType => typeof(TreeNodeElement);
    }
}
