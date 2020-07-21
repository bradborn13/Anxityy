﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

namespace Anxityy.Fragments
{
    public class CreateAnxityRecord : Android.Support.V4.App.Fragment
    {
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
        const string getPlaceDetails = "https://maps.googleapis.com/maps/api/place/details/json?placeid=";
        AutoCompleteTextView txtSearch;
        string autoCompleteOptions;
        GoogleMapPlaceClass objMapClass;
        string[] strPredictiveText;
        ArrayAdapter adapter = null;
        int index = 0;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.createAnxityRecord, container, false);
            var formDate = view.FindViewById<EditText>(Resource.Id.formDate);
            formDate.Click += getDatePicker;
            var exitForm = view.FindViewById<TextView>(Resource.Id.exitForm);
            exitForm.Click += ExitForm_Click1;
            TextView pickerResult = view.FindViewById<TextView>(Resource.Id.seekbar_selectedVal);
            SeekBar seekbar = view.FindViewById<SeekBar>(Resource.Id.seekBarRating);
            seekbar.ProgressChanged += (System.Object sender, SeekBar.ProgressChangedEventArgs e) =>
             {
                 if (e.FromUser)
                 {

                     pickerResult.Text = "" + e.Progress;
                 }
             };
            txtSearch = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
            txtSearch.TextChanged += async delegate (object sender, Android.Text.TextChangedEventArgs e)
            {
                try
                {
                    autoCompleteOptions = await fnDownloadString(strAutoCompleteGoogleApi + txtSearch.Text + "&key=" + "AIzaSyCKO2n8Sbu1Ho7YHyeirQMILx0IAOMlxug");
                    if (autoCompleteOptions == "Exception")
                    {
                        Toast.MakeText(Activity, "Unable to connect to server!!!", ToastLength.Short).Show();
                        return;
                    }

                    objMapClass = JsonConvert.DeserializeObject<GoogleMapPlaceClass>(autoCompleteOptions);
                    strPredictiveText = new string[objMapClass.predictions.Count];
                    index = 0;
                    foreach (Prediction objPred in objMapClass.predictions)
                    {

                        strPredictiveText[index] = objPred.description;
                        index++;
                    }
                    adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveText);
                    txtSearch.Adapter = adapter;
                }
                catch
                {
                    Toast.MakeText(Activity, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }
            };

            return view;
        }
        async Task<string> fnDownloadString(string strUri)
        {
            WebClient webclient = new WebClient();
            string strResultData;
            try
            {
                strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
                Console.WriteLine(strResultData);
            }
            catch
            {
                strResultData = "Exception";
                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, "Unable to connect to server!!!", ToastLength.Short).Show();
                });
            }
            finally
            {
                webclient.Dispose();
                webclient = null;
            }

            return strResultData;
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
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
            trans.Replace(Resource.Id.contentFragment, new HomeFragment(), "Main_Page").SetTransition(2).Commit();
        }
        public async Task addAnxRecord(object sender, EventArgs e)
        {
            var formDate = Activity.FindViewById<EditText>(Resource.Id.formDate).Text;
            var formNote = Activity.FindViewById<EditText>(Resource.Id.formNote).Text;
            var pickerResult = Convert.ToInt32(Activity.FindViewById<TextView>(Resource.Id.seekbar_selectedVal).Text);

            var txtSearchTitle = Activity.FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch).Text;
            var selectedLocations = objMapClass.predictions.Find(xer => xer.description == txtSearchTitle);
            var placeDetails = await fnDownloadString(getPlaceDetails + selectedLocations.place_id + "&key=" + "AIzaSyCKO2n8Sbu1Ho7YHyeirQMILx0IAOMlxug");
            GoogleMapPlaceDetails objMapClasser = JsonConvert.DeserializeObject<GoogleMapPlaceDetails>(placeDetails);
            var geoDetails = objMapClasser.result.geometry.location;

            //TO:DO Add long/lat var
            var record = new AnxityRecords(formDate, pickerResult, formNote, "none", txtSearchTitle, geoDetails.lat, geoDetails.lng);
            var db = new AnxityDatabase();
            await db.SaveAnxityRecordAsync(record);
            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal");
            trans.Commit();
            Android.App.AlertDialog cat = new Android.App.AlertDialog.Builder(Activity).Create();
            cat.SetMessage("Record Added");
            cat.SetTitle("message title");
            cat.SetButton("OK", delegate { });
            cat.Show();
        }
        public void saveForm_Click12(object sender, EventArgs e)
        {

            //De ce nu merge sa salvez recordu in baza dedate si sa stau pe form in continuare?
            Android.App.AlertDialog cat = new Android.App.AlertDialog.Builder(Activity).Create();
            cat.SetMessage("Location");

            cat.SetTitle("message title");
            cat.SetButton("OK", delegate { });
            cat.Show();


        }
    }
}