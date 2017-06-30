using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace PlayGround.UI.Droid.Views
{
    [Activity(Label = "DetailsActivity")]
    public class DetailsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);

            Forms.Init(this, null);
            var fragment = new DetailsPage().CreateFragment(this);
            var transaction = FragmentManager.BeginTransaction(); 
            transaction.Replace(Resource.Id.activity_details_layout, fragment, "details"); 
            transaction.Commit();
        }
    }
}
