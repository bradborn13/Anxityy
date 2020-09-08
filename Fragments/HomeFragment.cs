using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Hsalf.SmileRatingLib;
using Java.Util;
using Microcharts;
using Microcharts.Droid;
using Newtonsoft.Json;
using SkiaSharp;
using Random = System.Random;

namespace Anxityy.Fragments
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
           
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);
            LinearLayout charLayout = view.FindViewById<LinearLayout>(Resource.Id.charLayout);
            charLayout.Click += (s, e) =>
            {
                var trans = Activity.SupportFragmentManager.BeginTransaction();
                trans.Replace(Resource.Id.contentFragment, new WeeklyAnxityRecords(), "WeeklyAnxityRecords");
                trans.Commit();
            };
 
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.ChartFragment, new ChartFragment(), "ChartFragment");
            trans.Commit();
            return view;
        }
    }
}