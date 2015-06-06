using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;
using System.Threading.Tasks;
using Android.Graphics;

[assembly: ExportRenderer(typeof(FAB.Forms.FloatingActionButton), typeof(FAB.Droid.FloatingActionButtonRenderer))]

namespace FAB.Droid
{
    public partial class FloatingActionButtonRenderer : ViewRenderer<FAB.Forms.FloatingActionButton, com.refractored.fab.FloatingActionButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<FAB.Forms.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var fab = new com.refractored.fab.FloatingActionButton(this.Context);

                this.SetNativeControl(fab);

                this.UpdateStyle();
            }
        }  

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdateStyle()
        {
            switch (this.Element.Size)
            {
                case FAB.Forms.FabSize.Mini:
                    this.Control.Size = com.refractored.fab.FabSize.Mini;
                    break;
                case FAB.Forms.FabSize.Normal:
                    this.Control.Size = com.refractored.fab.FabSize.Normal;
                    break;
            }

            this.Control.ColorNormal = this.Element.NormalColor.ToAndroid();
            this.Control.ColorPressed = this.Element.PressedColor.ToAndroid();
            this.Control.ColorDisabled = this.Element.PressedColor.ToAndroid();

            this.Control.HasShadow = this.Element.HasShadow;

            Task.Run(async () =>  {
                var bitmap = await this.GetBitmapAsync(this.Element.Source);

                this.Control.SetImageBitmap(bitmap);
            });
        }

        private async Task<Bitmap> GetBitmapAsync(ImageSource source)
        {
            var handler = GetHandler(source);
            var returnValue = (Bitmap)null;

            returnValue = await handler.LoadImageAsync(source, this.Context);

            return returnValue;
        }
    }
}

