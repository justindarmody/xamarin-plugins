using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CardView.Forms.Plugin.Abstractions;
using CardView.Forms.Plugin.iOSUnified;
using UIKit;
using System.Collections;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

[assembly: ExportRenderer(typeof(CardsView), typeof(CardsViewRenderer))]

namespace CardView.Forms.Plugin.iOSUnified
{
    public class CardsViewRenderer : VisualElementRenderer<CardsView>
    {
        internal static readonly nfloat CARD_Y_MARGIN = 10f;
        internal static readonly nfloat CARD_X_MARGIN = 20f;

        public UISwipeGestureRecognizerDirection GestureDirection {get; set;}

        public CGPoint GestureStartPoint {get; set;}

        public UIView GestureView {get; set;}

        public List<L3SDKCardOptions> CardsOptions {get; set;}

        public CGRect SuperviewFrame {get; set;}

//        private iOSCardView nativeCardView = false;

        private IList<UIView> Cards {
            get { return this.Subviews.Where(s => s.GetType() == typeof(CardViewRenderer)).ToList(); }
        }

        public iOSCardsViewDelegate TheDelegate {get; set;}

        public CGRect ZeroFrame {get; set;}

        public nfloat CardWidth
        {
            get
            {
                return this.Frame.Size.Width - CARD_X_MARGIN;;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CardsView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                this.Setup();
            }
        }

//        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);
//        }

        public void DrawView()
        {
            this.SuperviewFrame = this.Superview.Frame;
            this.ZeroFrame = this.Frame;
            this.SetupHeight();
            this.SetNeedsDisplay();
        }

        private void Setup()
        {
            this.BackgroundColor = UIColor.Clear;
//            this.Cards = new List<UIView>();
            this.CardsOptions = new List<L3SDKCardOptions>();
            this.CardsOptions.AddRange(this.Element.Children.Select(c => new L3SDKCardOptions
                    {
                        IsSwipable = (c as CardContentView).IsSwipeable
                    }));
        }

        private void SetupHeight()
        {
            nfloat height = 0;
            nfloat y = this.Frame.Location.Y;
            for (int i = 0; i < this.Cards.Count; i++)
            {
                UIView card = this.Cards[i];
                card.Frame = new CGRect(card.Frame.Location.X, height, this.CardWidth, card.Frame.Size.Height);
                card.Center = this.GetCardCenterAndXOffsetAndYOffset(card, 0, CARD_Y_MARGIN);
                height += card.Frame.Size.Height + CARD_Y_MARGIN;
            }

            if (this.Frame.Location.Y < 0)
            {
                if (this.TheDelegate != null)
                {
                    this.TheDelegate.iOSCardsView_Scrolling(UISwipeGestureRecognizerDirection.Down);
                }

            }

            if (this.Cards.Count == 1 && !this.ViewCanSwipe(this.Cards[0]))
            {
                y = this.SuperviewFrame.Size.Height - ((UIView)this.Cards[0]).Frame.Size.Height - (CARD_Y_MARGIN * 2);
            }

            this.Frame = new CGRect(this.Frame.Location.X, this.Frame.Location.Y < 0 ? this.Frame.Location.Y + this.GestureView.Frame.Size.Height : y, this.Frame.Size.Width, height);
        }

        public override void TouchesBegan(NSSet touches, UIEvent theEvent)
        {
            //            base.TouchesBegan(touches, evt);
            UITouch touch = (UITouch)touches.AnyObject;
            this.GestureStartPoint = touch.LocationInView(this);
            this.GestureView = this.HitTest(this.GestureStartPoint, theEvent);
        }

