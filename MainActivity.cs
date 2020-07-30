using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Anxityy.Fragments;
using Android.Support.V4.App;
using Xamarin.Essentials;
using SQLite;
using System.IO;
using Environment = System.Environment;
using System.Linq;
using Android.Gms.Maps;
using Android.Content;
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;
using Android.Support.Design.Widget;

namespace Anxityy
{
    [Activity(Label = "Anxity", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            Window.RequestFeature(Android.Views.WindowFeatures.NoTitle);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        
            var trans = SupportFragmentManager.BeginTransaction();
            var intent = new Intent(this, typeof(SampleService));
                StartService(intent);
         
            trans.Replace(Resource.Id.contentFragment, new HomeFragment(), "Main_Page");
            trans.Replace(Resource.Id.menuFragment, new NavigationFragment(), "MenuFragment");
            trans.Commit();
            CreateNotificationChannel();
            var createRecordForm = FindViewById<TextView>(Resource.Id.createRecordForm);
            createRecordForm.Click += CreateRecordForm_Click;
        }
    
        private void CreateRecordForm_Click(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new CreateAnxityRecord(), "AddAnxityForm");
            trans.Commit();
        }

        void SendOutSampleNotification(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(Resource.Id.contentFragment, new Journal(), "Journal");
            trans.Commit();

            try
            {
                // Use default vibration length
                Vibration.Vibrate();

                // Or use specified tim
                var duration = TimeSpan.FromSeconds(2);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            // Build the notification:
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentTitle("Button Clicked") // Set the title
                          .SetSmallIcon(Resource.Drawable.arrow) // This is the icon to display
                          .SetContentText("The button has been clicked times."); // the message to display.

            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());

            //Increment the button press count:

        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var name = Resources.GetString(Resource.String.app_name);
            var description = GetString(Resource.String.app_name);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);

        }

        void populateFromDb(object sender, EventArgs e)
        {
            
            var records = new AnxityDatabase().GetAnxityRecordsAsync().Result;
            foreach( AnxityRecords anx in records)
            {

            }

        }
        void newMethod()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "anxity.db");
            var db = new SQLiteConnection(dbPath);
        }

    
    }
}