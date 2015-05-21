using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections;

namespace ExtendedMaps
{
	public class ExtendedMap : Map
	{
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<ExtendedMap, IEnumerable> (x => x.ItemsSource, null, propertyChanged: OnItemsSourceChanged);

		public static readonly BindableProperty SelectedPinProperty = BindableProperty.Create<ExtendedMap, ExtendedPin> (x => x.SelectedPin, null);

		public static readonly BindableProperty VisibleRegionProperty = BindableProperty.Create<ExtendedMap, MapSpan> (x => x.VisibleRegion, default(MapSpan));

		public static readonly BindableProperty ShowDetailCommandProperty = BindableProperty.Create<ExtendedMap, ICommand>( x => x.ShowDetailCommand, null);

		public ExtendedMap(MapSpan region) : base(region)
		{
		}

		public ExtendedPin SelectedPin 
		{
			get{ return (ExtendedPin)base.GetValue (SelectedPinProperty); }
			set{ base.SetValue (SelectedPinProperty, value); }
		}

		public ICommand ShowDetailCommand
		{ 
			get{ return (ICommand)base.GetValue (ShowDetailCommandProperty); }
			set{ base.SetValue (ShowDetailCommandProperty, value); }
		}

//		public MapSpan LastMoveToRegion { get; private set; }

		public new MapSpan VisibleRegion
		{ 
			get{ return (MapSpan)base.GetValue (VisibleRegionProperty); }
			set{ base.SetValue (VisibleRegionProperty, value); }
		}

		public IEnumerable ItemsSource
		{
			get{ return (IEnumerable)base.GetValue (ItemsSourceProperty); }
			set{ base.SetValue (ItemsSourceProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
		{
			var picker = bindable as ExtendedMap;
			picker.Pins.Clear ();

			if (newvalue != null)
			{
				var newList = newvalue.Cast<IMapModel> ();

				if (newList == null) {
					throw new ArgumentException ("Your ItemsSource must be compatible with IEnumerable<IMapModel>");
				}

				//now it works like "subscribe once" but you can improve
				foreach (var item in newList)
				{
					picker.Pins.Add (item.AsPin ());
				}
			}
		}
	}
}