        public override void TouchesMoved(NSSet touches, UIEvent theEvent)
        {
            //            base.TouchesMoved(touches, evt);
            UITouch touch = (UITouch)touches.AnyObject;
            CGPoint gestureEndPoint = touch.LocationInView(this);
            this.GestureDirection = this.GetGestureDirectionWithTouch(touch);
            bool canScroll = this.CanScroll(this.GestureDirection);
            if (canScroll)
            {
                if (this.TheDelegate != null)
                {
                    this.TheDelegate.iOSCardsView_Scrolling(this.GestureDirection);
                }
            }

            if ((this.GestureDirection == UISwipeGestureRecognizerDirection.Up | this.GestureDirection == UISwipeGestureRecognizerDirection.Down) && canScroll)
            {
                var offsetFrame = new CGRect(new CGPoint(this.Frame.X, this.Frame.Location.Y + (gestureEndPoint.Y - this.GestureStartPoint.Y)), this.Frame.Size);

                this.Frame = offsetFrame;
            }
            else if (this.GestureDirection == UISwipeGestureRecognizerDirection.Left | this.GestureDirection == UISwipeGestureRecognizerDirection.Right)
            {
                if (this.GestureView.IsEqual(this) | !this.ViewCanSwipe(this.GestureView))
                {
                    return;
                }

                gestureEndPoint = touch.LocationInView(this.GestureView);

                var offsetFrame = new CGRect(new CGPoint(this.GestureView.Frame.X + (gestureEndPoint.X - this.GestureStartPoint.X), this.GestureView.Frame.Y), this.GestureView.Frame.Size);
                this.GestureView.Frame = offsetFrame;

                if (this.GestureView.Alpha > 0.4)
                {
                    this.GestureView.Alpha = this.GestureView.Alpha - 0.03f;
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent theEvent)
        {
            if (!this.GestureView.IsEqual(this))
            {
                nfloat x = this.GestureView.Frame.Location.X;
                if (Math.Abs(x) > this.Frame.Size.Width / 2)
                {
                    if (this.TheDelegate != null)
                    {
                        this.TheDelegate.iOSCardsView_CardWillRemove(this.GestureView);
                    }

                    this.RemoveView(this.GestureView);
                    if (this.TheDelegate != null)
                    {
                        this.TheDelegate.iOSCardsView_CardDidlRemove(this.GestureView);
                    }

                    if (this.Cards.Count == 0)
                    {
                        if (this.TheDelegate != null)
                        {
                            this.TheDelegate.iOSCardsView_AllCardRemoved();
                        }

                    }

                }
                else
                {
                    this.GestureView.Center = this.GetCardCenter(this.GestureView);
                    this.GestureView.Alpha = 1.0f;
                }

            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            //            base.TouchesCancelled(touches, evt);
        }

        UISwipeGestureRecognizerDirection GetGestureDirectionWithTouch(UITouch touch)
        {
            CGPoint gestureEndPoint = touch.LocationInView(this);
            double dx = Math.Abs(this.GestureStartPoint.X - gestureEndPoint.X);
            nfloat dy = -1 * (gestureEndPoint.Y - this.GestureStartPoint.Y);

            if (dx > 20)
            {
                return UISwipeGestureRecognizerDirection.Right;
            }

            if (dy < 0)
            {
                return UISwipeGestureRecognizerDirection.Down;
            }
            else if (dy > 0)
            {
                return UISwipeGestureRecognizerDirection.Up;
            }

            return UISwipeGestureRecognizerDirection.Down;
        }

        bool CanScroll(UISwipeGestureRecognizerDirection scrollDirection)
        {
            if (scrollDirection == UISwipeGestureRecognizerDirection.Up && this.Frame.Location.Y < 0)
            {
                if (Math.Abs(this.Frame.Location.Y) >= Math.Abs(this.Frame.Size.Height - this.SuperviewFrame.Size.Height))
                {
                    if (this.TheDelegate != null)
                    {
                        this.TheDelegate.iOSCardsView_UpperLimitReached();
                    }

                    return false;
                }

            }
            else if (scrollDirection == UISwipeGestureRecognizerDirection.Down && this.Frame.Location.Y > 0)
            {
                if (Math.Abs(this.Frame.Location.Y) >= this.ZeroFrame.Location.Y)
                {
                    if (this.TheDelegate != null)
                    {
                        this.TheDelegate.iOSCardsView_BottomLimitReached();
                    }

                    return false;
                }

            }

            if ((this.Frame.Size.Height + this.Frame.Location.Y) < this.SuperviewFrame.Size.Height && (this.Frame.Location.Y >= this.ZeroFrame.Location.Y))
            {
                return false;
            }

            return true;
        }

        int GetIndexOfView(UIView view)
        {
            return this.Cards.IndexOf(view);
        }

        void RemoveView(UIView view)
        {
            int index = this.GetIndexOfView(view);
            this.GestureView.RemoveFromSuperview();
            this.Cards.Remove(this.GestureView);
            this.CardsOptions.RemoveAt(index);
            this.SetupHeight();
        }

        L3SDKCardOptions GetOptionsForView(UIView view)
        {
            int index = this.GetIndexOfView(view);
            if (index >= 0)
            {
                var options = this.CardsOptions[index];

                return options;
            }

            return null;
        }

        bool ViewCanSwipe(UIView view)
        {
            if (view.IsEqual(this))
            {
                return false;
            }

            L3SDKCardOptions options = this.GetOptionsForView(view);
            if (options != null && options.IsSwipable)
            {
                return true;
            }

            return false;
        }

        CGPoint GetCardCenter(UIView card)
        {
            return this.GetCardCenterAndXOffsetAndYOffset(card, 0, 0);
        }

        CGPoint GetCardCenterAndXOffsetAndYOffset(UIView card, nfloat xOffset, nfloat yOffset)
        {
            return new CGPoint((this.Frame.Size.Width / 2) + xOffset, card.Center.Y + yOffset);
        }
    }

//    public class CardsViewRenderer : iOSCardsView, IVisualElementRenderer //ViewRenderer<CardsView, iOSCardView>
//    {
//        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
//
//        private bool init;
//
//        public CardsView TheView
//        {
//            get { return this.Element == null ? null : (CardsView)Element; }
//        }
//
//        public VisualElementTracker Tracker
//        {
//            get;
//            private set;
//        }
//
////        public VisualElementPackager Packager
////        {
////            get;
////            private set;
////        }
//
//        public VisualElement Element
//        {
//            get;
//            private set;
//        }
//
//        public UIKit.UIView NativeView
//        {
//            get { return this as UIView; }
//        }
//
//        public UIKit.UIViewController ViewController
//        {
//            get
//            {
//                return null;
//            }
//        }
//
//        public void SetElement(VisualElement element)
//        {
//            var oldElement = this.Element;
//
//            if (oldElement != null)
//            {
//                oldElement.PropertyChanged -= this.HandlePropertyChanged;
//            }
//
//            this.Element = element;
//
//            if (this.Element != null)
//            {
//                this.Element.PropertyChanged += this.HandlePropertyChanged;
//                this.Element.ChildrenReordered += Element_ChildrenReordered;
//                this.Element.ChildRemoved += Element_ChildRemoved;
//                this.Element.ChildAdded += Element_ChildAdded;
//            }
//
//            this.RemoveAllSubviews();
//            //sizes to match the forms view
//            //updates properties, handles visual element properties
////            this.Tracker = new VisualElementTracker(this);
//
////            this.Packager = new VisualElementPackager(this);
////            this.Packager.Load();
//            this.LoadChildren();
//
//            this.SetBackgroundColor(this.TheView.BackgroundColor.ToUIColor());
//
//            if (this.ElementChanged != null)
//            {
//                this.ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
//            }
//
//            (this.Element as Layout).ForceLayout();
//        }
//
//        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
//        {
//            var size = UIViewExtensions.GetSizeRequest(this.NativeView, widthConstraint, heightConstraint, 44.0, 44.0);
//
//            return size;
//        }
//
//        public void SetElementSize(Size size)
//        {
//            this.Element.Layout(new Rectangle(this.Element.X, this.Element.Y, size.Width, size.Height));
//        }
//
//        protected override void Dispose(bool disposing)
//        {
//            base.Dispose(disposing);
//
//            if (disposing)
//            {
//                this.Element.ChildrenReordered -= Element_ChildrenReordered;
//                this.Element.ChildRemoved -= Element_ChildRemoved;
//                this.Element.ChildAdded -= Element_ChildAdded;
//            }
//        }
//
//        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//        }
//
//        private void SetBackgroundColor(UIColor color)
//        {
//            this.BackgroundColor = color;   
//        }
//
//        private void LoadChildren()
//        {
//            var cards = this.Element as CardsView;
//            foreach (var child in cards.Children)
//            {
//                var card = child as CardContentView;
//                if (card == null)
//                {
//                    throw new InvalidCastException("Children of CardsView can only be CardContentView");
//                }
//
//                this.AddChildCard(card);
//            }
//        }
//
//        void AddChildCard(CardContentView card)
//        {
//            var nativeCard = RendererFactory.GetRenderer(card);
//
//            var view = nativeCard.NativeView;
//
//            this.AddCard(view);
////            card.ForceLayout();
//        }
//
//        void Element_ChildAdded (object sender, ElementEventArgs e)
//        {
//            var card = e.Element as CardContentView;
//
//            this.AddChildCard(card);
//        }
//
//        void Element_ChildRemoved (object sender, ElementEventArgs e)
//        {
//            throw new NotImplementedException();
//        }
//
//        void Element_ChildrenReordered (object sender, EventArgs e)
//        {
//            throw new NotImplementedException();
//        }
//    }
}