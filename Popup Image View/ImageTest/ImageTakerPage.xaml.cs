using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ImageTest
{
	public partial class ImageTakerPage : ContentPage
	{
		public ImageTakerPage()
		{
			this.BindingContext = new ImageTakerPageModel();

			this.InitializeComponent();
		}

		private void HandleImagePreviewTapped(object sender, EventArgs args)
		{
			this.imagePreview.Show();
		}
	}
}

