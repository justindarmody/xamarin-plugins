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
            layout.Children.Add(
                label,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height)
            );
                        
            var normalFab = new FAB.Forms.FloatingActionButton();
            normalFab.Source = "ic_add_white_24dp.png";
            normalFab.Size = FabSize.Normal;

            layout.Children.Add(
                normalFab,
                xConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Width - normalFab.Width) - 16; }),
                yConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Height - normalFab.Height) - 16; })
            );

            normalFab.SizeChanged += (sender, args) => { layout.ForceLayout(); };

            var miniFab = new FAB.Forms.FloatingActionButton();
            miniFab.Source = "ic_add_white_24dp.png";
            miniFab.Size = FabSize.Mini;

            layout.Children.Add(
                miniFab,
                xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - miniFab.Width) - 16;
                    }),
                yConstraint: Constraint.RelativeToView(normalFab, (parent, view) =>
                    {
                        return (view.Y - miniFab.Height) - 16;
                    })
            );
            miniFab.SizeChanged += (sender, args) => { layout.ForceLayout(); };

            // The root page of your application
            MainPage = new ContentPage
            {
                BackgroundColor = Color.White,
                Content = layout
            };

            normalFab.Clicked += (sender, e) => 
                {
                    MainPage.DisplayAlert("Floating Action Button", "You clicked the normal FAB!", "Awesome!");
                };

            miniFab.Clicked += (sender, e) => 
                {
                    MainPage.DisplayAlert("Floating Action Button", "You clicked the mini FAB!", "Awesome!");
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

