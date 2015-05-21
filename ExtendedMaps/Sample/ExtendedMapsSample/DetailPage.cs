using System;

using Xamarin.Forms;
using ExtendedMaps;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Maps;

namespace ExtendedMapsSample
{
	public class DetailPage : ContentPage
	{
		public DetailPage (CustomPin pin)
		{
			this.BindingContext = new DetailViewModel(pin);

			var map = new LiteMap (default(MapSpan)) {
				HeightRequest = 200
			};
				
			map.SetBinding<DetailViewModel> (LiteMap.ItemsSourceProperty, b => b.Pins);
			map.SetBinding<DetailViewModel> (LiteMap.VisibleRegionProperty, b => b.Span);

			this.SetBinding<DetailViewModel> (Page.TitleProperty, b => b.Title);

			var detail = new Label ();
			detail.SetBinding<DetailViewModel> (Label.TextProperty, b => b.Description);

			Content = new StackLayout { 
				Children = {
					map,
					new ContentView {
						Padding = new Thickness (15),
						Content = detail
					}
				}
			};
		}

		public class DetailViewModel : INotifyPropertyChanged
		{
			public List<CustomPin> Pins { get; private set; }

			public MapSpan Span { get; private set; }

			public string Title { get; private set; }
			public string Description { get; private set; }

			#region INotifyPropertyChanged implementation
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion

			public DetailViewModel(CustomPin pin)
			{
				this.Pins = new List<CustomPin>() { pin };
				this.Span = MapSpan.FromCenterAndRadius(
					pin.Location,
					Distance.FromMeters(1000)
				);

				this.Title = pin.Name;
				this.Description = pin.Details;

				this.RaisePropertyChanged("Description");
				this.RaisePropertyChanged("Title");
				this.RaisePropertyChanged("Span");
				this.RaisePropertyChanged("Pins");
			}

			private void RaisePropertyChanged([CallerMemberName] string properName = null)
			{
				if (this.PropertyChanged != null) {
					this.PropertyChanged (this, new PropertyChangedEventArgs (properName));
				}
			}
		}
	}
}


