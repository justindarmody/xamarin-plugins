using System;
using FAB.Forms;
using Xamarin.Forms;

namespace FABSample
{
    public class App : Application
    {
        public App()
        {
            var tabbed = new TabbedPage();

            tabbed.Children.Add(new NavigationPage(new CSharpExample()) { Title = "C#" });
            tabbed.Children.Add(new NavigationPage(new XamlExample()) { Title = "Xaml" });

            this.MainPage = tabbed;
        }
    }
}