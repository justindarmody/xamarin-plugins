using System;

using Xamarin.Forms;
using CardView.Forms.Plugin.Abstractions;

namespace CardView.Sample
{
    public class App : Application
    {
        public App()
        {
            this.MainPage = GetMainPage();
        }

//        public static Page GetMainPage ()
//        {   
//            var page = new ContentPage()
//            {
//				BackgroundColor = Color.White
//            };

//            //var header = new BoxView
//            //{
//            //        BackgroundColor = Color.White,//.Blue,
//            //        Color = Color.Blue
//            //};

//            var wrapper = new CardsView();

//            for (int i = 0; i < 50; i++) {

//                var panel = new StackLayout();

//                panel.Children.Add(new Label
//                    {
//                        Text = "I am a card: " + i + "!!!",
//                        Font = Font.SystemFontOfSize(NamedSize.Large),
//                        VerticalOptions = LayoutOptions.Center,
//                        TextColor = Color.Black
//                    });
//                panel.Children.Add(new Label
//                    {
//                        Text = "Row 2",
//                        Font = Font.SystemFontOfSize(NamedSize.Large),
//                        VerticalOptions = LayoutOptions.Center,
//                        TextColor = Color.Black
//                    });

//                panel.Children.Add(new Button
//                    {Text = "Click Me",
//                        Command = new Command(() => {
//                            page.DisplayAlert("Click", "You clicked me!", "Ok");
//                        })
//                    });

//                var card = new CardContentView {
//                    Padding = 15,
//                    CornderRadius = 5,
//                    Content = panel,
//                    BackgroundColor = Color.White,
//                    Command = new Command(() => {
//                        page.DisplayAlert("Alert", "I am a card: " + i + "!!!", "Ok");
//                    })
//                };

//                wrapper.AddCard(card);
//            }

////            wrapper.Content = stack;

//            var content = new AbsoluteLayout();
//            //content.Children.Add(header, new Rectangle(0, 0, 1, 0.25), AbsoluteLayoutFlags.SizeProportional);
//            content.Children.Add(wrapper, new Rectangle(1, 0.25, 1, 1), AbsoluteLayoutFlags.All);

//            page.Content = content;

//            return new NavigationPage(page);
//        }

		public static Page GetMainPage()
		{
			var listView = new ListView ();

			listView.RowHeight = 160;
			listView.ItemsSource = new [] { "a", "b", "c" };
			listView.ItemTemplate = new DataTemplate (typeof (CustomCell));

			//listView.ItemTapped += async (sender, e) => {
			//	await DisplayAlert ("Tapped", e.Item + " row was tapped", "OK");
			//	((ListView)sender).SelectedItem = null; // de-select the row
			//};

			var page = new ContentPage () {
				BackgroundColor = Color.FromHex("#F8F6F1"),//.White,
				Padding = new Thickness (0, 20, 0, 0),
				Content = listView
			};
			return new NavigationPage (page);
		}

		public class ListButton : Button { }
		public class CustomCell : ViewCell
		{
			public CustomCell ()
			{
				var panel = new StackLayout ();

                var label1 = new Label
                    {
                        //Text = "I am a card: " + i + "!!!",
                        Font = Font.SystemFontOfSize(NamedSize.Large),
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    };
				label1.SetBinding (Label.TextProperty, new Binding ("."));
				panel.Children.Add (label1);

                panel.Children.Add(new Label
                    {
                        Text = "Row 2",
                        Font = Font.SystemFontOfSize(NamedSize.Large),
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    });

                panel.Children.Add(new Button
                    {Text = "Click Me",
                        Command = new Command(() => {
                            //page.DisplayAlert("Click", "You clicked me!", "Ok");
                        })
                    });

                var card = new CardContentView {
                    Padding = 15,
                    CornderRadius = 2,
                    Content = panel,
					BackgroundColor = Color.White,//Color.FromHex("#F8F6F1"),
					Command = new Command(() => {
                        //page.DisplayAlert("Alert", "I am a card: " + i + "!!!", "Ok");
                    }),
					HasShadow = true,

                };

				var frame = new StackLayout {
					Padding = 15
				};
				frame.Children.Add (card);

				View = frame;
			}
		}
    }
}