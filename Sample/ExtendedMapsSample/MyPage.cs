using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

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

			var relativeLayout = new RelativeLayout ();

			relativeLayout.Children.Add(
				map, 
				widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
				heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height * 0.5; }));

			var listView = new ListView ();
			listView.ItemTemplate = new DataTemplate (typeof(TextCell));
			listView.ItemTemplate.SetBinding (TextCell.TextProperty, "Name");
			listView.SetBinding<ViewModel> (ListView.ItemsSourceProperty, m => m.Monkeys);
			listView.ItemSelected += (sender, e) => {
				vm.Selected = e.SelectedItem as ExtendedPin;
			};

			relativeLayout.Children.Add(
				listView, 
				yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height * 0.5; }),
				heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height * 0.5; }));

			this.Content = relativeLayout;
		}

		private class ViewModel : INotifyPropertyChanged
		{
			private ExtendedPin selected;

			public ObservableCollection<ExtendedPin> Monkeys { get; set; }

			public ExtendedPin Selected { 
				get { return this.selected; }
				set {
					this.selected = value;
					this.RaisePropertyChanged ();
				}
			}

			#region INotifyPropertyChanged implementation
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion

			public ViewModel()
			{
				this.Monkeys = new ObservableCollection<ExtendedMaps.ExtendedPin>();

				this.Monkeys.Add(new ExtendedPin {
					Name = "Baboon",
					Location = new Position(34.027897,  118.301869 ),
					Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
				});

				this.Monkeys.Add(new ExtendedPin {
					Name = "Capuchin Monkey",
					Location = new Position(34.047797,-118.321869 ),
					Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
				});

				this.Monkeys.Add(new ExtendedPin {
					Name = "Blue Monkey",
					Location = new Position(34.007897, -118.300069 ),
					Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
				});


				this.Monkeys.Add(new ExtendedPin {
					Name = "Squirrel Monkey",
					Location = new Position(34.107897, -118.292869),
					Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.",
				});

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

