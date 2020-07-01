﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Runtime;

using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Java.Util.Zip;

namespace Anxityy.Fragments
{
    public class CreateAnxityRecord : Android.Support.V4.App.Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here

        }

        public   override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.createAnxityRecord, container, false);
            var formDate = view.FindViewById<EditText>(Resource.Id.formDate);
            formDate.Click += getDatePicker;
            var exitForm = view.FindViewById<TextView>(Resource.Id.exitForm);
            exitForm.Click += ExitForm_Click1;
            TextView pickerResult = view.FindViewById<TextView>(Resource.Id.seekbar_Resulttext);
            SeekBar seekbar = view.FindViewById<SeekBar>(Resource.Id.seekBarRating);
            seekbar.ProgressChanged += (System.Object sender, SeekBar.ProgressChangedEventArgs e) =>
             {
                 if (e.FromUser)
                 {

                     pickerResult.Text = "" + e.Progress;
                 }
             };
            return view;
        }
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var saveForm = Activity.FindViewById<TextView>(Resource.Id.cevaVar);
            saveForm.Click += async (sender, e) =>
            {
                await addAnxRecord(sender, e);
            };


        }
     
        void getDatePicker(object sender, EventArgs e)
        {
        
            DateTime today = DateTime.Today;
            DatePickerDialog dialog = new DatePickerDialog(Context, OnDateSet, today.Year, today.Month - 1, today.Day);
            dialog.DatePicker.MinDate = today.Millisecond;
            dialog.Show();
        }
        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            var formDate = Activity.FindViewById<EditText>(Resource.Id.formDate);
            formDate.Text = e.Date.ToLongDateString();
        }
        public void ExitForm_Click1(object sender, EventArgs e)
        {

            var trans = Activity.SupportFragmentManager.BeginTransaction();
            //trans.SetCustomAnimations(Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_top);
            trans.Replace(Resource.Id.contentFragment, new HomeFragment(), "Main_Page");
            trans.Commit(); 
        }
        public async Task addAnxRecord(object sender, EventArgs e)
        {
            var formLocation = Activity.FindViewById<EditText>(Resource.Id.formLocation).Text;
            var formDate = Activity.FindViewById<EditText>(Resource.Id.formDate).Text;
            var formNote = Activity.FindViewById<EditText>(Resource.Id.formNote).Text;

            var record = new AnxityRecords(formLocation, formDate, 3, formNote, "art");
            var db = new AnxityDatabase();
          await db.SaveAnxityRecordAsync(record);
            Android.App.AlertDialog cat = new Android.App.AlertDialog.Builder(Activity).Create();
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal");
            cat.SetMessage("Location : " + formLocation + " || Date :" + formDate + " || Note : " + formNote);

       
            trans.Commit();
            cat.SetTitle("message title");
            cat.SetButton("OK", delegate { });
            cat.Show();
        }
        public void saveForm_Click12(object sender, EventArgs e )
        {

            //De ce nu merge sa salvez recordu in baza dedate si sa stau pe form in continuare?
            Android.App.AlertDialog cat = new Android.App.AlertDialog.Builder(Activity).Create();
            //cat.SetMessage("Location : " + formLocation + " || Date :" + formDate + " || Note : " + formNote);
            cat.SetMessage("Location" );

            cat.SetTitle("message title");
            cat.SetButton("OK", delegate { });
            cat.Show();
           
         
        }
    }
}