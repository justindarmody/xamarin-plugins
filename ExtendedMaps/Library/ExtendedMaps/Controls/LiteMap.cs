using System;
using Xamarin.Forms.Maps;

namespace ExtendedMaps
{
	public class LiteMap : ExtendedMap
	{
		internal event EventHandler<MapSpan> MoveToRegionRequested = delegate {};

		public LiteMap(MapSpan region) : base(region)
		{
//			LastMoveToRegion = region;
		}

		public new void MoveToRegion(MapSpan mapSpan)
		{
			this.MoveToRegionRequested (this, mapSpan);
		}
	}
}