//using System;
//using CardView.Forms.Plugin.Abstractions;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using Android.Util;
//
//[assembly: Dependency(typeof(CardView.Forms.Plugin.Droid.AndroidDeviceInfo))]
//
//namespace CardView.Forms.Plugin.Droid
//{
//    private class AndroidDeviceInfo : DeviceInfo
//    {
//        private FormsApplicationActivity formsActivity;
//        private readonly Size pixelScreenSize;
//        private readonly Size scaledScreenSize;
//        private readonly double scalingFactor;
//
//        public override Size PixelScreenSize
//        {
//            get
//            {
//                return this.pixelScreenSize;
//            }
//        }
//
//        public override Size ScaledScreenSize
//        {
//            get
//            {
//                return this.scaledScreenSize;
//            }
//        }
//
//        public override double ScalingFactor
//        {
//            get
//            {
//                return this.scalingFactor;
//            }
//        }
//
//        public AndroidDeviceInfo(FormsApplicationActivity formsActivity)
//        {
//            this.formsActivity = formsActivity;
//
//            using (DisplayMetrics displayMetrics = formsActivity.Resources.DisplayMetrics)
//            {
//                this.scalingFactor = (double) displayMetrics.Density;
//                this.pixelScreenSize = new Size((double) displayMetrics.WidthPixels, (double) displayMetrics.HeightPixels);
//                this.scaledScreenSize = new Size(this.pixelScreenSize.Width / this.scalingFactor, this.pixelScreenSize.Width / this.scalingFactor);
//            }
//        }
//
//        private void CheckOrientationChanged(Orientation orientation)
//        {
//
//            this.previousOrientation = orientation;
//        }
//
//        private void ConfigurationChanged(object sender, EventArgs e)
//        {
//            this.CheckOrientationChanged(this.formsActivity.Resources.Configuration.Orientation);
//        }
//
//        protected override void Dispose(bool disposing)
//        {
//            this.formsActivity.ConfigurationChanged -= new EventHandler(this.ConfigurationChanged);
//            base.Dispose(disposing);
//        }
//    }
//}
//
