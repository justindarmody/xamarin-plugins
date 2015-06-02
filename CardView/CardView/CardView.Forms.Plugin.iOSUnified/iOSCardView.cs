using System;
using UIKit;
using CoreGraphics;

namespace CardView.Forms.Plugin.iOSUnified
{
    public class iOSCardView : UIView, IUIGestureRecognizerDelegate //, UIGestureRecognizerDelegate
    {
        public CGRect ZeroFrame { get; set; }

        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;

                this.DrawBorder(base.Frame, 0.0f);
            }
        }

        public iOSCardView() 
            : base()
        {
        }

        public iOSCardView(CGRect frame)
            : base(frame)
        {
            this.DrawBorder(frame, 0.0f);
        }

        protected void DrawBorder(CGRect rect, nfloat radius)
        {
            UIBezierPath shadowPath = UIBezierPath.FromRect(this.Bounds);
            this.ZeroFrame = this.Frame;
            this.Layer.MasksToBounds = false;
            this.Layer.ShadowColor = UIColor.Black.CGColor;
            this.Layer.ShadowOffset = new CGSize(0.0f, 1.0f);
            this.Layer.ShadowOpacity = 0.5f;
            this.Layer.ShadowPath = shadowPath.CGPath;
            this.Layer.CornerRadius = radius;
            this.Layer.ShadowRadius = radius;
//            this.BackgroundColor = UIColor.White;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}