using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Java.Lang;

[assembly: ExportRenderer (typeof(ExtendedMaps.ExtendedMap), typeof(ExtendedMaps.Droid.ExtendedMapRenderer))]
namespace ExtendedMaps.Droid
{
	public class ExtendedMapRenderer : MapRenderer, GoogleMap.IOnCameraChangeListener
	{
		private List<Marker> markers;

		bool _isDrawnDone;

		protected new ExtendedMap Map
		{
			get { return this.Element as ExtendedMap; }
		}

		private GoogleMap NativeMap
		{
			get { return (this.Control as MapView).Map; }
		}

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);
			var formsMap = (ExtendedMap)Element;
			var androidMapView = (MapView)Control;

			if (androidMapView != null && androidMapView.Map != null)
			{
				androidMapView.Map.InfoWindowClick += MapOnInfoWindowClick;

				androidMapView.Map.SetOnCameraChangeListener (this);
			}

			if (formsMap != null)
			{
				((ObservableCollection<Pin>)formsMap.Pins).CollectionChanged += OnCollectionChanged;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == ExtendedMap.VisibleRegionProperty.PropertyName && !_isDrawnDone)
			{
				UpdatePins ();

				this.MoveToRegion (this.Map.VisibleRegion, true);

				_isDrawnDone = true;
			} else if (e.PropertyName == ExtendedMap.SelectedPinProperty.PropertyName)
				{
					this.UpdateSelectedPin ();	
				}
		}

		void OnCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdatePins ();
		}

		private void MoveToRegion (MapSpan span, bool animate)
		{
			if (this.NativeMap == null)
			{
				return;
			}
//			span = span.ClampLatitude (85.0, -85.0);

			LatLng northeast = new LatLng (span.Center.Latitude + span.LatitudeDegrees / 2.0, span.Center.Longitude + span.LongitudeDegrees / 2.0);
			CameraUpdate update = CameraUpdateFactory.NewLatLngBounds (new LatLngBounds (new LatLng (span.Center.Latitude - span.LatitudeDegrees / 2.0, span.Center.Longitude - span.LongitudeDegrees / 2.0), northeast), 0);
			try
			{
				this.NativeMap.MoveCamera (update);
			} catch (IllegalStateException ex)
			{
			}
		}

		private void UpdatePins ()
		{
			var androidMapView = (MapView)Control;
			var formsMap = (ExtendedMap)Element;

			androidMapView.Map.Clear ();

			androidMapView.Map.MarkerClick += HandleMarkerClick;
			androidMapView.Map.MyLocationEnabled = formsMap.IsShowingUser;

			if (formsMap.ItemsSource != null)
			{

				if (this.markers == null)
					this.markers = new List<Marker> ();

				var items = formsMap.ItemsSource.Cast<IMapModel> ();

				foreach (var item in items)
				{
					var markerWithIcon = new MarkerOptions ();

					markerWithIcon.SetPosition (new LatLng (item.Location.Latitude, item.Location.Longitude));
					markerWithIcon.SetTitle (string.IsNullOrWhiteSpace (item.Name) ? "-" : item.Name);
					markerWithIcon.SetSnippet (item.Details);

					markerWithIcon.InvokeIcon (BitmapDescriptorFactory.DefaultMarker ());

					var addedMarker = androidMapView.Map.AddMarker (markerWithIcon);
					this.markers.Add (addedMarker);
				}
			}
		}

		public new void OnCameraChange (CameraPosition position)
		{
			// 156543.03392 * Math.cos(latLng.lat() * Math.PI / 180) / Math.pow(2, zoom)
//			var radius = 156543.03392 * System.Math.Cos(position.Target.Latitude * System.Math.PI / 180) / System.Math.Pow(2, position.Zoom);

			var bounds = this.NativeMap.Projection.VisibleRegion.LatLngBounds;

			var radiusInMeters = GeoHelper.GetDistance (bounds.Northeast.Latitude, bounds.Northeast.Longitude, bounds.Southwest.Latitude, bounds.Southwest.Longitude, true);

			var span = MapSpan.FromCenterAndRadius (
				           new Position (bounds.Center.Latitude, bounds.Center.Longitude),
				           Distance.FromMeters (radiusInMeters));
//				.WithZoom (position.Zoom);

			this.Map.VisibleRegion = span;
		}

		private void UpdateSelectedPin ()
		{
			var formsMap = (ExtendedMap)this.Element;
			var androidMapView = (MapView)Control;

			Marker selectedMarker = this.markers.FirstOrDefault (m => IsItem (formsMap.SelectedPin, m));

			if (selectedMarker != null)
			{

				var ne = androidMapView.Map.Projection.VisibleRegion.LatLngBounds.Northeast;
				var sw = androidMapView.Map.Projection.VisibleRegion.LatLngBounds.Southwest;

				var radius = GeoHelper.GetDistance (ne.Latitude, ne.Longitude, sw.Latitude, sw.Longitude, true);

				formsMap.MoveToRegion (MapSpan.FromCenterAndRadius (
					new Position (
						selectedMarker.Position.Latitude, 
						selectedMarker.Position.Longitude),
					Distance.FromMeters (radius))
				);
				selectedMarker.ShowInfoWindow ();
			}

		}

		private void HandleMarkerClick (object sender, GoogleMap.MarkerClickEventArgs e)
		{
			var marker = e.Marker;
			marker.ShowInfoWindow ();

			var map = this.Element as ExtendedMap;

			var formsPin = new ExtendedPin (marker.Title, marker.Snippet, marker.Position.Latitude, marker.Position.Longitude);

			map.SelectedPin = formsPin;
		}

		private void MapOnInfoWindowClick (object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			Marker clickedMarker = e.Marker;
			// Find the matchin item
			var formsMap = (ExtendedMap)Element;
			if (formsMap.ShowDetailCommand != null && formsMap.ShowDetailCommand.CanExecute (formsMap.SelectedPin))
			{
				formsMap.ShowDetailCommand.Execute (formsMap.SelectedPin);
			}
		}

		private bool IsItem (IMapModel item, Marker marker)
		{
			return item.Name == marker.Title &&
			item.Details == marker.Snippet &&
			item.Location.Latitude == marker.Position.Latitude &&
			item.Location.Longitude == marker.Position.Longitude;
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);

			//NOTIFY CHANGE

			if (changed)
			{
				_isDrawnDone = false;
			}
		}
	}
}