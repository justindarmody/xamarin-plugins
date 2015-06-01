using System;
using CardView.Forms.Plugin.Abstractions;
using Xamarin.Forms.Platform.Android;
using CardView.Forms.Plugin.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CardsView), typeof(CardsViewRenderer))]

namespace CardView.Forms.Plugin.Droid
{
    public class CardsViewRenderer  : VisualElementRenderer<CardsView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CardsView> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}