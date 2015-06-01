using System;
using Xamarin.Forms;

namespace CardView.Forms.Plugin.Abstractions
{
    /// <summary>
    /// CardContentView Interface
    /// </summary>
    public class CardContentView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<CardContentView,float> ( p => p.CornderRadius, 3.0F);   
        public static readonly BindableProperty IsSwipeableProperty = BindableProperty.Create<CardContentView,bool> ( p => p.IsSwipeable, true);   

        public float CornderRadius 
        {
            get { return (float)GetValue (CornerRadiusProperty); } 
            set { SetValue (CornerRadiusProperty, value); } 
        }

        public bool IsSwipeable
        {
            get { return (bool)GetValue (IsSwipeableProperty); } 
            set { SetValue (IsSwipeableProperty, value); } 
        }

        protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
        {
            return base.OnSizeRequest(widthConstraint, heightConstraint);
        }
    }
}
