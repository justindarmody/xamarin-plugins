using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CardView.Forms.Plugin.Abstractions
{
    public class CardsView : StackLayout//Layout<CardContentView>
    {
        internal event EventHandler LayoutChildrenRequested;

        public new Color BackgroundColor 
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return base.OnSizeRequest(widthConstraint, heightConstraint);
        }

//        private readonly Dictionary<Size, SizeRequest> measureCache = new Dictionary<Size, SizeRequest>();
//        private CardsView.LayoutInformation layoutInformation = new CardsView.LayoutInformation();
//
//        #region implemented abstract members of Layout
//
//        protected override void LayoutChildren(double x, double y, double width, double height)
//        {
////            throw new NotImplementedException();
//        }
//
//        #endregion
//

//
//        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
//        {
//            var deviceInfo = DependencyService.Get<DeviceInfo>();
//
//            return new SizeRequest(new Size(Math.Min(Device.Info.ScaledScreenSize.Width, Device.Info.ScaledScreenSize.Height), Math.Max(Device.Info.ScaledScreenSize.Width, Device.Info.ScaledScreenSize.Height)), new Size(40.0, 40.0));
//        } 
//
//        protected override void InvalidateMeasure()
//        {
//            this.measureCache.Clear();
//            this.layoutInformation = new CardsView.LayoutInformation();
//            base.InvalidateMeasure();
//        }
//       
//        private class LayoutInformation
//        {
//            public Size Constraint;
//            public Rectangle[] Plots;
//            public SizeRequest[] Requests;
//            public Rectangle Bounds;
//            public Size MinimumSize;
//            public double CompressionSpace;
//            public int Expanders;
//        }
    }
}