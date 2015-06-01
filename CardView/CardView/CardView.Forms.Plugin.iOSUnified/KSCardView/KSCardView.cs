using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace CardView.Forms.Plugin.iOSUnified
{
    public class KSCardView
    {
        private static CGRect s_cardFrame;
        private static KSCardView s_overlayContainer;

        static bool s_hasLeftOverlay = false;
        static bool s_hasRightOverlay = false;
        static bool s_hasUpOverlay = false;
        static bool s_hasDownOverlay = false;

//        protected CGContextRef _ctx;
        public KSCardViewDelegate TheDelegate {get; set;}

        public bool AllowLeft {get; set;}

        public bool AllowRight {get; set;}

        public bool AllowUp {get; set;}

        public bool AllowDown {get; set;}

        public CGRect OriginalRect {get; set;}

        public CGPoint OriginalCenter {get; set;}

        public uint TouchCount {get; set;}

        public CGPoint Shift {get; set;}

        public nfloat AntiShift {get; set;}

        public bool FirstEdgeHit {get; set;}

        public bool MoveLaterally {get; set;}

        public uint LastDirection {get; set;}

        public static void SetCardViewFrame(CGRect frame)
        {
            s_cardFrame = frame;
            s_overlayContainer = new UIView(new CGRect(0, 0, s_cardFrame.Size.Width, s_cardFrame.Size.Height));
        }

        public static void SetOverlayLeftRightUpDown(UIView leftOverlay, UIView rightOverlay, UIView upOverlay, UIView downOverlay)
        {
            KSCardView._addOverlayWithDirection(leftOverlay, KSDirection.Left);
            KSCardView._addOverlayWithDirection(rightOverlay, KSDirection.Right);
            KSCardView._addOverlayWithDirection(upOverlay, KSDirection.Up);
            KSCardView._addOverlayWithDirection(downOverlay, KSDirection.Down);
        }

        public KSCardView()
        {
            if (s_cardFrame.Size.Height == 0.0f && s_cardFrame.Size.Width == 0.0f)
            {
                string desc = "You must call +setCardViewFrame first in order to " "initialize this object";
                throw new Exception("No Frame Specified", "%@", desc, null);
            }

            this.OriginalRect = s_cardFrame;
            this = base.initWithFrame(this.OriginalRect);
            if (this)
            {
                this.AllowUp = true, this.AllowDown = true, this.AllowLeft = true, this.AllowRight = true;
                this.OriginalCenter = this.Center;
                this.AntiShift = 0.0f;
                this.LastDirection = DirectionCount;
                this.MultipleTouchEnabled = true;
                this.FirstEdgeHit = true;
            }
        }
        public void ShowFromLeft()
        {
            this.Layer.Opacity = 0.0f;
            this.Center = new CGPoint(-this.Frame.Size.Width / 2, this.Superview.Center.Y);
            this.Transform = CGAffineTransformMakeRotation(-Constants.KStartRotation * Math.PI / 180);
            UIView.AnimateWithDurationAnimations(0.5f, delegate()
                {
                    this.Layer.Opacity = 1.0f;
                    this.Transform = CGAffineTransformMakeRotation(0);
                    this.Center = this.OriginalCenter;
                });
        }

        public void ShowFromRight()
        {
            this.Layer.Opacity = 0.0f;
            this.Center = new CGPoint(1.5 * this.Frame.Size.Width, this.Superview.Center.Y);
            this.Transform = CGAffineTransformMakeRotation(Constants.KStartRotation * Math.PI / 180);
            UIView.AnimateWithDurationAnimations(0.5f, delegate()
                {
                    this.Layer.Opacity = 1.0f;
                    this.Transform = CGAffineTransformMakeRotation(0);
                    this.Center = this.OriginalCenter;
                });
        }

        public void ShowFromTop()
        {
            this.Layer.Opacity = 0.0f;
            this.Center = new CGPoint(this.Superview.Center.X, -this.Superview.Frame.Size.Height / 2);
            UIView.AnimateWithDurationAnimations(0.5f, delegate()
                {
                    this.Layer.Opacity = 1.0f;
                    this.Center = this.OriginalCenter;
                });
        }

        public void ShowFromBottom()
        {
            this.Layer.Opacity = 0.0f;
            this.Center = new CGPoint(this.Superview.Center.X, 1.5 * this.Superview.Frame.Size.Height);
            UIView.AnimateWithDurationAnimations(0.5f, delegate()
                {
                    this.Layer.Opacity = 1.0f;
                    this.Center = this.OriginalCenter;
                });
        }

        public void DemoUp()
        {
            CGPoint demoShift = new CGPoint(this.Center.X, Constants.KVerticalEdgeOffset);
            this.AddSubview(s_overlayContainer);
            UIView sub = null;
            foreach (object sub in s_overlayContainer.Subviews())
            {
                if (sub.Tag == KSViewTag.UpImage)
                {
                    break;
                }

            }
            UIView.AnimateWithDurationAnimations(1.5f, delegate()
                {
                    this.Center = demoShift;
                    this.Alpha = 0.5f;
                    if (sub) sub.Alpha = 1.0f;

                });
        }

        public void DemoDown()
        {
            CGPoint demoShift = new CGPoint(this.Center.X, this.Superview.Frame.Size.Height - Constants.KVerticalEdgeOffset);
            this.AddSubview(s_overlayContainer);
            UIView sub = null;
            foreach (object sub in s_overlayContainer.Subviews())
            {
                if (sub.Tag == KSViewTag.DownImage)
                {
                    break;
                }

            }
            UIView.AnimateWithDurationAnimations(1.5f, delegate()
                {
                    this.Center = demoShift;
                    this.Alpha = 0.5f;
                    if (sub) sub.Alpha = 1.0f;

                });
        }

        public void DemoLeft()
        {
            CGPoint demoShift = new CGPoint(Constants.KHorizontalEdgeOffset, this.Center.Y);
            UIView sub = null;
            if (s_hasLeftOverlay)
            {
                this.AddSubview(s_overlayContainer);
                foreach (object sub in s_overlayContainer.Subviews())
                {
                    if (sub.Tag == KSViewTag.LeftImage)
                    {
                        break;
                    }

                }
            }

            UIView.AnimateWithDurationAnimations(1.5f, delegate()
                {
                    this.Center = demoShift;
                    this.Alpha = 0.5f;
                    if (s_hasLeftOverlay)
                    {
                        if (sub) sub.Alpha = 1.0f;

                    }
                    else
                    {
                        this.Transform = CGAffineTransformMakeRotation(-Constants.KStartRotation * Math.PI * Constants.KRotationFactor / 180);
                    }

                });
        }

        public void DemoRight()
        {
            CGPoint demoShift = new CGPoint(this.Superview.Frame.Size.Width - Constants.KHorizontalEdgeOffset, this.Center.Y);
            UIView sub = null;
            if (s_hasRightOverlay)
            {
                this.AddSubview(s_overlayContainer);
                foreach (object sub in s_overlayContainer.Subviews())
                {
                    if (sub.Tag == KSViewTag.RightImage)
                    {
                        break;
                    }

                }
            }

            UIView.AnimateWithDurationAnimations(1.5f, delegate()
                {
                    this.Center = demoShift;
                    this.Alpha = 0.5f;
                    if (s_hasRightOverlay)
                    {
                        if (sub) sub.Alpha = 1.0f;

                    }
                    else
                    {
                        this.Transform = CGAffineTransformMakeRotation(Constants.KStartRotation * Math.PI * Constants.KRotationFactor / 180);
                    }

                });
        }

        public void DemoReset()
        {
            UIView.AnimateWithDurationAnimations(0.25f, delegate()
                {
                    this.Center = this.OriginalCenter;
                    this.Alpha = 1.0f;
                    this.Transform = CGAffineTransformMakeRotation(0);
                    foreach (UIView view in s_overlayContainer.Subviews())
                    {
                        switch (view.Tag)
                        {
                            case KSViewTag.UpImage :
                            case KSViewTag.DownImage :
                            case KSViewTag.LeftImage :
                            case KSViewTag.RightImage :
                                view.Alpha = 0.0f;
                                break;
                            default :
                                break;
                        }

                    }
                });
        }

        public void LeaveLeft()
        {
            this._cardLeavesWithRotation(KSDirection.Left, true);
            this.TheDelegate.CardDidLeaveLeftEdge(this);
        }

        public void LeaveRight()
        {
            this._cardLeavesWithRotation(KSDirection.Right, true);
            this.TheDelegate.CardDidLeaveRightEdge(this);
        }

        public void LeaveTop()
        {
            this._cardLeavesWithRotation(KSDirection.Up, false);
            this.TheDelegate.CardDidLeaveTopEdge(this);
        }

        public void LeaveBottom()
        {
            this._cardLeavesWithRotation(KSDirection.Down, false);
            this.TheDelegate.CardDidLeaveBottomEdge(this);
        }

        void TouchesBeganWithEvent(NSSet touches, UIEvent theEvent)
        {
            if (touches.Count == 1)
            {
                this.TouchCount = 0;
                this.AddSubview(s_overlayContainer);
            }

        }
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
        }

        void TouchesMovedWithEvent(NSSet touches, UIEvent theEvent)
        {
            if (touches.Count == 1)
            {
                this.TouchCount++;
                if (this.TouchCount == 3)
                {
                    if (Math.Abs(this.Center.X - this.OriginalCenter.X) > Math.Abs(this.Center.Y - this.OriginalCenter.Y)) this.MoveLaterally = true;
                    else this.MoveLaterally = false;

                    this.FirstEdgeHit = true;
                }

                UITouch touch = touches.AllObjects().FirstObject();
                CGPoint center = this.Center;
                CGPoint currentLoc = touch.LocationInView(this);
                CGPoint prevLoc = touch.PreviousLocationInView(this);
                if (this.TouchCount < 3)
                {
                    center.X += (currentLoc.X - prevLoc.X);
                    center.Y += (currentLoc.Y - prevLoc.Y);
                }
                else
                {
                    if (this.MoveLaterally)
                    {
                        if (currentLoc.X - prevLoc.X < 0.0f && !this.AllowLeft) return;
                        else if (currentLoc.X - prevLoc.X > 0.0f && !this.AllowRight) return;

                        center.X += (currentLoc.X - prevLoc.X);
                    }
                    else
                    {
                        if (currentLoc.Y - prevLoc.Y < 0.0f && !this.AllowUp) return;
                        else if (currentLoc.Y - prevLoc.Y > 0.0f && !this.AllowDown) return;

                        center.Y += (currentLoc.Y - prevLoc.Y);
                    }

                }

                this.Center = center;
                if (this.MoveLaterally)
                {
                    if ((this.Center.X + this.Frame.Size.Width / 2) > this.Superview.Frame.Size.Width)
                    {
                        this._resetRotation(KSDirection.Right);
                        this.LastDirection = KSDirection.Right;
                        if (this.FirstEdgeHit)
                        {
                            this.FirstEdgeHit = false;
                            this.Shift = new CGPoint(0, 0);
                        }

                        this.Shift.X += (currentLoc.X - prevLoc.X);
                        this._changeViewOpacityForDirection(KSDirection.Right);
                        if (s_hasRightOverlay)
                        {
                            this._showOverlayWithDirectionCurrentLocationPreviousLocation(KSDirection.Right, currentLoc, prevLoc);
                            return;
                        }

                        this.Transform = CGAffineTransform.MakeRotation(Constants.KRotationFactor * this.Shift.X * Math.PI / 180);
                    }
                    else if ((this.Center.X - this.Frame.Size.Width / 2) < 0)
                    {
                        this._resetRotation(KSDirection.Left);
                        this.LastDirection = KSDirection.Left;
                        if (this.FirstEdgeHit)
                        {
                            this.FirstEdgeHit = false;
                            this.Shift = new CGPoint(0, 0);
                        }

                        this.Shift.X += (currentLoc.X - prevLoc.X);
                        this._changeViewOpacityForDirection(KSDirection.Left);
                        if (s_hasLeftOverlay)
                        {
                            this._showOverlayWithDirectionCurrentLocationPreviousLocation(KSDirection.Left, currentLoc, prevLoc);
                            return;
                        }

                        this.Transform = CGAffineTransform.MakeRotation(Constants.KRotationFactor * this.Shift.X * Math.PI / 180);
                    }
                    else
                    {
                        this.Transform = CGAffineTransform.MakeRotation(0);
                        this.Layer.Opacity = 1.0f;
                        this._hideViewOverlays();
                    }

                }
                else
                {
                    if ((this.Center.Y + this.Frame.Size.Height / 2) > this.Superview.Frame.Size.Height)
                    {
                        if (this.FirstEdgeHit)
                        {
                            this.FirstEdgeHit = false;
                            this.Shift = new CGPoint(0, 0);
                        }

                        this.Shift.Y += (currentLoc.Y - prevLoc.Y);
                        this._changeViewOpacityForDirection(KSDirection.Down);
                        this._showOverlayWithDirectionCurrentLocationPreviousLocation(KSDirection.Down, currentLoc, prevLoc);
                    }
                    else if ((this.Center.Y - this.Frame.Size.Height / 2) < 0)
                    {
                        if (this.FirstEdgeHit)
                        {
                            this.FirstEdgeHit = false;
                            this.Shift = new CGPoint(0, 0);
                        }

                        this.Shift.Y += (currentLoc.Y - prevLoc.Y);
                        this._changeViewOpacityForDirection(KSDirection.Up);
                        this._showOverlayWithDirectionCurrentLocationPreviousLocation(KSDirection.Up, currentLoc, prevLoc);
                    }
                    else
                    {
                        this._hideViewOverlays();
                    }

                }

            }

        }

        void TouchesEndedWithEvent(NSSet touches, UIEvent theEvent)
        {
            if (touches.Count == 1)
            {
                this.TouchCount = 0;
                this.FirstEdgeHit = true;
                CGSize superSize = this.Superview.Frame.Size;
                bool outRight = false, outLeft = false, outTop = false, outBottom = false;
                if (this.Center.X > (superSize.Width - Constants.KHorizontalEdgeOffset))
                {
                    outRight = true;
                    this._cardLeavesWithRotation(KSDirection.Right, false);
                    return this.TheDelegate.CardDidLeaveRightEdge(this);
                }
                else if ((this.Center.X - Constants.KHorizontalEdgeOffset) < 0.0f)
                {
                    outLeft = true;
                    this._cardLeavesWithRotation(KSDirection.Left, false);
                    return this.TheDelegate.CardDidLeaveLeftEdge(this);
                }
                else if (this.Center.Y > (superSize.Height - Constants.KVerticalEdgeOffset))
                {
                    outBottom = true;
                    this._cardLeavesWithRotation(KSDirection.Down, false);
                    return this.TheDelegate.CardDidLeaveBottomEdge(this);
                }
                else if ((this.Center.Y - Constants.KVerticalEdgeOffset) < 0.0f)
                {
                    outTop = true;
                    this._cardLeavesWithRotation(KSDirection.Up, false);
                    return this.TheDelegate.CardDidLeaveTopEdge(this);
                }

                if (!outRight && !outLeft && !outTop && !outBottom)
                {
                    this._rubberBand();
                }

            }

        }

        void TouchesCancelledWithEvent(NSSet touches, UIEvent theEvent)
        {
            if (touches.Count == 1)
            {
                this.TouchCount = 0;
                this.FirstEdgeHit = true;
            }
        }


        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            foreach (UIView view in this.Subviews)
            {
                if (view.IsKindOfClass(typeof(UIScrollView)))
                {
                    if (view.PointInside(point, theEvent))
                    {
                        return view.HitTest(point, theEvent);
                    }

                }

            }
            return base.HitTest(point, theEvent);
        }

        public static void _addOverlayWithDirection(UIView overlay, KSDirection direction)
        {
            if (!overlay) return;

            KSViewTag viewTag;
            switch (direction)
            {
                case KSDirection.Left :
                    s_hasLeftOverlay = true;
                    viewTag = KSViewTag.LeftImage;
                    break;
                case KSDirection.Right :
                    s_hasRightOverlay = true;
                    viewTag = KSViewTag.RightImage;
                    break;
                case KSDirection.Down :
                    s_hasDownOverlay = true;
                    viewTag = KSViewTag.DownImage;
                    break;
                case KSDirection.Up :
                    s_hasUpOverlay = true;
                    viewTag = KSViewTag.UpImage;
                    break;
                default :
                    break;
            }

            overlay.Layer.Opacity = 0.0f;
            overlay.Tag = viewTag;
            s_overlayContainer.AddSubview(overlay);
        }

        public void _rubberBand()
        {
            CGPoint cardCenter = this.Center;
            bool isNegative = true;
            bool isVertical = true;
            if (!this.MoveLaterally)
            {
                if (Math.Abs(cardCenter.Y - this.OriginalCenter.Y) > Math.Abs(cardCenter.X - this.OriginalCenter.X))
                {
                    if (cardCenter.Y < this.OriginalCenter.Y)
                    {
                        isNegative = false;
                        isVertical = true;
                    }
                    else
                    {
                        isNegative = true;
                        isVertical = true;
                    }

                }

            }
            else
            {
                if (cardCenter.X < this.OriginalCenter.X)
                {
                    isNegative = false;
                    isVertical = false;
                }
                else
                {
                    isNegative = true;
                    isVertical = false;
                }

            }

            UIView.Animate(Constants.KRubberBandDuration / 3.0f, 0.0f, UIViewAnimationCurve.EaseInOut, () =>
                {
                    CGPoint center = this.OriginalCenter;
                    if (!isNegative && isVertical) center.Y += Constants.KRubberBandFirstPass;
                    else if (isNegative && isVertical) center.Y -= Constants.KRubberBandFirstPass;
                    else if (!isNegative && !isVertical) center.X += Constants.KRubberBandFirstPass;
                    else if (isNegative && !isVertical) center.X -= Constants.KRubberBandFirstPass;

                    this.Center = center;
                    this.Layer.Opacity = 1.0f;
                    this.Transform = CGAffineTransform.MakeRotation(0);
                    this._hideViewOverlays();
                }, (bool finished) =>
                {
                    UIView.Animate(Constants.KRubberBandDuration / 3.0f, () =>
                        {
                            CGPoint center = this.OriginalCenter;
                            if (!isNegative && isVertical) center.Y -= Constants.KRubberBandSecondPass;
                            else if (isNegative && isVertical) center.Y += Constants.KRubberBandSecondPass;
                            else if (!isNegative && !isVertical) center.X -= Constants.KRubberBandSecondPass;
                            else if (isNegative && !isVertical) center.X += Constants.KRubberBandSecondPass;

                            this.Center = center;
                        }, (bool finished2) =>
                        {
                            UIView.Animate(Constants.KRubberBandDuration / 3.0f, () =>
                                {
                                    this.Center = this.OriginalCenter;
                                });
                        });
                });
        }

        public void _hideViewOverlays()
        {
            foreach (UIView view in s_overlayContainer.Subviews)
            {
                switch (view.Tag)
                {
                    case KSViewTag.DownImage :
                    case KSViewTag.UpImage :
                    case KSViewTag.LeftImage :
                    case KSViewTag.RightImage :
                        view.Layer.Opacity = 0.0f;
                    default :
                        break;
                }

            }
        }

        public void _cardLeavesWithRotation(uint direction, bool shouldRotate)
        {
            CGPoint end = new CGPoint(0, 0);
            switch (direction)
            {
                case KSDirection.Up :
                    end = new CGPoint(this.Center.X, -this.Frame.Size.Height);
                    break;
                case KSDirection.Down :
                    end = new CGPoint(this.Center.X, 2 * this.Frame.Size.Height);
                    break;
                case KSDirection.Left :
                    end = new CGPoint(-this.Frame.Size.Width, this.Center.Y);
                    break;
                case KSDirection.Right :
                    end = new CGPoint(1.5 * this.Frame.Size.Width, this.Center.Y);
                    break;
                default :
                    break;
            }

            UIView.Animate(Constants.KCardLeavesDuration, () =>
                {
                    this.Center = end;
                    this.Layer.Opacity = 0.0f;
                    if (shouldRotate)
                    {
                        this.Transform = CGAffineTransform.MakeRotation(-Constants.KStartRotation * Math.PI / 180);
                    }

                });
            this._hideViewOverlays();
        }

        public void _showOverlayWithDirectionCurrentLocationPreviousLocation(uint direction, CGPoint currentLoc, CGPoint prevLoc)
        {
            if (this.FirstEdgeHit)
            {
                this.FirstEdgeHit = false;
                this.Shift = new CGPoint(0, 0);
            }

            if (direction == KSDirection.Up || direction == KSDirection.Down)
            {
                this.Shift.Y += (currentLoc.Y - prevLoc.Y);
            }
            else
            {
                this.Shift.X += (currentLoc.X - prevLoc.X);
            }

            foreach (UIView view in s_overlayContainer.Subviews)
            {
                if (view.Tag == KSViewTag.DownImage && direction == KSDirection.Down)
                {
                    view.Layer.Opacity = (Constants.KOverlayOpacityFactor * this.Shift.Y / 100);
                }
                else if (view.Tag == KSViewTag.UpImage && direction == KSDirection.Up)
                {
                    view.Layer.Opacity = -(Constants.KOverlayOpacityFactor*this.Shift.Y/100);
                }
                else if (view.Tag == KSViewTag.RightImage && direction == KSDirection.Right)
                {
                    view.Layer.Opacity = (Constants.KOverlayOpacityFactor * this.Shift.X / 100);
                }
                else if (view.Tag == KSViewTag.LeftImage && direction == KSDirection.Left)
                {
                    view.Layer.Opacity = -(Constants.KOverlayOpacityFactor*this.Shift.X/100);
                }

            }
        }

        public void _resetRotation(uint direction)
        {
            if (this.LastDirection != direction && this.LastDirection != DirectionCount)
            {
                UIView.Animate(0.2f, () =>
                    {
                        this.Transform = CGAffineTransform.MakeRotation(0);
                    });
            }

        }

        public void _changeViewOpacityForDirection(uint direction)
        {
            switch (direction)
            {
                case KSDirection.Up :
                    if (this.Shift.Y > 0) this.Shift.Y = 0;

                    this.Alpha = 1 + (Constants.KViewOpacityFactor * this.Shift.Y / 100);
                    break;
                case KSDirection.Down :
                    if (this.Shift.Y < 0) this.Shift.Y = 0;

                    this.Alpha = 1 - (Constants.KViewOpacityFactor * this.Shift.Y / 100);
                    break;
                case KSDirection.Left :
                    if (this.Shift.X > 0) this.Shift.X = 0;

                    if (!s_hasLeftOverlay) this.Alpha = 1 + (Constants.KViewRotationOpacityFactor * this.Shift.X / 100);
                    else this.Alpha = 1 + (Constants.KViewOpacityFactor * this.Shift.X / 100);

                    break;
                case KSDirection.Right :
                    if (this.Shift.X < 0) this.Shift.X = 0;

                    if (!s_hasLeftOverlay) this.Alpha = 1 - (Constants.KViewRotationOpacityFactor * this.Shift.X / 100);
                    else this.Alpha = 1 - (Constants.KViewOpacityFactor * this.Shift.X / 100);

                    break;
                default :
                    break;
            }

        }

    }
    }
}

