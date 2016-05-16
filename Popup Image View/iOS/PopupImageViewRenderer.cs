using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using CoreGraphics;
using ImageTest;

[assembly: ExportRenderer(typeof(PopupImageView), typeof(PopupImageViewRenderer))]
namespace ImageTest
{
	public class PopupImageViewRenderer : ImageRenderer
	{
		UIViewFullscreen vMain;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				(e.NewElement as PopupImageView).PopupRequested += HandlePopupRequested;
			}
			else if (e.OldElement != null)
			{
				(e.OldElement as PopupImageView).PopupRequested -= HandlePopupRequested;
			}
		}

		private void HandlePopupRequested(object sender, EventArgs e)
		{
			if (vMain == null)
			{
				vMain = new UIViewFullscreen();
			}

			vMain.SetImage(this.Control.Image);
			vMain.Show();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.vMain = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// UIView used to fill the entire window and overlay over current view controller
		/// </summary>
		public class UIViewFullscreen : UIView
		{
			UIImage iImage;
			UIScrollViewImage sviMain;

			public bool UseAnimation = true;
			public float AnimationDuration = 0.3f;

			public UIViewFullscreen()
			{
				var cBackground = new UIColor(0.0f, 0.0f, 0.0f, 0.6f);
				BackgroundColor = cBackground;

				sviMain = new UIScrollViewImage();
				AddSubview(sviMain);
			}

			public void SetImage(UIImage image)
			{
				iImage = image;
			}

			public void Show()
			{
				var window = UIApplication.SharedApplication.Windows[0];
				Frame = window.Frame;
				sviMain.Frame = window.Frame;
				sviMain.SetImage(iImage);
				sviMain.OnSingleTap += () =>
				{
					Hide();
				};

				window.AddSubview(this);

				Alpha = 0f;
				UIView.Animate(AnimationDuration, () =>
					{
						Alpha = 1f;
					});
			}

			public void Hide()
			{
				if (Superview != null)
				{
					if (!UseAnimation)
					{
						RemoveFromSuperview();
					}
					else
					{
						Alpha = 1f;
						UIView.Animate(AnimationDuration, () =>
							{
								Alpha = 0f;
							}, () =>
							{
								RemoveFromSuperview();
							});
					}
				}
			}
		}

		/// <summary>
		/// Scrollview used to display the image & allow it to scale to fill the window
		/// </summary>
		public class UIScrollViewImage : UIScrollView
		{
			nfloat defaultZoom = 2f;
			nfloat minZoom = 0.1f;
			nfloat maxZoom = 3f;
			nfloat sizeToFitZoom = 1f;

			UIImageView ivMain;

			UITapGestureRecognizer grTap;
			UITapGestureRecognizer grDoubleTap;

			public event Action OnSingleTap;

			public override CGRect Frame
			{
				get
				{
					return base.Frame;
				}
				set
				{
					base.Frame = value;

					if (ivMain != null)
					{
						ivMain.Frame = value;
					}
				}
			}

			public UIScrollViewImage()
			{
				AutoresizingMask = UIViewAutoresizing.All;

				ivMain = new UIImageView();
				//          ivMain.AutoresizingMask = UIViewAutoresizing.All;
				ivMain.ContentMode = UIViewContentMode.ScaleAspectFit;
				//          ivMain.BackgroundColor = UIColor.Red; // DEBUG
				AddSubview(ivMain);

				ScrollEnabled = false;

				// Setup zoom
				MaximumZoomScale = 3.0f;
				MinimumZoomScale = 0.1f;
				ShowsVerticalScrollIndicator = false;
				ShowsHorizontalScrollIndicator = false;
				BouncesZoom = true;
				ViewForZoomingInScrollView += (UIScrollView sv) =>
				{
					return ivMain;
				};

				// Setup gestures
				grTap = new UITapGestureRecognizer(() =>
					{
						if (OnSingleTap != null)
						{
							OnSingleTap();
						}
					});
				grTap.NumberOfTapsRequired = 1;
				AddGestureRecognizer(grTap);
			}

			public void SetImage(UIImage image)
			{
				ZoomScale = 1;

				ivMain.Image = image;
				ivMain.Frame = new CGRect(new CGPoint(), image.Size);
				ContentSize = image.Size;

				double wScale = (double)(Frame.Width / image.Size.Width);
				double hScale = (double)(Frame.Height / image.Size.Height);

				MinimumZoomScale = (nfloat)Math.Min(wScale, hScale);
				sizeToFitZoom = MinimumZoomScale;
				ZoomScale = MinimumZoomScale;

				ivMain.Frame = CenterScrollViewContents();
			}

			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				ivMain.Frame = CenterScrollViewContents();
			}

			public CGRect CenterScrollViewContents()
			{
				var boundsSize = Bounds.Size;
				var contentsFrame = ivMain.Frame;

				if (contentsFrame.Width < boundsSize.Width)
				{
					contentsFrame.X = (boundsSize.Width - contentsFrame.Width) / 2;
				}
				else
				{
					contentsFrame.X = 0;
				}

				if (contentsFrame.Height < boundsSize.Height)
				{
					contentsFrame.Y = (boundsSize.Height - contentsFrame.Height) / 2;
				}
				else
				{
					contentsFrame.Y = 0;
				}

				return contentsFrame;
			}
		}
	}
}

