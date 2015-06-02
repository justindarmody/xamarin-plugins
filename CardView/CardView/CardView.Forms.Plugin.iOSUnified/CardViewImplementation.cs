using CardView.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using CardView.Forms.Plugin.iOSUnified;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(CardContentView), typeof(CardViewRenderer))]

namespace CardView.Forms.Plugin.iOSUnified
{
    /// <summary>
    /// CardView Renderer
    /// </summary>
    public class CardViewRenderer : iOSCardView, IVisualElementRenderer
    {
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        private bool init;

        public CardContentView TheView
        {
            get { return this.Element == null ? null : (CardContentView)Element; }
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

        public VisualElement Element
        {
            get;
            private set;
        }

        public UIKit.UIView NativeView
        {
            get { return this as UIView; }
        }

        public UIKit.UIViewController ViewController
        {
            get
            {
                return null;
            }
        }

        public void SetElement(VisualElement element)
        {
            var oldElement = this.Element;

            if (oldElement != null)
            {
                oldElement.PropertyChanged -= this.HandlePropertyChanged;
            }

            this.Element = element;

            if (this.Element != null)
            {
                this.Element.PropertyChanged += this.HandlePropertyChanged;
            }

            this.RemoveAllSubviews();
            //sizes to match the forms view
            //updates properties, handles visual element properties
            this.Tracker = new VisualElementTracker(this);

            this.Packager = new VisualElementPackager(this);
            this.Packager.Load();

            this.SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);

            this.SetCardBackgroundColor(this.TheView.BackgroundColor.ToUIColor());

            if (ElementChanged != null)
            {
                this.ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
            }
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var size = UIViewExtensions.GetSizeRequest(this.NativeView, widthConstraint, heightConstraint, 44.0, 44.0);

            return size;
        }

        public void SetElementSize(Size size)
        {
            this.Element.Layout(new Rectangle(this.Element.X, this.Element.Y, size.Width, size.Height));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Packager == null)
                {
                    return;
                }
                
                this.SetElement((VisualElement) null);
                this.Packager.Dispose();
                this.Packager = (VisualElementPackager) null;
                this.Tracker.Dispose();
                this.Tracker = (VisualElementTracker) null;
            }

            base.Dispose(disposing);
        }

        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
////                Tracker.UpdateLayout ();
            }
            else if (
                e.PropertyName == CardContentView.WidthProperty.PropertyName ||
                e.PropertyName == CardContentView.HeightProperty.PropertyName ||
                e.PropertyName == CardContentView.XProperty.PropertyName ||
                e.PropertyName == CardContentView.YProperty.PropertyName ||
                e.PropertyName == CardContentView.CornerRadiusProperty.PropertyName)
            {
                this.Element.Layout(this.Element.Bounds);

                var radius = (this.Element as CardContentView).CornderRadius;
                var bound = this.Element.Bounds;
                this.DrawBorder(new CoreGraphics.CGRect(bound.X, bound.Y, bound.Width, bound.Height), (nfloat)radius);
            }
            else if (e.PropertyName == CardContentView.PaddingProperty.PropertyName)
            {
                SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);
            }
            else if (e.PropertyName == CardContentView.BackgroundColorProperty.PropertyName)
            {
                if (TheView.BackgroundColor != null)
                {
                    SetCardBackgroundColor(TheView.BackgroundColor.ToUIColor());
                }
            }
        }

        private void SetCardBackgroundColor(UIColor color)
        {
            this.BackgroundColor = color;   
        }

        private void SetContentPadding(int left, int top, int right, int bottom)
        {
         // TODO: maybe??   
        }
    }
}
