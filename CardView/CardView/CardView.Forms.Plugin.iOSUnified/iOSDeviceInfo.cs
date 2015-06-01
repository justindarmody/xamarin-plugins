using System;
using CardView.Forms.Plugin.Abstractions;
using Foundation;
using Xamarin.Forms;
using UIKit;

[assembly: Dependency(typeof(CardView.Forms.Plugin.iOSUnified.IOSDeviceInfo))]

namespace CardView.Forms.Plugin.iOSUnified
{
    internal class IOSDeviceInfo : DeviceInfo
    {
        private NSObject notification;
        private readonly Size pixelScreenSize;
        private readonly Size scaledScreenSize;
        private readonly double scalingFactor;

        public override Size PixelScreenSize
        {
            get
            {
                return this.pixelScreenSize;
            }
        }

        public override Size ScaledScreenSize
        {
            get
            {
                return this.scaledScreenSize;
            }
        }

        public override double ScalingFactor
        {
            get
            {
                return this.scalingFactor;
            }
        }

        public IOSDeviceInfo()
        {
//            this.notification = UIDevice.Notifications.ObserveOrientationDidChange((EventHandler<NSNotificationEventArgs>) ((sender, args) => this.SetValueCore(DeviceInfo.CurrentOrientationPropertyKey, (object) Xamarin.Forms.Platform.iOS.Extensions.ToDeviceOrientation(UIDevice.CurrentDevice.Orientation), BindableObject.SetValueFlags.None)));
            this.scalingFactor = (double) UIScreen.MainScreen.Scale;
            this.scaledScreenSize = new Size((double) UIScreen.MainScreen.Bounds.Width, (double) UIScreen.MainScreen.Bounds.Height);
            this.pixelScreenSize = new Size(this.scaledScreenSize.Width * this.scalingFactor, this.scaledScreenSize.Height * this.scalingFactor);
        }

        protected override void Dispose(bool disposing)
        {
//            this.notification.Dispose();
            base.Dispose(disposing);
        }
    }
}

