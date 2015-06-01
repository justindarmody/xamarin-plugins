using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace CardView.Forms.Plugin.iOSUnified
{
    public class L3SDKCardOptions
    {
        public bool IsSwipable { get; set; }
    }

    public interface iOSCardsViewDelegate
    {
        void iOSCardsView_Scrolling(UISwipeGestureRecognizerDirection scrollDirection);

        void iOSCardsView_CardWillRemove(UIView view);

        void iOSCardsView_CardDidlRemove(UIView view);

        void iOSCardsView_AllCardRemoved();

        void iOSCardsView_UpperLimitReached();

        void iOSCardsView_BottomLimitReached();
    }

    public partial class iOSCardsView : UIView
    {
        private static readonly nfloat CARD_Y_MARGIN = 10f;
        private static readonly nfloat CARD_X_MARGIN = 20f;

        public UISwipeGestureRecognizerDirection GestureDirection {get; set;}

        public CGPoint GestureStartPoint {get; set;}

        public UIView GestureView {get; set;}

        public List<L3SDKCardOptions> CardsOptions {get; set;}

        public CGRect SuperviewFrame {get; set;}

        public List<UIView> Cards {get; private set;}

        public iOSCardsViewDelegate TheDelegate {get; set;}

        public CGRect ZeroFrame {get; set;}

        public nfloat CardWidth
        {
            get
            {
                return this.Frame.Size.Width - CARD_X_MARGIN;;
            }
        }

        #region Constructors

        public iOSCardsView (IntPtr handle) : base (handle)
        {
            this.Setup();
        }

        public iOSCardsView(CGRect frame) : base (frame)
        {
            this.Setup();

            this.Draw(frame);
        }

        public iOSCardsView() 
        {
            this.Setup();
        }

        #endregion

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            this.DrawView();
        }

        private void Setup()
        {
            this.BackgroundColor = UIColor.Clear;
            this.Cards = new List<UIView>();
            this.CardsOptions = new List<L3SDKCardOptions>();
        }

        public void AddCard(UIView card)
        {
            this.AddCard(card, new L3SDKCardOptions());
        }

        public void AddCard(UIView card, L3SDKCardOptions options)
        {
            this.Cards.Add(card);
            this.CardsOptions.Add(options);
            this.AddSubview(card);
        }

        public void DrawView()
        {
            this.SuperviewFrame = this.Superview.Frame;
            this.ZeroFrame = this.Frame;
            this.SetupHeight();
            this.SetNeedsDisplay();
        }

        void SetupHeight()
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
            var options = this.CardsOptions[index];

            return options;
        }

        bool ViewCanSwipe(UIView view)
        {
            if (view.IsEqual(this))
            {
                return false;
            }

            L3SDKCardOptions options = this.GetOptionsForView(view);
            if (options.IsSwipable)
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
}

