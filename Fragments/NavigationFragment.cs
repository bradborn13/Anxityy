using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Anxityy.Fragments
{
    public class NavigationFragment : Android.Support.V4.App.Fragment
    {
        TextView navHome;
        TextView navJournal;
        TextView navCalendar;
        int selectedItem = 1;
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
             navHome =view.FindViewById<TextView>(Resource.Id.homeNavBtn);
             navJournal = view.FindViewById<TextView>(Resource.Id.journalNavBtn);
             navCalendar = view.FindViewById<TextView>(Resource.Id.calendarNavBtn);
            navHome.Click += redirectHome;
            navJournal.Click += redirectToJournal;
            navCalendar.Click += redirectToCalendar;
            return view;

        }
        public void updateNavMenu(int updateElementNo)
        {
            this.selectedItem = updateElementNo;
            navHome.SetTextColor(Color.Black);
            navJournal.SetTextColor(Color.Black);
            navCalendar.SetTextColor(Color.Black);
            navCalendar.SetTextSize(ComplexUnitType.Px, 30);
            navJournal.SetTextSize(ComplexUnitType.Px, 30);
            navHome.SetTextSize(ComplexUnitType.Px, 30);


            if (selectedItem == 1)
            {
                navHome.SetTextColor(Color.Blue);
                navHome.SetTextSize(ComplexUnitType.Px, 40);

            }
            if (selectedItem == 2)
            {
                navJournal.SetTextColor(Color.Blue);
                navJournal.SetTextSize(ComplexUnitType.Px, 40);

            }
            if (selectedItem == 3)
            {
                 navCalendar.SetTextColor(Color.Blue);
                navCalendar.SetTextSize(ComplexUnitType.Px, 40);
            }
        }
        void redirectHome(object sender, EventArgs e)
        {
            updateNavMenu(1);
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new HomeFragment(), "MenuFragment");
            trans.Commit();
        }
        void redirectToJournal(object sender, EventArgs e)
        {
            updateNavMenu(2);
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal");
            trans.Commit();

        }
        void redirectToCalendar(object sender, EventArgs e)
        {
            updateNavMenu(3);
            //var trans = Activity.SupportFragmentManager.BeginTransaction();
            //trans.Replace(Resource.Id.fragmentContent, new Single_RecordAnx(), "MenuFragment");
            //trans.Commit(); 
          
   
          
          
        }
    }
}