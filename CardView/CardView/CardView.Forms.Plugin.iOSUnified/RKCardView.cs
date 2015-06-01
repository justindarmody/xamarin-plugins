using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;

namespace CardView.Forms.Plugin.iOSUnified
{
    public partial class Constants
    {
        public const float CORNER_RATIO = 0.015f;
        public const float CP_RATIO = 0.38f;
        public const float PP_RATIO = 0.247f;
        public const float PP_X_RATIO = 0.03f;
        public const float PP_Y_RATIO = 0.213f;
        public const int PP_BUFF = 3;
        public const float LABEL_Y_RATIO = 0.012f;
    }

    public class RKCardView : UIView
    {
        protected UIVisualEffectView visualEffectView;

        private UIView cp_mask;
        private UIView pp_mask;
        private UIView pp_circle;

        private UIImageView profileImageView;
        private UIImageView coverImageView;
        private UILabel titleLabel;

        private bool showProfileImage;

        public bool ShowProfileImage
        {
            get { return this.showProfileImage; }
            set { 
                this.showProfileImage = value;
                this.RemovePhotos();
                this.SetupPhotos();
            }
        }

        public RKCardView(CGRect frame)
            : base(frame)
        {
            this.SetupView();
        }

        public RKCardView()
        {
            this.SetupView();
        }

        void AddShadow()
        {
            this.Layer.ShadowOpacity = 0.15f;
        }

        void RemoveShadow()
        {
            this.Layer.ShadowOpacity = 0;
        }

        void SetupView()
        {
            this.BackgroundColor = UIColor.White;
            this.Layer.CornerRadius = this.Frame.Size.Width * Constants.CORNER_RATIO;
            this.Layer.ShadowRadius = 3;
            this.Layer.ShadowOpacity = 0;
            this.Layer.ShadowOffset = new CGSize(1, 1);
            this.SetupPhotos();
        }

        void SetupPhotos()
        {
            nfloat height = this.Frame.Size.Height;
            nfloat width = this.Frame.Size.Width;
            cp_mask = new UIView(new CGRect(0, 0, width, height * Constants.CP_RATIO));
            pp_mask = new UIView(new CGRect(width * Constants.PP_X_RATIO, height * Constants.PP_Y_RATIO, height * Constants.PP_RATIO, height * Constants.PP_RATIO));
            pp_circle = new UIView(new CGRect(pp_mask.Frame.Location.X - Constants.PP_BUFF, pp_mask.Frame.Location.Y - Constants.PP_BUFF, pp_mask.Frame.Size.Width + 2 * Constants.PP_BUFF, pp_mask.Frame.Size.Height + 2 * Constants.PP_BUFF));
            pp_circle.BackgroundColor = UIColor.White;
            pp_circle.Layer.CornerRadius = pp_circle.Frame.Size.Height / 2;
            pp_mask.Layer.CornerRadius = pp_mask.Frame.Size.Height / 2;
            cp_mask.BackgroundColor = new UIColor(0.98f, 0.98f, 0.98f, 1);

            nfloat cornerRadius = this.Layer.CornerRadius;
            UIBezierPath maskPath = UIBezierPath.FromRoundedRect(cp_mask.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new CGSize(cornerRadius, cornerRadius));
            CAShapeLayer maskLayer = new CAShapeLayer();
            maskLayer.Frame = cp_mask.Bounds;
            maskLayer.Path = maskPath.CGPath;
            cp_mask.Layer.Mask = maskLayer;

            UIBlurEffect blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
            visualEffectView = new UIVisualEffectView(blurEffect);
            visualEffectView.Frame = cp_mask.Frame;
            visualEffectView.Alpha = 0;

            profileImageView = new UIImageView();
            profileImageView.Frame = new CGRect(0, 0, pp_mask.Frame.Size.Width, pp_mask.Frame.Size.Height);
            coverImageView = new UIImageView();
            coverImageView.Frame = cp_mask.Frame;
            coverImageView.ContentMode = UIViewContentMode.ScaleAspectFill;

            cp_mask.AddSubview(coverImageView);
            pp_mask.AddSubview(profileImageView);
            cp_mask.ClipsToBounds = true;
            pp_mask.ClipsToBounds = true;

            // setup the label
            nfloat titleLabelX = pp_circle.Frame.Location.X + pp_circle.Frame.Size.Width;
            titleLabel = new UILabel(new CGRect(titleLabelX, cp_mask.Frame.Size.Height + 7, this.Frame.Size.Width - titleLabelX, 26));
            titleLabel.AdjustsFontSizeToFitWidth = false;
            titleLabel.LineBreakMode = UILineBreakMode.Clip;
            titleLabel.Font = UIFont.FromName("HelveticaNeue-Light", 20);
            titleLabel.TextColor = new UIColor(0, 0, 0, 0.8f);
            titleLabel.Text = "Title Label";
            titleLabel.UserInteractionEnabled = true;
            UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(this.TitleLabelTap);
            titleLabel.AddGestureRecognizer(tapGesture);
            coverImageView.UserInteractionEnabled = true;
            UITapGestureRecognizer tapGestureCover = new UITapGestureRecognizer(this.CoverPhotoTap);
            coverImageView.AddGestureRecognizer(tapGestureCover);
            profileImageView.UserInteractionEnabled = true;
            UITapGestureRecognizer tapGestureProfile = new UITapGestureRecognizer(this.ProfilePhotoTap);
            profileImageView.AddGestureRecognizer(tapGestureProfile);
            this.AddSubview(titleLabel);
            this.AddSubview(cp_mask);
            this.AddSubview(pp_circle);
            this.AddSubview(pp_mask);
            coverImageView.AddSubview(visualEffectView);
        }

        private void RemovePhotos()
        {
            this.titleLabel.RemoveFromSuperview();
            this.cp_mask.RemoveFromSuperview();
            this.pp_circle.RemoveFromSuperview();
            this.pp_mask.RemoveFromSuperview();

            this.titleLabel.Dispose();
            this.cp_mask.Dispose();
            this.pp_circle.Dispose();
            this.pp_mask.Dispose();
        }

        void TitleLabelTap()
        {
//                if (this.TheDelegate != null && this.TheDelegate.RespondsToSelector(@selector (nameTap)))
//                {
//                    this.TheDelegate.NameTap();
//                }
        }

        void CoverPhotoTap()
        {
//                if (this.TheDelegate != null && this.TheDelegate.RespondsToSelector(@selector (coverPhotoTap)))
//                {
//                    this.TheDelegate.CoverPhotoTap();
//                }

        }

        void ProfilePhotoTap()
        {
//                if (this.TheDelegate != null && this.TheDelegate.RespondsToSelector(@selector (profilePhotoTap)))
//                {
//                    this.TheDelegate.ProfilePhotoTap();
//                }

        }

        void AddBlur()
        {
            visualEffectView.Alpha = 1;
        }

        void RemoveBlur()
        {
            visualEffectView.Alpha = 0;
        }

        void HideProfileImage()
        {
            // TODO: 
        }
    }
}


