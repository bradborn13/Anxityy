using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;

namespace Anxityy.Fragments
{
    public class NavigationFragment : Android.Support.V4.App.Fragment
    {
        private BottomNavigationView bottomBar;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.navigationFragment, container, false);
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            var bottomBar = view.FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationView);
            bottomBar.NavigationItemSelected += async (s, a) =>
            {

                switch (a.Item.ItemId)
                {
                    case Resource.Id.homeTab:
                        var trans = Activity.SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.contentFragment, new HomeFragment(), "MenuFragment");
                        trans.Commit();
                        break;
                    case Resource.Id.journalTab:
                         trans = Activity.SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal");
                        trans.Commit();
                        break;
                    case Resource.Id.calendarTab:
                        var location = await new LocationTracker().getCurrentLocation();
                        Android.App.AlertDialog cat = new Android.App.AlertDialog.Builder(Activity).Create();
                        cat.SetMessage($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                        cat.SetTitle("message title");
                        cat.SetButton("OK", delegate { });
                        cat.Show();
                        break;
                }
            };

          

            return view;

        }

    }
}

     

