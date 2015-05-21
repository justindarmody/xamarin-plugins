using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

// http://blog.falafel.com/finding-closest-location-by-latitude-and-longitude/

namespace ExtendedMaps
{
	public static class GeoHelper
	{
		// Convert from Degrees to Radians
		private static double ToRad (this double num)
		{
			return num * Math.PI / 180;
		}

		// Convert from Radians to Degrees
		private static double ToDeg (this double num)
		{
			return num * 180 / Math.PI;
		}

		public static Position GetCentralPosition(IEnumerable<Position> geoCoordinates)
		{
			if (geoCoordinates.Count() == 1) {
				return geoCoordinates.Single ();
			}

			double x = 0;
			double y = 0;
			double z = 0;

			foreach (var geoCoordinate in geoCoordinates) {
				var latitude = geoCoordinate.Latitude * Math.PI / 180;
				var longitude = geoCoordinate.Longitude * Math.PI / 180;

				x += Math.Cos (latitude) * Math.Cos (longitude);
				y += Math.Cos (latitude) * Math.Sin (longitude);
				z += Math.Sin (latitude);
			}

			var total = geoCoordinates.Count();

			x = x / total;
			y = y / total;
			z = z / total;

			var centralLongitude = Math.Atan2 (y, x);
			var centralSquareRoot = Math.Sqrt (x * x + y * y);
			var centralLatitude = Math.Atan2 (z, centralSquareRoot);

			return new Position (
				centralLatitude * 180 / Math.PI,
				centralLongitude * 180 / Math.PI
			);
		}

		public static double GetDistance (double lat1, double lon1, double lat2, double lon2, bool inMeters = false)
		{
			const int r = 6371; // radius of earth in km

			// Convert to Radians
			lat1 = lat1.ToRad ();
			lon1 = lon1.ToRad ();
			lat2 = lat2.ToRad ();
			lon2 = lon2.ToRad ();

			// Spherical Law of Cosines
			var resultCos =
				Math.Acos (
					Math.Sin (lat1) * Math.Sin (lat2) +
					Math.Cos (lat1) * Math.Cos (lat2) * Math.Cos (lon2 - lon1)
				) * r;

			if (inMeters) {
				return resultCos * 1000;
			} else {
				return resultCos;
			}
		}

		public static Position GetClosestPoint (double originLat, double originLong, IEnumerable<Position> points)
		{
			// Build a List<Distance> that contains the calculated distance for each point
			var list = points.Select (p => new Distance (p, GetDistance (originLat, originLong, p.Latitude, p.Longitude))).ToList ();

			if (!list.Any ()) 
			{
				return default(Position);
			}

			// Sort the List using the custom IComparable implementation to sort by Distance.Km
			list.Sort ();

			return list.First ().Position;
		}

		public static double GetRadius (Position center, IEnumerable<Position> points, bool meters = false)
		{
			var distances = new List<double> ();
			//			var distances = points.Select (p => GetDistance (center.Latitude, center.Longitude, p.Latitude, p.Longitude));
			foreach (Position point in points) {
				var distance = GetDistance (center.Latitude, center.Longitude, point.Latitude, point.Longitude);
				distances.Add (distance);
			}

			var max = distances.Max ();

			if (meters) {
				return max * 1000;
			} else {
				return max;
			}
		}

		private class Distance : IComparable
		{
			internal int Id { get; set; }

			internal double Km { get; set; }

			internal double Latitude { get; set; }

			internal double Longitude { get; set; }

			internal Position Position { get { return new Position (this.Latitude, this.Longitude); } }

			public Distance (Position iLocation, double km)
			{
//				Id = iLocation.Id;
				Latitude = iLocation.Latitude;
				Longitude = iLocation.Longitude;
				Km = km;
			}

			// Compare Km for sorting
			public int CompareTo (object obj)
			{
				var d = (Distance)obj;
				return Km.CompareTo (d.Km);
			}
		}
	}
}