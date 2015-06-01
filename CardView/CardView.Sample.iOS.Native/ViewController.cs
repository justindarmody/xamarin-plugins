using System;

using UIKit;

using CardView.Forms.Plugin.iOSUnified;
using CoreGraphics;

namespace CardView.Sample.iOS.Native
{
    public partial class ViewController : UIViewController
    {
        private const float BUFFERX = 20f;
        private const float BUFFERY = 40f;

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            this.View.BackgroundColor = new UIColor(0.95f,0.95f, 0.95f, 1);


            UILabel label1 = new UILabel(new CGRect(0, 0, 320, 44))
            {
                Text = "Label 1"
            };

            UILabel label2 = new UILabel(new CGRect(0, label1.Frame.Y + label1.Frame.Height, 320, 44))
                {
                    Text = "Label 2"
                };

            UILabel label3 = new UILabel(new CGRect(0, label2.Frame.Y + label2.Frame.Height, 320, 100))
                {
                    Text = "Maecenas sed diam eget risus varius blandit sit amet non magna. Donec id elit non mi porta gravida at eget metus.",
                    LineBreakMode = UILineBreakMode.WordWrap
                };

            try {
            var cardView = new XCardView();
            cardView.AddSubview(label1);
            cardView.AddSubview(label2);
            cardView.AddSubview(label3);

            this.View.AddSubview(cardView);
            }
            catch(Exception e)
            {
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

