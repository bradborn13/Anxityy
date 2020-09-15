using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
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
            LinearLayout locationLayout = view.FindViewById<LinearLayout>(Resource.Id.locationLayout);

            View fragMap = view.FindViewById<View>(Resource.Id.map);
            TextView locationTextView = view.FindViewById<TextView>(Resource.Id.locationTextView);
            TextView dateTextView = view.FindViewById<TextView>(Resource.Id.dateTextView);
            TextView seekbarTextView = view.FindViewById<TextView>(Resource.Id.seekbar_val);
            SeekBar seekbar = view.FindViewById<SeekBar>(Resource.Id.seekbar);
            getAnxRecord();
            dateTextView.Text = anx.date;
            seekbarTextView.Text = Convert.ToString(anx.rating);
            seekbar.Progress = anx.rating;
            seekbar.Touch += (s, e) =>
            {
            };
            seekbar.Enabled = false;
            if (string.IsNullOrEmpty(anx.locationLat) || string.IsNullOrEmpty(anx.locationLong))
            {
                fragMap.Visibility = ViewStates.Invisible;
                locationLayout.Visibility = ViewStates.Invisible;
            } else {
                locationTextView.Text = anx.locationName;

            };
        
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

        public void getAnxRecord()
        {
            int anxId = Arguments.GetInt("recordId");

            anx = new AnxityDatabase().GetAnxityRecordAsync(anxId).Result;
        }

        public void OnMapReady(GoogleMap googleMap)
        {

            if (anx.locationLat != null && anx.locationLong != null)
            {
                mMap = googleMap;
                mMap.UiSettings.ZoomControlsEnabled = true;
                mMap.UiSettings.CompassEnabled = true;
                mMap.UiSettings.ZoomControlsEnabled = true;
                LatLng marker = new LatLng(Convert.ToDouble(anx.locationLat), Convert.ToDouble(anx.locationLong));
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