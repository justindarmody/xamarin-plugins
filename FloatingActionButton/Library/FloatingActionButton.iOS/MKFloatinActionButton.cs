using System;
using UIKit;
using CoreGraphics;
using Foundation;
using CoreAnimation;

namespace FAB.iOS
{
    public class MNFloatingActionButton : UIControl
    {
        public enum ShadowState
        {
            ShadowStateShown,
            ShadowStateHidden
        }

        public enum FABSize {
            Mini,
            Normal
        }

        private readonly nfloat animationDuration;
        private readonly nfloat animationScale;
        private readonly nfloat shadowOpacity;
        private readonly nfloat shadowRadius;

        private FABSize size = FABSize.Normal;

        public FABSize Size {
            get { return size; }
            set {
                if (size == value)
                    return;

                size = value;
                UpdateBackground ();
            }
        }


        UIImageView _centerImageView;
        public UIImageView CenterImageView
        {
            get
            {
                if (_centerImageView == null)
                {
                    _centerImageView = new UIImageView(new UIImage("plus"));
                }

                return _centerImageView;
            }
            set
            {
                _centerImageView = value;
            }
        }

        UIColor _backgroundColor;
        public UIColor BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                this.BackgroundCircle.BackgroundColor = _backgroundColor;
            }
        }

        public UIColor PressedBackgroundColor { get; set; }

        public UIColor ShadowColor {get; set;}

        public nfloat ShadowOpacity {get; set;}

        public nfloat ShadowRadius {get; set;}

        public nfloat AnimationScale {get; set;}

        public nfloat AnimationDuration {get; set;}

        public bool IsAnimating {get; set;}

        UIView _backgroundCircle;
        public UIView BackgroundCircle
        {
            get
            {
                if (_backgroundCircle == null)
                {
                    _backgroundCircle = new UIView(this.Bounds);
                }

                return _backgroundCircle;
            }
            set
            {
                _backgroundCircle = value;
            }
        }

        public MNFloatingActionButton() : base() 
        {
            this.animationDuration = 0.05f;
            this.animationScale = 0.85f;
            this.shadowOpacity = 0.6f;
            this.shadowRadius = 1.5f;
            
            this.CommonInit();
        }

        public MNFloatingActionButton(CGRect frame) : base(frame)
        {
            this.animationDuration = 0.05f;
            this.animationScale = 0.85f;
            this.shadowOpacity = 0.6f;
            this.shadowRadius = 1.5f;
            
            this.CommonInit();
        }

        void CommonInit()
        {
            this.BackgroundColor = UIColor.Red.ColorWithAlpha(0.4f);
            this.BackgroundColor = new UIColor(33.0f / 255.0f, 150.0f / 255.0f, 243.0f / 255.0f, 1.0f);
            this.BackgroundCircle.BackgroundColor = this.BackgroundColor;
            this.ShadowOpacity = shadowOpacity;
            this.ShadowRadius = shadowRadius;
            this.AnimationScale = animationScale;
            this.AnimationDuration = animationDuration;
            this.BackgroundCircle.AddSubview(this.CenterImageView);
            this.AddSubview(this.BackgroundCircle);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.CenterImageView.Center = this.BackgroundCircle.Center;
            if (!this.IsAnimating)
            {
                var viewSize = this.Size == FABSize.Normal ? 56 : 40;

                this.BackgroundCircle.Frame = new CGRect(this.Bounds.X, this.Bounds.Y, viewSize, viewSize);
                this.BackgroundCircle.Layer.CornerRadius = this.Bounds.Size.Height / 2;
                this.BackgroundCircle.Layer.ShadowColor = this.ShadowColor != null ? this.ShadowColor.CGColor : this.BackgroundColor.CGColor; 
                this.BackgroundCircle.Layer.ShadowOpacity = (float)this.ShadowOpacity;
                this.BackgroundCircle.Layer.ShadowRadius = this.ShadowRadius;
                this.BackgroundCircle.Layer.ShadowOffset = new CGSize(1.0, 1.0);

                this.CenterImageView.Frame = new CGRect(0, 0, 24, 24);
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.AnimateToSelectedState();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            this.AnimateToDeselectedState();
            this.SendActionForControlEvents(UIControlEvent.TouchUpInside);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            this.AnimateToDeselectedState();
            this.SendActionForControlEvents(UIControlEvent.TouchCancel);
        }

        public void AnimateToSelectedState()
        {
            this.IsAnimating = true;
            this.ToggleShadowAnimationToState(ShadowState.ShadowStateHidden);
            UIView.Animate(animationDuration, () =>
                {
                    this.BackgroundCircle.Transform = CGAffineTransform.MakeScale(this.AnimationScale, this.AnimationScale);
                    this.BackgroundCircle.BackgroundColor = this.PressedBackgroundColor;
                }, () =>
                {
                    this.IsAnimating = false;
                });
        }

        public void AnimateToDeselectedState()
        {
            this.IsAnimating = true;
            this.ToggleShadowAnimationToState(ShadowState.ShadowStateShown);
            UIView.Animate(animationDuration, () =>
                {
                    this.BackgroundCircle.Transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
                    this.BackgroundCircle.BackgroundColor = this.BackgroundColor;
                }, () =>
                {
                    this.IsAnimating = false;
                });
        }

        public void ToggleShadowAnimationToState(ShadowState state)
        {
            nfloat endOpacity = 0.0f;
            if (state == ShadowState.ShadowStateShown)
            {
                endOpacity = this.ShadowOpacity;
            }

            CABasicAnimation animation = CABasicAnimation.FromKeyPath("shadowOpacity");
            animation.From = NSNumber.FromFloat((float)this.ShadowOpacity);
            animation.To = NSNumber.FromFloat((float)endOpacity);
            animation.Duration = animationDuration;
            this.BackgroundCircle.Layer.AddAnimation(animation, "shadowOpacity");
            this.BackgroundCircle.Layer.ShadowOpacity = (float)endOpacity;
        }
    
        private void UpdateBackground()
        {
        }
            
    }
}