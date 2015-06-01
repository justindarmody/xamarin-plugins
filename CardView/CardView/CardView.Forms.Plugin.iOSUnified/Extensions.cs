using System;
using UIKit;

namespace CardView.Forms.Plugin.iOSUnified
{
    internal static class Extensions
    {
        internal static void RemoveAllSubviews(this UIView super)
        {
            if (super == null)
            {
                return;
            }
            
            for (int i = 0; i < super.Subviews.Length; i++)
            {
                var subview = super.Subviews[i];

                subview.RemoveFromSuperview();
            }
        }
    }
}

