using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using CardView.Forms.Plugin.Abstractions;

[assembly:ExportRendererAttribute (typeof(CardContentView), typeof(CardView.Forms.Plugin.Droid.CardViewRenderer))]

namespace CardView.Forms.Plugin.Droid
{
    public class CardViewRenderer : Android.Support.V7.Widget.CardView,  IVisualElementRenderer
    {
        public CardViewRenderer () : base (Xamarin.Forms.Forms.Context)
        {
            
        }

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        bool init;

        public void SetElement (VisualElement element)
        {
            var oldElement = this.Element;

            if (oldElement != null)
                oldElement.PropertyChanged -= HandlePropertyChanged;

            this.Element = element;
            if (this.Element != null) {
                //UpdateContent ();
                this.Element.PropertyChanged += HandlePropertyChanged;
            }

            this.ViewGroup.RemoveAllViews ();
            //sizes to match the forms view
            //updates properties, handles visual element properties
            this.Tracker = new VisualElementTracker (this);

            this.Packager = new VisualElementPackager (this);
            this.Packager.Load ();

            this.UseCompatPadding = true;

            this.SetContentPadding ((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);

            this.Radius = TheView.CornderRadius;

            this.SetCardBackgroundColor(TheView.BackgroundColor.ToAndroid ());
            if (this.ElementChanged != null)
            {
                this.ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
            }

			this.Elevation = 8;
			this.CardElevation = 8;
        }

        public CardContentView TheView 
        {
            get { return this.Element == null ? null : (CardContentView)Element; }
        }

        private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content") 
            {
                this.Tracker.UpdateLayout ();
            } 
            else if (e.PropertyName == CardContentView.PaddingProperty.PropertyName) 
            {
                this.SetContentPadding ((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);
            } 
            else if (e.PropertyName == CardContentView.CornerRadiusProperty.PropertyName) 
            {
                this.Radius = this.TheView.CornderRadius;
            } 
            else if (e.PropertyName == CardContentView.BackgroundColorProperty.PropertyName) 
            {
                if (this.TheView.BackgroundColor != null)
                {
                    this.SetCardBackgroundColor(this.TheView.BackgroundColor.ToAndroid());
                }
            }
        }

        public SizeRequest GetDesiredSize (int widthConstraint, int heightConstraint)
        {
            this.Measure (widthConstraint, heightConstraint);

            //Measure child here and determine size
            return new SizeRequest (new Size (this.MeasuredWidth, this.MeasuredHeight));
        }

        public void UpdateLayout ()
        {
            if (Tracker == null)
            {
                return;
            }

            Tracker.UpdateLayout ();
        }

        public VisualElementTracker Tracker 
        {
            get;
            private set;
        }

        public VisualElementPackager Packager 
        {
            get;
            private set;
        }

        public Android.Views.ViewGroup ViewGroup 
        {
            get{ return this; }
        }

        public VisualElement Element 
        {
            get;
            private set;
        }
    }
}