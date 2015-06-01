using System;
using UIKit;

namespace CardView.Forms.Plugin.iOSUnified
{
    public class XCardView : UIView {

        private float raidus =  2;

        public float Radius
        {
            get
            { 
                return this.raidus; 
            }
            set
            {
                this.raidus = value;

                this.RefreshView();
            }
        }

        public XCardView() 
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.RefreshView();
        }

        private void RefreshView()
        {
            this.Layer.CornerRadius = this.raidus;

            var shadowPath = UIBezierPath.FromRoundedRect(this.Bounds, this.raidus);

            this.Layer.MasksToBounds = false;
            this.Layer.ShadowColor = UIColor.Black.CGColor;
            this.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 3);
            this.Layer.ShadowOpacity = 0.5f;
            this.Layer.ShadowPath = shadowPath.CGPath;
        }
    }
}