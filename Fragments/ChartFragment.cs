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
using Java.Util;
using Microcharts;
using Microcharts.Droid;
using Newtonsoft.Json;
using SkiaSharp;
using Random = System.Random;

namespace Anxityy.Fragments
{
    public class ChartFragment : Android.Support.V4.App.Fragment
    {
        DayOfWeek[] days = {
        DayOfWeek.Sunday,
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday };
        List<ChartEntry> entries = new List<ChartEntry>();
        List<AnxityRecords> charData = new List<AnxityRecords>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           


            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ChartFragment, container, false);
            var fragArgs = Arguments;
            //if (!containsKey)
            //{
            //    Bundle stringBundle = Arguments.GetBundle("CharData");
            //    bundleString = stringBundle.GetString("CharData");
            //}
            if (fragArgs != null && fragArgs.ContainsKey("CharData"))
            {
                var charRecords = fragArgs.GetString("CharData");
                List<AnxityRecords> myObject = JsonConvert.DeserializeObject<List<AnxityRecords>>(charRecords);
                charData = myObject;
            }
            else
            {
                charData = new AnxityDatabase().GetCurrentWeekRecordsCount();
            }

            //ChartFragment
            var groupedData = charData.GroupBy(op => op.date);
            Dictionary<DayOfWeek, int> cct = new Dictionary<DayOfWeek, int>();
            foreach (var weekDay in days)
            {
                var datesByDaysOfWeek = groupedData.Where(op => DateTime.Parse(op.Key).DayOfWeek == weekDay).ToList();
                cct.Add(weekDay, datesByDaysOfWeek.Count);
            }
            foreach (KeyValuePair<DayOfWeek, int> entry in cct)
            {
                var chartEntry = new ChartEntry(entry.Value)
                {
                    Color = SKColor.Parse("#7A9E7E")
                };
                entries.Add(chartEntry);
            }
            var chart = new RadialGaugeChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                MaxValue = cct.Values.Max(),
                Margin = 100,
                LineSize = 15
            };
            var chartView = view.FindViewById<ChartView>(Resource.Id.theChart);
            chartView.Chart = chart;
            return view;
        }
    }
}