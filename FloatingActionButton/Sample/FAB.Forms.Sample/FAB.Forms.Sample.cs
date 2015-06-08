using System;

using Xamarin.Forms;

namespace FAB.Forms.Sample
{
    public class App : Application
    {
        public App()
        {
            var layout = new RelativeLayout();

            var label = new Label
            {
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center,
                Text = "Hello Floating Action Button!"
            };
                        
            var fab = new FAB.Forms.FloatingActionButton();
            fab.Source = "ic_add_white_24dp.png";

            layout.Children.Add(
                label,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height)
            );

            layout.Children.Add(
                fab,
                xConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Width - fab.Width) - 16; }),
                yConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Height - fab.Height) - 16; })
            );

            fab.SizeChanged += (sender, args) => { layout.ForceLayout(); };

            // The root page of your application
            MainPage = new ContentPage
            {
                BackgroundColor = Color.White,
                Content = layout
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

