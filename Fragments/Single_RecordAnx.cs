using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Platform.Android;

namespace Anxityy.Fragments
{
    public class Single_RecordAnx : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private AnxityRecords anx;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.singleRecordAnx, container, false);
            LinearLayout anxLayout = view.FindViewById<LinearLayout>(Resource.Id.anxLayout);
            SetupMap();



            getAnxRecord();
            foreach (var val in anx.GetType().GetProperties())
            {
                if (val.Name == "_id")
                {
                    continue;
                }
                var formLayout = createAnxLayout(val, anx);
                anxLayout.AddView(formLayout);
            }

            return view;
        }

        private void SetupMap()
        {
            if (mMap == null)
            {
                //SupportMapFragment mapFragment = (SupportMapFragment)Activity.GetFragmentManager()
                //          .FindFragmentById(Resource.Id.map);
                SupportMapFragment mapFragment = ChildFragmentManager.FindFragmentById(Resource.Id.map) as SupportMapFragment;
                mapFragment.GetMapAsync(this);
            }

        }
        public RelativeLayout createAnxLayout(PropertyInfo val, AnxityRecords anx)
        {
            RelativeLayout layout_lin1 = new RelativeLayout(Activity);

            RelativeLayout.LayoutParams lin1_params = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            RelativeLayout.LayoutParams lin1_params_left = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            RelativeLayout.LayoutParams lin1_params_right = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);

            RelativeLayout layout_lin1_left = new RelativeLayout(Context);
            RelativeLayout layout_lin1_right = new RelativeLayout(Context);

            layout_lin1.LayoutParameters = lin1_params;
            layout_lin1_left.LayoutParameters = lin1_params_left;
            layout_lin1_right.LayoutParameters = lin1_params_right;
            lin1_params_right.AddRule(LayoutRules.AlignParentRight);
            lin1_params_left.AddRule(LayoutRules.AlignParentLeft);

            TextView textView = new TextView(Activity);
            textView.Text = "" + FirstCharToUpper(val.Name);
            textView.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
            textView.SetPadding(60, 50, 0, 50);
            layout_lin1_left.AddView(textView);
            TextView textView2 = new TextView(Activity);
            textView2.Text = "" + val.GetValue(anx);
            textView2.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
            textView2.SetPadding(0, 50, 50, 50);
            layout_lin1_right.AddView(textView2);
            layout_lin1.AddView(layout_lin1_left);
            layout_lin1.AddView(layout_lin1_right);


            return layout_lin1;
        }
        public void getAnxRecord()
        {
            int anxId = Arguments.GetInt("recordId");

            anx = new AnxityDatabase().GetAnxityRecordAsync(anxId).Result;
        }
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public  void OnMapReady(GoogleMap googleMap)
        {
           
            if(anx.locationLat != null && anx.locationLong != null)
            {
                mMap = googleMap;
                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.CompassEnabled = true;
                mMap.UiSettings.ZoomControlsEnabled = true;
                LatLng marker = new LatLng(Convert.ToDouble( anx.locationLat), Convert.ToDouble(anx.locationLong));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(marker, 15);
                mMap.MoveCamera(camera);
                var markerOptions = new MarkerOptions().SetPosition(marker).SetTitle("Target 0 ");
                mMap.AddMarker(markerOptions);
            }
            else
            {
                //Dont initialize map

                //var location = await new LocationTracker().getLaskKnownLocation();
                ////var location = new LatLng(Convert.ToDouble(13.0291), Convert.ToDouble(80.2083));
                //LatLng marker = new LatLng(location.Latitude, location.Longitude);
                //CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(marker, 15);
                //mMap.MoveCamera(camera);
                //var markerOptions = new MarkerOptions().SetPosition(marker).SetTitle("Target 0 ");
                //mMap.AddMarker(markerOptions);
            }
           


        }

    }
}