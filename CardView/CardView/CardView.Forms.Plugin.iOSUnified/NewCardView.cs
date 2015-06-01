using System;
using UIKit;

namespace CardView.Forms.Plugin.iOSUnified
{
    public class NewCardCell : UIView
    {
        public UILabel TitleLabel {get; set;}

        public UILabel NameLabel {get; set;}

        public UILabel DescriptionLabel {get; set;}

        public UIButton CommentButton {get; set;}

        public UIButton LikeButton {get; set;}

        public UIView CardView {get; set;}

        public UIImageView ProfileImage {get; set;}

        void LayoutSubviews()
        {
            this.CardSetup();
            this.ImageSetup();
        }

        void CardSetup()
        {
            this.CardView.Alpha =1;
            this.CardView.Layer.MasksToBounds = false;
            this.CardView.Layer.CornerRadius = 1;
            this.CardView.Layer.ShadowOffset = new CoreGraphics.CGSize(- .2f, .2f);
            this.CardView.Layer.ShadowRadius = 1;
            this.CardView.Layer.ShadowOpacity = 0.2;
            UIBezierPath path = new UIBezierPath(this.CardView.Bounds);
            this.CardView.Layer.ShadowPath = path.CGPath;
            this.BackgroundColor = new UIColor( .9, .9, .9, 1);
        }

        void ImageSetup()
        {
            this.ProfileImage.Layer.CornerRadius = this.ProfileImage.Frame.Size.Width / 2;
            this.ProfileImage.ClipsToBounds = true;
            this.ProfileImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.ProfileImage.BackgroundColor = UIColor.White;
        }
    }
}