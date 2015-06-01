using UIKit;
using CoreGraphics;

namespace CardView.Forms.Plugin.iOSUnified
{
    public partial class Constants
    {
        public const int KHorizontalEdgeOffset = 65;
        public const int KVerticalEdgeOffset = 65;
        public const int KRubberBandFirstPass = 25;
        public const int KRubberBandSecondPass = 10;
        public const float KRubberBandDuration = 0.75f;
        public const float KCardLeavesDuration = 0.5f;
        public const float KRotationFactor = 0.25f;
        public const float KOverlayOpacityFactor = 0.5f;
        public const float KViewOpacityFactor = 0.15f;
        public const float KViewRotationOpacityFactor = 0.5f;
        public const int KStartRotation = 60;
    }

    public enum KSDirection {
        Up,
        Down,
        Left,
        Right
    }

    public enum KSViewTag {
        UpImage,
        DownImage,
        LeftImage,
        RightImage,
    }
    /*
     typedef NS_ENUM(NSUInteger, Direction)
{
    DirectionUp,
    DirectionDown,
    DirectionLeft,
    DirectionRight,
    
    DirectionCount
};

typedef NS_ENUM(NSUInteger, ViewTags)
{
    ViewTagUpImage = 1,
    ViewTagDownImage,
    ViewTagLeftImage,
    ViewTagRightImage,
};

*/

    public class KSCardView : UIView
    {
        public KSCardViewDelegate TheDelegate {get; set;}

        public bool AllowLeft {get; set;}

        public bool AllowRight {get; set;}

        public bool AllowUp {get; set;}

        public bool AllowDown {get; set;}

        static void SetCardViewFrame(CGRect frame);

        static void SetOverlayLeftRightUpDown(UIView leftOverlay, UIView rightOverlay, UIView upOverlay, UIView downOverlay);

        void ShowFromLeft();

        void ShowFromRight();

        void ShowFromTop();

        void ShowFromBottom();

        void DemoUp();

        void DemoDown();

        void DemoLeft();

        void DemoRight();

        void DemoReset();

        void LeaveLeft();

        void LeaveRight();

        void LeaveTop();

        void LeaveBottom();
    }

    public interface KSCardViewDelegate
    {
        void CardDidLeaveTopEdge(KSCardView cardView);

        void CardDidLeaveBottomEdge(KSCardView cardView);

        void CardDidLeaveLeftEdge(KSCardView cardView);

        void CardDidLeaveRightEdge(KSCardView cardView);
    }
}
