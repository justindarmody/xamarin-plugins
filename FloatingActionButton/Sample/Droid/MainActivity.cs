using Android.App;
using Android.Widget;
using Android.OS;

namespace FABSample.Droid
{
    [Activity(Label = "FABSample.Droid", MainLauncher = true)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var app = new FABSample.App();

            this.LoadApplication(app);
        }
    }
}