using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;
using System.Threading.Tasks;
using Android.Graphics;
using Android.App;
using FAB.Forms;

[assembly: ExportRenderer(typeof(FAB.Forms.FloatingActionButton), typeof(FAB.Droid.FloatingActionButtonRenderer))]

namespace FAB.Droid
{
    public partial class FloatingActionButtonRenderer : ViewRenderer<FloatingActionButton, com.refractored.fab.FloatingActionButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var fab = new com.refractored.fab.FloatingActionButton(this.Context);


                this.SetNativeControl(fab);

                this.UpdateStyle();
            }

            if (e.NewElement != null)
            {
                this.Control.Click += Fab_Click;
            }
            else if (e.OldElement != null)
            {
                this.Control.Click -= Fab_Click;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == FloatingActionButton.SourceProperty.PropertyName)
            {
                this.SetSize();
            }
            else if (e.PropertyName == FloatingActionButton.NormalColorProperty.PropertyName ||
                     e.PropertyName == FloatingActionButton.PressedColorProperty.PropertyName ||
                     e.PropertyName == FloatingActionButton.DisabledColorProperty.PropertyName)
            {
                this.SetBackgroundColors();
            }
            else if (e.PropertyName == FloatingActionButton.HasShadowProperty.PropertyName)
            {
                this.SetHasShadow();
            }
            else if (e.PropertyName == FloatingActionButton.SourceProperty.PropertyName)
            {
                this.SetImage();
            }
            else if (e.PropertyName == FloatingActionButton.IsEnabledProperty.PropertyName)
            {
                this.UpdateEnabled();
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Control.Click -= this.Fab_Click;
            }

            base.Dispose(disposing);
        }

        private void UpdateStyle()
        {
            this.SetSize();

            this.SetBackgroundColors();

            this.SetHasShadow();

            this.SetImage();

            this.UpdateEnabled();
        }

        private void SetSize()
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
        }

        private void SetBackgroundColors()
        {
            this.Control.ColorNormal = this.Element.NormalColor.ToAndroid();
            this.Control.ColorPressed = this.Element.PressedColor.ToAndroid();
            this.Control.ColorDisabled = this.Element.DisabledColor.ToAndroid();
        }

        private void SetHasShadow()
        {
            this.Control.HasShadow = this.Element.HasShadow;
        }

        private void SetImage()
        {
            Task.Run(async () =>
            {
                var bitmap = await this.GetBitmapAsync(this.Element.Source);

                (this.Context as Activity).RunOnUiThread(() =>
                {
                    this.Control.SetImageBitmap(bitmap);
                });
            });
        }

        private void UpdateEnabled()
        {
            this.Control.Enabled = this.Element.IsEnabled;
        }

        private async Task<Bitmap> GetBitmapAsync(ImageSource source)
        {
            var handler = GetHandler(source);
            var returnValue = (Bitmap)null;

            returnValue = await handler.LoadImageAsync(source, this.Context);

            return returnValue;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            this.Element.SendClicked();
        }
    }
}