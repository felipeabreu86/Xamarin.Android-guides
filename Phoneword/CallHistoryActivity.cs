using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Phoneword
{
    [Activity(Label = "CallHistoryActivity")]
    public class CallHistoryActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var phonesNumbers = Intent.Extras.GetStringArrayList("phone_numbers") ?? new string[0];
            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, phonesNumbers);
        }
    }
}