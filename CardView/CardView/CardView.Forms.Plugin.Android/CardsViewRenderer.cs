using System;
using System.Linq;
using CardView.Forms.Plugin.Abstractions;
using Xamarin.Forms.Platform.Android;
using CardView.Forms.Plugin.Droid;
using Xamarin.Forms;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using System.ComponentModel;
using Android.App;
using Android.Content;
using System.Collections.ObjectModel;

//[assembly: ExportRenderer(typeof(CardsView), typeof(CardsViewRenderer))]

namespace CardView.Forms.Plugin.Droid
{
    public class CardsViewRenderer  : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }
    }
}