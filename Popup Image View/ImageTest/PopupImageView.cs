using System;
using Xamarin.Forms;

namespace ImageTest
{
	public class PopupImageView : Image
	{
		internal event EventHandler PopupRequested;
		// empty, all extra stuff is done in native renderers

		public void Show()
		{
			if (this.PopupRequested != null)
			{
				this.PopupRequested.Invoke(this, new EventArgs());
			}
		}
	}
}

