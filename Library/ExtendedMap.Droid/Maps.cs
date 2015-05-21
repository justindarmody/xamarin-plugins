using System;
using Android.OS;
using Android.Content;
using Android.App;

namespace ExtendedMaps
{
	public class Maps
	{
		public static void Init(Activity context, Bundle bundle)
		{
			Xamarin.FormsMaps.Init (context, bundle);

			Droid.LiteMapRenderer.Bundle = bundle;
		}
	}
}