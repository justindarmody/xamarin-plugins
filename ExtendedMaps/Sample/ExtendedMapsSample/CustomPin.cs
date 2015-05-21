using System;
using System.Linq;
using ExtendedMaps;
using System.Collections.Generic;

namespace ExtendedMapsSample
{
	public class CustomPin : ExtendedPin
	{
		public IEnumerable<ExtendedPin> Enumerable
		{
			get {
				return new List<ExtendedPin> {
					this
				}.AsEnumerable ();
			}
		}
	}
}

