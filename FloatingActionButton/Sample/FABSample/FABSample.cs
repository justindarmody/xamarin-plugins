using System;
using FAB.Forms;
using Xamarin.Forms;

namespace FABSample
{
    public class App : Application
    {
        public App ()
        {
            var layout = new RelativeLayout ();

            var label = new Label {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Hello Floating Action Button!"
            };
            layout.Children.Add (
                label,
                xConstraint: Constraint.Constant (0),
                yConstraint: Constraint.Constant (0),
                widthConstraint: Constraint.RelativeToParent (parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent (parent => parent.Height)
            );

            var normalFab = new FloatingActionButton ();
            normalFab.Source = "plus.png";
            normalFab.Size = FabSize.Normal;

            layout.Children.Add (
                normalFab,
                xConstraint: Constraint.RelativeToParent ((parent) => { return (parent.Width - normalFab.Width) - 16; }),
                yConstraint: Constraint.RelativeToParent ((parent) => { return (parent.Height - normalFab.Height) - 16; })
            );

            normalFab.SizeChanged += (sender, args) => { layout.ForceLayout (); };

            var miniFab = new FloatingActionButton ();
            miniFab.Source = "plus.png";
            miniFab.Size = FabSize.Mini;

            layout.Children.Add (
                miniFab,
                xConstraint: Constraint.RelativeToParent ((parent) => {
                    return (parent.Width - miniFab.Width) - 16;
                }),
                yConstraint: Constraint.RelativeToView (normalFab, (parent, view) => {
                    return (view.Y - miniFab.Height) - 16;
                })
            );
            miniFab.SizeChanged += (sender, args) => { layout.ForceLayout (); };

            // The root page of your application
            MainPage = new ContentPage {
                BackgroundColor = Color.White,
                Content = layout
            };

            normalFab.Clicked += (sender, e) => {
                MainPage.DisplayAlert ("Floating Action Button", "You clicked the normal FAB!", "Awesome!");
            };

            miniFab.Clicked += (sender, e) => {
                MainPage.DisplayAlert ("Floating Action Button", "You clicked the mini FAB!", "Awesome!");
            };
        }
    }
}