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
using Microcharts;
using Microcharts.Droid;
using Newtonsoft.Json;
using SkiaSharp;

namespace Anxityy.Fragments
{
    public class WeeklyAnxityRecords : Android.Support.V4.App.Fragment
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

            View view = inflater.Inflate(Resource.Layout.weeklyRecords, container, false);
            var charData = new AnxityDatabase().GetCurrentWeekRecordsCount();
            var json = JsonConvert.SerializeObject(charData);
            Bundle mybundle = new Bundle();
            mybundle.PutString("CharData", json);
            ChartFragment charFragment = new ChartFragment { Arguments = mybundle };
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.ChartFragment2, charFragment, "ChartFragment");
            trans.Commit();
            return view;
        }
    }
}