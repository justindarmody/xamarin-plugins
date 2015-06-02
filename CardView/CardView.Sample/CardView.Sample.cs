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

        public static Page GetMainPage ()
        {   
            var page = new ContentPage()
            {
                BackgroundColor = Color.Gray
            };

            var header = new BoxView
            {
                    BackgroundColor = Color.Blue,
                    Color = Color.Blue
            };

            var wrapper = new CardsView();

            for (int i = 0; i < 1; i++) {

                var panel = new StackLayout();

                panel.Children.Add(new Label
                    {
                        Text = "I am a card: " + i + "!!!",
                        Font = Font.SystemFontOfSize(NamedSize.Large),
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    });
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
                            page.DisplayAlert("Click", "You clicked me!", "Ok");
                        })
                    });

                var card = new CardContentView {
                    Padding = 15,
                    CornderRadius = 5,
                    Content = panel,
                    BackgroundColor = Color.White,
                    Command = new Command(() => {
                        page.DisplayAlert("Alert", "I am a card: " + i + "!!!", "Ok");
                    })
                };

                wrapper.AddCard(card);
            }

//            wrapper.Content = stack;

            var content = new AbsoluteLayout();
            content.Children.Add(header, new Rectangle(0, 0, 1, 0.25), AbsoluteLayoutFlags.SizeProportional);
            content.Children.Add(wrapper, new Rectangle(1, 0.25, 1, 1), AbsoluteLayoutFlags.All);

            page.Content = content;

            return new NavigationPage(page);
        }
    }
}