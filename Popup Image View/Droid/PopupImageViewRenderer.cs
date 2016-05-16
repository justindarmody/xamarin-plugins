using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using ImageTest;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(PopupImageView), typeof(PopupImageViewRenderer))]
namespace ImageTest
{
	public class PopupImageViewRenderer : ImageRenderer, Android.Views.View.IOnClickListener
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				(e.NewElement as PopupImageView).PopupRequested += HandlePopupRequested;
			}
			else if (e.OldElement != null)
			{
				(e.NewElement as PopupImageView).PopupRequested -= HandlePopupRequested;
			}
		}

		/// <summary>
		/// Handles displaying the image in a full screen dialog.
		/// Will load a new instance of the image into the dialog's ImageView.
		/// </summary>
		private async void HandlePopupRequested(object sender, EventArgs e)
		{
			Dialog nagDialog = new Dialog(this.Context, global::Android.Resource.Style.ThemeTranslucentNoTitleBarFullScreen);
			nagDialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			nagDialog.SetCancelable(false);
			nagDialog.SetContentView(Droid.Resource.Layout.PreviewImage);

			var root = nagDialog.FindViewById<Android.Widget.RelativeLayout>(Droid.Resource.Id.iv_preview_container);
			root.SetBackgroundColor(Xamarin.Forms.Color.White.ToAndroid());

			Android.Widget.Button btnClose = nagDialog.FindViewById<Android.Widget.Button>(Droid.Resource.Id.btnIvClose);
			ImageView ivPreview = nagDialog.FindViewById<ImageView>(Droid.Resource.Id.iv_preview_image);

			ivPreview.SetImageResource(global::Android.Resource.Color.Transparent);

			// this bitmap loading logic was taken & slightly modified from the Xamarin.Forms image renderer
			// https://github.com/xamarin/Xamarin.Forms/blob/2d9288eee6e6f197364a64308183725e7bd561f9/Xamarin.Forms.Platform.Android/Renderers/ImageRenderer.cs
			// https://github.com/xamarin/Xamarin.Forms/blob/2d9288eee6e6f197364a64308183725e7bd561f9/Xamarin.Forms.Platform.Android/Renderers/ImageLoaderSourceHandler.cs

			BitmapDrawable drawable = this.Control.Drawable as BitmapDrawable;

			try
			{
				if (drawable != null)
				{
					Bitmap bitmap = drawable.Bitmap;

					ivPreview.SetImageBitmap(bitmap);

					if (bitmap != null)
					{
						bitmap.Dispose();
					}

					btnClose.Click += (s2, e2) => nagDialog.Dismiss();

					nagDialog.Show();
				}
			}
			catch (Exception ex)
			{
				Toast.MakeText(this.Context, "Unable to load your the image for preview.", ToastLength.Short);
			}
		}
	}
}