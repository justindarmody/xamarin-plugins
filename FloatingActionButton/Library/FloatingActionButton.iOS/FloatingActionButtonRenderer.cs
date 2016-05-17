using System;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using Xamarin.Forms;
using UIKit;
using FAB.Forms;

[assembly: ExportRenderer(typeof(FAB.Forms.FloatingActionButton), typeof(FAB.iOS.FloatingActionButtonRenderer))]

namespace FAB.iOS
{
	public partial class FloatingActionButtonRenderer : ViewRenderer<FloatingActionButton, MNFloatingActionButton>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButton> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				var fab = new MNFloatingActionButton();
				fab.Frame = new CoreGraphics.CGRect(0, 0, 24, 24);

				this.SetNativeControl(fab);

				this.UpdateStyle();
			}

			if (e.NewElement != null)
			{
				this.Control.TouchUpInside += this.Fab_TouchUpInside;
			}

			if (e.OldElement != null)
			{
				this.Control.TouchUpInside -= this.Fab_TouchUpInside;
			}
		}

		void Fab_TouchUpInside(object sender, EventArgs e)
		{
			this.Element.SendClicked();
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var viewSize = this.Element.Size == FAB.Forms.FabSize.Normal ? 56 : 40;

			return new SizeRequest(new Size(viewSize, viewSize));
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

			this.Control.BackgroundColor = this.Element.NormalColor.ToUIColor();
			this.Control.PressedBackgroundColor = this.Element.PressedColor.ToUIColor();

			this.Control.HasShadow = this.Element.HasShadow;

			SetImageAsync(this.Element.Source, (float)this.Element.WidthRequest, (float)this.Element.Height, this.Control);
		}

		private async static void SetImageAsync(ImageSource source, nfloat widthRequest, nfloat heightRequest, MNFloatingActionButton targetButton)
		{
			var handler = GetHandler(source);
			using (UIImage image = await handler.LoadImageAsync(source))
			{
				UIGraphics.BeginImageContext(new CoreGraphics.CGSize(widthRequest, heightRequest));
				image.Draw(new CoreGraphics.CGRect(0, 0, widthRequest, heightRequest));
				using (var resultImage = UIGraphics.GetImageFromCurrentImageContext())
				{
					if (resultImage != null)
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
}