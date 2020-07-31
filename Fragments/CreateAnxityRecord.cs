using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Widget;
using Newtonsoft.Json;
using static Android.Views.View;

namespace Anxityy.Fragments
{
    public class CreateAnxityRecord : Android.Support.V4.App.Fragment
    {
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
        const string getPlaceDetails = "https://maps.googleapis.com/maps/api/place/details/json?placeid=";
        const string strReverseGeolocationApi = "https://maps.googleapis.com/maps/api/geocode/json?latlng=";
        AutoCompleteTextView txtSearch;
        string autoCompleteOptions;
        GoogleMapPlaceClass objMapClass;
        string[] strPredictiveText;
        ArrayAdapter adapter = null;
        bool optionCurrentLocation = false;
        int index = 0;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            Iconify
                 .with(new JoanZapata.XamarinIconify.Fonts.FontAwesomeModule())
                 .with(new JoanZapata.XamarinIconify.Fonts.IonIconsModule());

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
            LinearLayout formBackground = view.FindViewById<LinearLayout>(Resource.Id.formBackground);
            formBackground.Touch += (s,e)=>
            {
                
                    var handled = false;
                    if (e.Event.Action == MotionEventActions.Down)
                    {
                    // do stuff
                    InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.NotAlways);
                    formBackground.RequestFocus();
                    handled = true;
                    }
                   

                    e.Handled = handled;
                };
            
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
                    var googleKey = Resources.GetString(Resource.String.google_maps_key);
                    autoCompleteOptions = await fnDownloadString(strAutoCompleteGoogleApi + txtSearch.Text + "&key=" + googleKey);
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
            IconTextView currentLocationButton = view.FindViewById<IconTextView>(Resource.Id.currentLocation);
            currentLocationButton.Click += async (sender, args) =>
            {

                optionCurrentLocation = true; 
                txtSearch.Text = "Current Location";

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
        public bool onSubmitFormValidate()
        {
            bool isFormValid = true;
           
            EditText formDate = Activity.FindViewById<EditText>(Resource.Id.formDate);
         
            EditText formNote = Activity.FindViewById<EditText>(Resource.Id.formNote);
            if (string.IsNullOrEmpty(formDate.Text))
            {
                isFormValid = false;
                formDate.RequestFocus();
                formDate.SetError("Date Required", null);
            }
            if (string.IsNullOrEmpty(formNote.Text))
            {
                isFormValid = false;
                formNote.RequestFocus();
                formNote.SetError("Note required", null);
            }
            return isFormValid;
        }
        public async Task addAnxRecord(object sender, EventArgs e)
        {
            bool isFormValid =  onSubmitFormValidate();
            if (!isFormValid)
            {
                return;
            }
            string formDate = Activity.FindViewById<EditText>(Resource.Id.formDate).Text;
            string formNote = Activity.FindViewById<EditText>(Resource.Id.formNote).Text;
          
            var pickerResult = Convert.ToInt32(Activity.FindViewById<TextView>(Resource.Id.seekbar_selectedVal).Text);
            var txtSearchTitle = Activity.FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch).Text;

            if (string.IsNullOrEmpty(txtSearchTitle))
            {
                var record = new AnxityRecords(formDate, pickerResult, formNote, "none");
                var db = new AnxityDatabase();
                await db.SaveAnxityRecordAsync(record);
            }
            else
            {
                //get location data based on the current location data
                if ( optionCurrentLocation )
                {
                    var googleKey = Resources.GetString(Resource.String.google_maps_key);
                    var location = await new LocationTracker().getLaskKnownLocation();

                    var geolocationData = await fnDownloadString(strReverseGeolocationApi + location.Latitude + "," + location.Longitude + "&key=" + googleKey);
                    if (geolocationData == "Exception")
                    {
                        Toast.MakeText(Activity, "Could not get current location.Try again", ToastLength.Short).Show();
                        return;
                    }
                    GeolocationDetails currentLocationData = JsonConvert.DeserializeObject<GoogleMapGeolocaitonApi>(geolocationData).results[0];
                    var record = new AnxityRecords(formDate, pickerResult, formNote, "none", currentLocationData.formatted_address
                        , currentLocationData.geometry.location.lat, currentLocationData.geometry.location.lng);
                    var db = new AnxityDatabase();
                    await db.SaveAnxityRecordAsync(record);
                }
                else
                {
                    // get location data based on location search
                    var googleKey = Resources.GetString(Resource.String.google_maps_key);
                    var selectedLocations = objMapClass.predictions.Find(xer => xer.description == txtSearchTitle);
                    var placeDetails = await fnDownloadString(getPlaceDetails + selectedLocations.place_id + "&key=" + googleKey);
                    GoogleMapPlaceDetails objMapClasser = JsonConvert.DeserializeObject<GoogleMapPlaceDetails>(placeDetails);
                    var geoDetails = objMapClasser.result.geometry.location;
                    var record = new AnxityRecords(formDate, pickerResult, formNote, "none", txtSearchTitle, geoDetails.lat, geoDetails.lng);
                    var db = new AnxityDatabase();
                    await db.SaveAnxityRecordAsync(record);
                }
            
            }
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