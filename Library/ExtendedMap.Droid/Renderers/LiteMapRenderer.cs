using System;
using System.Linq;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Maps;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Android.Gms.Maps.Model;
using Java.Lang;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

[assembly: ExportRenderer (typeof(ExtendedMaps.LiteMap), typeof(ExtendedMaps.Droid.LiteMapRenderer))]

namespace ExtendedMaps.Droid
{
	public class LiteMapRenderer : ViewRenderer, IOnMapReadyCallback, IJavaObject, IDisposable
	{
		private bool init = true;
		private bool disposed = false;
		private List<Marker> markers;
		private bool _isDrawnDone;

		private static Bundle bundle;

		internal static Bundle Bundle {
			set {
				LiteMapRenderer.bundle = value;
			}
		}

		protected GoogleMap googleMap;

		protected LiteMap Map {
			get {
				return (LiteMap)this.Element;
			}
		}

		public LiteMapRenderer ()
		{
			this.AutoPackage = false;
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			this.googleMap = googleMap;

			this.googleMap.UiSettings.CompassEnabled = false;
			this.googleMap.UiSettings.MyLocationButtonEnabled = false;
			this.googleMap.UiSettings.MapToolbarEnabled = false;

			MapsInitializer.Initialize (this.Context);

			this.MoveToRegion (((LiteMap)this.Element).VisibleRegion, false);
		}

		public override SizeRequest GetDesiredSize (int widthConstraint, int heightConstraint)
		{
			return new SizeRequest (new Size ((double)ContextExtensions.ToPixels (this.Context, 40.0), (double)ContextExtensions.ToPixels (this.Context, 40.0)));
		}

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);
			MapView mapView1 = (MapView)this.Control;

			if (e.OldElement != null) {
				var formsMap = e.OldElement as LiteMap;
				((ObservableCollection<Pin>)formsMap.Pins).CollectionChanged -= OnCollectionChanged;
				this.Map.MoveToRegionRequested -= this.MoveToRegionRequested;
			}

			if (e.NewElement != null) {
				this.Map.MoveToRegionRequested += this.MoveToRegionRequested;

				var options = new GoogleMapOptions ();
				options.InvokeLiteMode (true);

				MapView mapView2 = new MapView (this.Context, options);
				mapView2.OnCreate (LiteMapRenderer.bundle);
				mapView2.OnResume ();

				this.SetNativeControl (mapView2);

				mapView2.GetMapAsync (this);

				if (e.NewElement != null) {
					var formsMap = e.NewElement as LiteMap;
					((ObservableCollection<Pin>)formsMap.Pins).CollectionChanged += OnCollectionChanged;
				}
			}
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			GoogleMap nativeMap = this.googleMap;
			if (nativeMap == null) {
				return;
			}

			if (e.PropertyName == LiteMap.VisibleRegionProperty.PropertyName && !_isDrawnDone) {
				UpdatePins ();

				this.MoveToRegion (this.Map.VisibleRegion, false);

				_isDrawnDone = true;
			}


//			else if (e.PropertyName == Map.IsShowingUserProperty.PropertyName) {
//				nativeMap.MyLocationEnabled = nativeMap.UiSettings.MyLocationButtonEnabled = this.Map.IsShowingUser;
//			}
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
			if (!this.init)
				return;
			this.MoveToRegion (((LiteMap)this.Element).VisibleRegion, false);
			this.UpdatePins ();
			this.init = false;
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing && !this.disposed) {
				this.disposed = true;
				Map map = this.Element as Map;

				GoogleMap nativeMap = this.googleMap;
				if (nativeMap == null) {
					return;
				}

				nativeMap.MyLocationEnabled = false;
				nativeMap.Dispose ();
			}

			base.Dispose (disposing);
		}

		void OnCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdatePins ();
		}

		private void MoveToRegionRequested(object sender, MapSpan mapSpan)
		{
			
		}

		private void MoveToRegion (MapSpan span, bool animate)
		{
			if (span != null)
			{
				if (this.googleMap == null)
				{
					return;
				}
				//			span = span.ClampLatitude (85.0, -85.0);

				LatLng northeast = new LatLng (span.Center.Latitude + span.LatitudeDegrees / 2.0, span.Center.Longitude + span.LongitudeDegrees / 2.0);
				CameraUpdate update = CameraUpdateFactory.NewLatLngBounds (new LatLngBounds (new LatLng (span.Center.Latitude - span.LatitudeDegrees / 2.0, span.Center.Longitude - span.LongitudeDegrees / 2.0), northeast), 0);
				try
				{
					this.googleMap.MoveCamera (update);
				} catch (IllegalStateException ex)
				{
				}
			}
		}

		private void UpdatePins ()
		{
			var androidMapView = (MapView)Control;
			var formsMap = (ExtendedMap)Element;

			androidMapView.Map.Clear ();

			androidMapView.Map.MyLocationEnabled = formsMap.IsShowingUser;

			if (formsMap.ItemsSource != null) {

				if (this.markers == null)
					this.markers = new List<Marker> ();

				var items = formsMap.ItemsSource.Cast<IMapModel> ();

				foreach (var item in items) {
					var markerWithIcon = new MarkerOptions ();

					markerWithIcon.SetPosition (new LatLng (item.Location.Latitude, item.Location.Longitude));
					markerWithIcon.SetTitle (string.IsNullOrWhiteSpace (item.Name) ? "-" : item.Name);
					markerWithIcon.SetSnippet (item.Details);

					markerWithIcon.InvokeIcon (BitmapDescriptorFactory.DefaultMarker ());

					var addedMarker = androidMapView.Map.AddMarker (markerWithIcon);
					this.markers.Add (addedMarker);
				}

				var central = GeoHelper.GetCentralPosition (items.Select (i => i.Location));
				var radius = GeoHelper.GetRadius (central, items.Select (i => i.Location), true);

				this.MoveToRegion (
					MapSpan.FromCenterAndRadius (
						central,
						Distance.FromMeters (radius)
					),
					false
				);
			}
		}
	}
}