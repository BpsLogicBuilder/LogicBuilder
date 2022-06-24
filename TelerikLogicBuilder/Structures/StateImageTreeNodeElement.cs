using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class StateImageTreeNodeElement : TreeNodeElement
    {
        private readonly IExceptionHelper _exceptionHelper;

        public StateImageTreeNodeElement(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
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

            int size = this.Data.Image.Width;

            if (this.Data.Image.Height != size)
                throw _exceptionHelper.CriticalException("{1A6B297D-640D-4F5B-A317-11A70333F1AE}");
            if (this.StateImage!.Width != size)
                throw _exceptionHelper.CriticalException("{11E511D5-BC1A-454A-BF27-A3F92B6A1F59}");
            if (this.StateImage!.Height != size)
                throw _exceptionHelper.CriticalException("{EB64AADD-4161-49D7-95EE-50FCA904922C}");

            Bitmap bitmap = new(size * 2, size);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(this.StateImage!, 0, 0, size, size);
            graphics.DrawImage(this.Data.Image, size, 0, size, size);

            return bitmap;
        }

        protected override Type ThemeEffectiveType => typeof(TreeNodeElement);
    }
}
