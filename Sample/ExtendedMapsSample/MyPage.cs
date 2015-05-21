using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ExtendedMapsSample;

namespace ExtendedMaps.Sample
{
	public class MyPage : ContentPage
	{
		public MyPage ()
		{
			var vm = new ViewModel ();

			this.BindingContext = vm;

			var center = GeoHelper.GetCentralPosition (vm.Monkeys.Select (m => m.Location));

			var radius = GeoHelper.GetRadius (center, vm.Monkeys.Select (m => m.Location), true);

			var map = new ExtendedMap (
				MapSpan.FromCenterAndRadius (
					center,
					Distance.FromMeters (radius)
				)
			);
//			map.BindingContext = this.BindingContext;
			map.SetBinding<ViewModel> (ExtendedMap.ItemsSourceProperty, m => m.Monkeys);
			map.SetBinding<ViewModel> (ExtendedMap.SelectedPinProperty, m => m.Selected, BindingMode.TwoWay);
			map.SetBinding<ViewModel> (ExtendedMap.VisibleRegionProperty, m => m.Span, BindingMode.TwoWay);

			var relativeLayout = new RelativeLayout ();

			relativeLayout.Children.Add(
				map, 
				widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
				heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height * 0.5; }));

			var listView = new ListView ();
			listView.RowHeight = 100;
			listView.ItemTemplate = new DataTemplate (typeof(TextCell));
			listView.ItemTemplate.SetBinding (TextCell.TextProperty, "Name");
			listView.SetBinding<ViewModel> (ListView.ItemsSourceProperty, m => m.Monkeys);
			listView.ItemSelected += (sender, e) => {
				vm.Selected = e.SelectedItem as CustomPin;
			};

			relativeLayout.Children.Add(
				listView, 
				yConstraint: Constraint.RelativeToView(map, (parent, view) => { return view.Height + 50; }),
				heightConstraint: Constraint.RelativeToView(map, (parent, view) => { return parent.Height - view.Height - 50; }));

			this.Content = relativeLayout;
		}

		private class ViewModel : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private CustomPin selected;

			public ObservableCollection<CustomPin> Monkeys { get; set; }

			public CustomPin Selected { 
				get { return this.selected; }
				set {
					this.selected = value;
					this.RaisePropertyChanged ();
				}
			}

			public MapSpan Span { get; private set; }

			public ViewModel()
			{
				this.Monkeys = new ObservableCollection<CustomPin>();

				this.Monkeys.Add(new CustomPin {
					Name = "Baboon",
					Location = new Position(34.027897,  118.301869 ),
					Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
				});

				this.Monkeys.Add(new CustomPin {
					Name = "Capuchin Monkey",
					Location = new Position(34.047797,-118.321869 ),
					Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
				});

				this.Monkeys.Add(new CustomPin {
					Name = "Blue Monkey",
					Location = new Position(34.007897, -118.300069 ),
					Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
				});


				this.Monkeys.Add(new CustomPin {
					Name = "Squirrel Monkey",
					Location = new Position(34.107897, -118.292869),
					Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.",
				});

				var center = GeoHelper.GetCentralPosition(this.Monkeys.Select(m => m.Location));
				var radiusInMeters = GeoHelper.GetRadius(center, this.Monkeys.Select(m => m.Location), true);

				this.Span = MapSpan.FromCenterAndRadius(center, Distance.FromMeters(radiusInMeters));
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