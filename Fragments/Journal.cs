using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Anxityy.Fragments
{
    public class Journal : Android.Support.V4.App.Fragment
    {


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //TO-DO: Order Records By the Dates -> Create View for the records
            View view = inflater.Inflate(Resource.Layout.JournalFragment, container, false);
            LinearLayout textHold = view.FindViewById<LinearLayout>(Resource.Id.journalRecordsWrapper);


            List<AnxityRecords> records = new AnxityDatabase().GetAnxityRecordsAsync().Result;
            var c = records.GroupBy(p => p.date);
            foreach (var anx in c)
            {
                if (anx.Count() > 0)
                {

                    LinearLayout externalWrapper = new LinearLayout(Context);
                    LinearLayout.LayoutParams generalLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    externalWrapper.LayoutParameters = generalLayoutParams;
                    externalWrapper.Orientation = Orientation.Vertical;


                    RelativeLayout layout_4 = new RelativeLayout(Context);
                    RelativeLayout.LayoutParams layout_4_params = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    layout_4.LayoutParameters = layout_4_params;

                    RelativeLayout Rlayout_left = new RelativeLayout(Context);
                    RelativeLayout.LayoutParams RlayoutRule = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    Rlayout_left.LayoutParameters = RlayoutRule;
                    RlayoutRule.AddRule(LayoutRules.AlignParentLeft);
                    TextView textView = new TextView(Activity);
                    textView.Text = "Date: " + anx.Key;
                    textView.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
                    textView.SetPadding(60, 50, 0, 50);
                    Rlayout_left.AddView(textView);

                    RelativeLayout Rlayout_right = new RelativeLayout(Context);
                    RelativeLayout.LayoutParams RlayoutRule_2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    RlayoutRule_2.AddRule(LayoutRules.AlignParentRight);
                    Rlayout_right.LayoutParameters = RlayoutRule_2;
                    TextView textView2 = new TextView(Activity);
                    textView2.Text = "Tracked: " + anx.Count();
                    textView2.SetTypeface(Typeface.Serif, TypefaceStyle.Bold);
                    textView2.SetPadding(0, 50, 70, 50);
                    Rlayout_right.AddView(textView2);

                    LinearLayout separatorLayout = new LinearLayout(Context);
                    separatorLayout.SetBackgroundColor(Color.LightGray);
                    LinearLayout.LayoutParams separatorParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 7);
                    separatorLayout.LayoutParameters = separatorParams;
                    separatorLayout.SetPadding(0, 30, 0, 30);

                    layout_4.AddView(Rlayout_right);
                    layout_4.AddView(Rlayout_left);
                    externalWrapper.AddView(layout_4);
                    externalWrapper.AddView(separatorLayout);


                    LinearLayout s1Layout = getRecordsLayout(anx);

                    externalWrapper.AddView(s1Layout);
                    textHold.AddView(externalWrapper);
                }


            }
            return view;
        }
        public LinearLayout getRecordsLayout(IGrouping<String, AnxityRecords> anx)
        {
            LinearLayout s1Layout = new LinearLayout(Context);
            s1Layout.Orientation = Orientation.Vertical;
            LinearLayout.LayoutParams s1LayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            s1Layout.LayoutParameters = s1LayoutParams;
            foreach (var x in anx)
            {

                RelativeLayout layout_4 = new RelativeLayout(Context);
                RelativeLayout.LayoutParams layout_4_params = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                layout_4.Click += delegate
                {
                    var fragment = new Single_RecordAnx();
                    Bundle args = new Bundle();
                    args.PutInt("recordId", x._id);
                    fragment.Arguments = args;
                    var trans = Activity.SupportFragmentManager.BeginTransaction();
                    trans.Replace(Resource.Id.contentFragment, fragment, "Single_record");
                    trans.Commit(); ;


                };
                layout_4.SetPadding(30, 30, 30, 30);
                layout_4.LayoutParameters = layout_4_params;
                TextView s1Text = new TextView(Activity);
                s1Text.Text = "Location: " + x._id;
                s1Text.TextSize = 10;
                layout_4.AddView(s1Text);
                s1Layout.AddView(layout_4);

            }
            return s1Layout;
        }
        //  Get Data form the database and return the updated View view
        // neccesary : "View" -  object whcih needs to be updated and sent back
        public View GetDataFromDb(View view)
        {
            //What to do here?
            return view;

        }

    }
}