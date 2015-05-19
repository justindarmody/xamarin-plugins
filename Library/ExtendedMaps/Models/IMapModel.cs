using System;
using Xamarin.Forms.Maps;

namespace ExtendedMaps
{
	public interface IMapModel
	{
		string Name { get; set; }
		string Details { get; set; }
		Position Location { get; set; }
		string ImageUrl { get; set; }

		Pin AsPin();
	}
}

