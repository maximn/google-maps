using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;

namespace XamarinCanConsumeNetStandard
{
    [Activity(Label = "Places near Seattle", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool useAsync = true;  //Note: Non asyc is obsolete
        private string apiKey = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            var request = new PlacesNearByRequest
            {
                ApiKey = apiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            if (!useAsync)
            {
                //Note: sycronous webservice calls can't be done on the UI thread in Xamarin or native android. It will cause it to crash with no info.
                //Note: this is an obsolete way of doing things, its much better to use async
                var task = Task.Factory.StartNew(() => GoogleMaps.PlacesNearBy.Query(request));
                var listview = FindViewById<ListView>(Resource.Id.list_view);
                listview.Adapter = new HomeScreenAdapter(this, task.Result.Results.Select(r => r.Name).ToArray());
            }
            else
            {
                //NOTE: async operations can be done on UI thread as Xamarin will handle it for us
                PlacesNearByResponse result = await GoogleMaps.PlacesNearBy.QueryAsync(request);
                var listview = FindViewById<ListView>(Resource.Id.list_view);
                listview.Adapter = new HomeScreenAdapter(this, result.Results.Select(r => r.Name).ToArray());
            }
        }
    }

    public class HomeScreenAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public HomeScreenAdapter(Activity context, string[] items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            return view;
        }
    }
}

