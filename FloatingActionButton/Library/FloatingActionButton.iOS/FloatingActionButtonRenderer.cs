using System;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using Xamarin.Forms;
using UIKit;

[assembly: ExportRenderer(typeof(FAB.Forms.FloatingActionButton), typeof(FAB.iOS.FloatingActionButtonRenderer))]

namespace FAB.iOS
{
    public partial class FloatingActionButtonRenderer : ViewRenderer<FAB.Forms.FloatingActionButton, FAB.iOS.MNFloatingActionButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<FAB.Forms.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var fab = new MNFloatingActionButton();
                fab.Frame = new CoreGraphics.CGRect(0, 0, 24, 24);

                this.SetNativeControl(fab);

                this.UpdateStyle();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == "Width" ||
                e.PropertyName == "Height" ||
                e.PropertyName == "X" ||
                e.PropertyName == "Y")
                {
                    
                }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(24, 24)); //base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        private void UpdateStyle()
        {
            switch (this.Element.Size)
            {
                case FAB.Forms.FabSize.Mini:
                    this.Control.Size = MNFloatingActionButton.FABSize.Mini;
                    break;
                case FAB.Forms.FabSize.Normal:
                    this.Control.Size = MNFloatingActionButton.FABSize.Normal;
                    break;
            }
//
            this.Control.BackgroundColor = this.Element.NormalColor.ToUIColor();
            this.Control.PressedBackgroundColor = this.Element.PressedColor.ToUIColor();

            this.Control.ShadowOpacity = this.Element.HasShadow ? 0.6f : 0.0f;

//            this.Control.ColorNormal = this.Element.NormalColor.ToAndroid();
//            this.Control.ColorPressed = this.Element.PressedColor.ToAndroid();
//            this.Control.ColorDisabled = this.Element.PressedColor.ToAndroid();
//
//            this.Control.HasShadow = this.Element.HasShadow;

            Task.Run(async () =>
                {
                    await SetImageAsync(this.Element.Source, (float)this.Element.WidthRequest, (float)this.Element.Height, this.Control);
                });
        }

        private async static Task SetImageAsync(ImageSource source, nfloat widthRequest, nfloat heightRequest, MNFloatingActionButton targetButton)
        {
            var handler = GetHandler(source);
            using (UIImage image = await handler.LoadImageAsync(source))
            {
                UIGraphics.BeginImageContext(new CoreGraphics.CGSize(widthRequest, heightRequest));
                image.Draw(new CoreGraphics.CGRect(0, 0, widthRequest, heightRequest));
                using (var resultImage = UIGraphics.GetImageFromCurrentImageContext())
                {
                    UIGraphics.EndImageContext();
                    using (var resizableImage = resultImage.CreateResizableImage(new UIEdgeInsets(0f, 0f, widthRequest, heightRequest)))
                    {
                        targetButton.CenterImageView.Image = resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    }
                }
            }
        }
    }
}