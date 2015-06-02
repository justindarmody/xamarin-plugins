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
            var stack = new CardsView()
            {
                    Padding = new Thickness(Device.OnPlatform(15, 25, 15), Device.OnPlatform(0, 5, 0))
            };

            for (int i = 0; i < 20; i++) {

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

                var card = new CardContentView {
                    Padding = 40,
                    CornderRadius = 10,
                    Content = panel,
                    BackgroundColor = Color.White
                };

                stack.Children.Add (card);
            }

            return new NavigationPage(new ContentPage
                { 
                    Content = stack,
                    BackgroundColor = Color.Gray
                }
            );
        }
    }
}