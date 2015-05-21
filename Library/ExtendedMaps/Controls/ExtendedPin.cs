using System;
using Xamarin.Forms.Maps;

namespace ExtendedMaps
{
	public class ExtendedPin : IMapModel
	{
		public ExtendedPin()
		{
		}

		public ExtendedPin(string name, string details, double latitude, double longitude)
		{
			Name = name;
			Details = details;
			Location = new Position(latitude, longitude);
		}

		public string Name { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
		public Position Location { get; set; }

		public Pin AsPin ()
		{
			return new Pin () {
				Label = this.Name,
				Position = this.Location,
				Address = this.Details,
			};
		}
	}
}