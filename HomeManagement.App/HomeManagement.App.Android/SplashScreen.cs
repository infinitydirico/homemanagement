
using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace HomeManagement.App.Droid
{
    [Activity(Label = "Home Mgmt", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            // Create your application here
        }
    }
}