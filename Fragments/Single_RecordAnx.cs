using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

using Android.OS;
using Android.Renderscripts;
using Android.Views;
using Android.Widget;
using Hsalf.SmileRatingLib;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Platform.Android;
using Math = System.Math;

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
            SetupMap();
            ImageView imgView = Activity.FindViewById<ImageView>(Resource.Id.buttonCreate);
            var navLayout = Activity.SupportFragmentManager.FindFragmentByTag("MenuFragment");
            imgView.Visibility = ViewStates.Gone;
            var fm = Activity.SupportFragmentManager.BeginTransaction();

            fm.Hide(navLayout)
                     .Commit();
            ImageView backBtn = view.FindViewById<ImageView>(Resource.Id.backBtn);
            backBtn.Click += delegate
            {
                var trans = Activity.SupportFragmentManager.BeginTransaction();

                trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal")
                .SetTransition(1)
                .Commit();
            };
            View fragMap = view.FindViewById<View>(Resource.Id.map);
          
            TextView locationTextView = view.FindViewById<TextView>(Resource.Id.locationTextView);
            TextView dateTextView = view.FindViewById<TextView>(Resource.Id.dateTextView);
            //TextView seekbarTextView = view.FindViewById<TextView>(Resource.Id.seekbkar_val);
            //SeekBar seekbar = view.FindViewById<SeekBar>(Resource.Id.seekbar);
            TextView noteText = view.FindViewById<TextView>(Resource.Id.Anxnote);
            SmileRating smileRating = view.FindViewById<SmileRating>(Resource.Id.someFukingRating);
            getAnxRecord();
            dateTextView.Text = anx.date;
            noteText.Text = anx.note;

           
            if (string.IsNullOrEmpty(anx.locationLat) || string.IsNullOrEmpty(anx.locationLong))
            {
                //blur out the map
                //fragMap.Visibility = ViewStates.Invisible;
                //locationLayout.Visibility = ViewStates.Invisible;
            }
            else
            {
                locationTextView.Text = anx.locationName;

            };

            return view;
        }
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            ImageView imgView = Activity.FindViewById<ImageView>(Resource.Id.buttonCreate);
            imgView.Visibility = ViewStates.Visible;
            var navLayout = Activity.SupportFragmentManager.FindFragmentByTag("MenuFragment");
            var fm = Activity.SupportFragmentManager.BeginTransaction();
            fm.Show(navLayout)
                     .Commit();

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

        public async void OnMapReady(GoogleMap googleMap)
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
                mMap = googleMap;
            }



        }

    }
}