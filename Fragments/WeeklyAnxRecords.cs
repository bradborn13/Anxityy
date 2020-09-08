using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
using Orientation = Android.Widget.Orientation;

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
            var charData = new AnxityDatabase().GetCurrentWeekRecords();
            var json = JsonConvert.SerializeObject(charData);
            Bundle mybundle = new Bundle();
            mybundle.PutString("CharData", json);
            ChartFragment charFragment = new ChartFragment { Arguments = mybundle };
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.ChartFragment2, charFragment, "ChartFragment");
            trans.Commit();
            LinearLayout textHold = view.FindViewById<LinearLayout>(Resource.Id.journalRecordsWrapper);

            var c = charData.OrderByDescending(p => Convert.ToDateTime(p.date)).GroupBy(p => p.date);
            foreach (var anx in c)
            {
                if (anx.Count() > 0)
                {
                    LinearLayout externalWrapper = new LinearLayout(Context);
                    externalWrapper.Orientation = Orientation.Vertical;
                    LinearLayout.LayoutParams generalLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    generalLayoutParams.SetMargins(20, 20, 20, 50);
                    externalWrapper.Background = Context.GetDrawable(Resource.Drawable.rectangleShadow);

                    externalWrapper.LayoutParameters = generalLayoutParams;
                    externalWrapper.Elevation = 6;
                    //  Header Layout +++
                    LinearLayout headerLayout = new LinearLayout(Context);
                    LinearLayout.LayoutParams headerLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    headerLayout.Orientation = Orientation.Horizontal;
                    headerLayout.LayoutParameters = headerLayoutParams;
                    headerLayout.SetPadding(20, 50, 20, 50);

                    RelativeLayout relativeHeaderLayout = new RelativeLayout(Context);
                    RelativeLayout.LayoutParams relativeHeaderLayoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    relativeHeaderLayout.LayoutParameters = relativeHeaderLayoutParams;
                    relativeHeaderLayout.Elevation = 10;
                    TextView countText = new TextView(Context);
                    countText.LayoutParameters = relativeHeaderLayoutParams;
                    countText.SetPadding(0, 0, 20, 0);
                    ; countText.Text = "Tracked : " + anx.Count();
                    countText.Gravity = GravityFlags.Right;


                    TextView dateText = new TextView(Context);
                    dateText.LayoutParameters = relativeHeaderLayoutParams;
                    dateText.Gravity = GravityFlags.Left;
                    dateText.SetPadding(40, 0, 0, 0);
                    dateText.Text = "" + customDateText(anx.Key);

                    relativeHeaderLayout.AddView(countText);
                    relativeHeaderLayout.AddView(dateText);
                    headerLayout.AddView(relativeHeaderLayout);
                    externalWrapper.AddView(headerLayout);
                    //  Header Layout ---


                    LinearLayout s1Layout = getRecordsLayout(anx);
                    s1Layout.LayoutParameters = headerLayoutParams;
                    externalWrapper.AddView(s1Layout);
                    textHold.AddView(externalWrapper);

                }
            }
            return view;

        }
        public string customDateText(string anxDate)
        {
            DateTime todayDate = DateTime.Now;
            DateTime yesterdayDate = DateTime.Today.AddDays(-1);
            DateTime parsedDate = DateTime.Parse(anxDate);
            if (todayDate.Date == parsedDate.Date)
            {
                return "Today";
            }
            if (yesterdayDate.Date == parsedDate.Date)
            {
                return "Yesterday";
            }
            return anxDate;

        }
        public LinearLayout getRecordsLayout(IGrouping<String, AnxityRecords> anx)
        {
            LinearLayout s1Layout = new LinearLayout(Context);
            s1Layout.Orientation = Orientation.Vertical;
            LinearLayout.LayoutParams s1LayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            s1Layout.LayoutParameters = s1LayoutParams;
            s1Layout.SetBackgroundColor(Color.ParseColor("#e5e5e5"));

            foreach (var x in anx)
            {
                LinearLayout rowLayout = new LinearLayout(Context);
                LinearLayout.LayoutParams rowLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                rowLayout.Orientation = Orientation.Horizontal;
                rowLayoutParams.SetMargins(30, 50, 30, 50);
                rowLayout.LayoutParameters = rowLayoutParams;
                rowLayout.Elevation = 10;
                rowLayout.Click += delegate
                {
                    var fragment = new Single_RecordAnx();
                    Bundle args = new Bundle();
                    args.PutInt("recordId", x._id);
                    fragment.Arguments = args;
                    var trans = Activity.SupportFragmentManager.BeginTransaction();
                    trans.Replace(Resource.Id.contentFragment, fragment, "Single_record")

                    .Commit();
                };

                RelativeLayout pingLayout = new RelativeLayout(Context);
                TableLayout.LayoutParams pingLayoutParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1f);

                pingLayoutParams.SetMargins(50, 0, 0, 0);
                pingLayout.LayoutParameters = pingLayoutParams;
                ImageView pingImg = new ImageView(Context);
                pingImg.SetImageResource(Resource.Drawable.pin);
                pingImg.LayoutParameters = new LinearLayout.LayoutParams(50, 50);
                pingLayout.AddView(pingImg);

                LinearLayout centerLayout = new LinearLayout(Context);
                TableLayout.LayoutParams centerLayoutParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1f);
                centerLayoutParams.Gravity = GravityFlags.CenterHorizontal;
                centerLayoutParams.SetMargins(-20, 0, 0, 0);
                centerLayout.Orientation = Orientation.Vertical;
                centerLayout.LayoutParameters = centerLayoutParams;


                TextView dateText = new TextView(Activity);
                dateText.Text = "" + x.date;
                dateText.TextSize = 10;

                TextView timeText = new TextView(Activity);
                timeText.Text = "" + DateTime.Now.ToString("HH:mm tt");
                timeText.TextSize = 10;
                timeText.SetPadding(0, 10, 0, 0);
                timeText.Gravity = GravityFlags.Center;
                centerLayout.AddView(dateText);
                centerLayout.AddView(timeText);




                RelativeLayout emojiLayout = new RelativeLayout(Context);
                TableLayout.LayoutParams emojiRLParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1f);
                emojiLayout.LayoutParameters = emojiRLParams;
                ImageView emojiImg = new ImageView(Context);
                emojiImg.SetImageResource(Resource.Drawable.emoji_sad);
                var emojiParam = new RelativeLayout.LayoutParams(80, 80);
                emojiParam.AddRule(LayoutRules.AlignParentRight);
                emojiParam.SetMargins(0, 0, 20, 0);
                emojiImg.LayoutParameters = emojiParam;
                emojiLayout.AddView(emojiImg);


                rowLayout.AddView(pingLayout);
                rowLayout.AddView(centerLayout);
                rowLayout.AddView(emojiLayout);
                s1Layout.AddView(rowLayout);
                var bv = anx.Last();
                if (!x.Equals(anx.Last()))
                {
                    RelativeLayout relImg = new RelativeLayout(Context);
                    RelativeLayout.LayoutParams relImgPara = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    relImg.LayoutParameters = relImgPara;
                    relImg.Elevation = 6;
                    //relImg.SetBackgroundColor(Color.Yellow);
                    TextView lineSeparator = new TextView(Context);
                    var lineSeparatorParams = new TableLayout.LayoutParams(2, 2, 1f);
                    lineSeparatorParams.SetMargins(60, 0, 60, 0);
                    lineSeparator.LayoutParameters = lineSeparatorParams;
                    lineSeparator.Background = Context.GetDrawable(Resource.Drawable.rectangleShadow);

                   
                    relImg.AddView(lineSeparator);
                    s1Layout.AddView(relImg);
                }


            }

            return s1Layout;

        }
  
    }
}