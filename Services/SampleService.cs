using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Java.Util.Functions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

[Service]
public class SampleService : Service
{

    bool isStarted = false;
    Random rnd = new Random();

    static readonly string CHANNEL_ID = "anx_createAnx_bckg";
    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        if (isStarted)
        {
        }
        else
        {

            isStarted = true;
            Accelerometer.Start(SensorSpeed.Game);

        }
        return StartCommandResult.NotSticky;
    }

    public override IBinder OnBind(Intent intent)
    {
        // This is a started service, not a bound service, so we just return null.
        return null;
    }


    public override void OnDestroy()
    {
        isStarted = false;
        base.OnDestroy();
    }

    void HandleTimerCallback(object state)
    {
        Accelerometer.ShakeDetected +=

            Accelerometer_ShakeDetectedAsync;

    }
    private async void Accelerometer_ShakeDetectedAsync(object sender, EventArgs e)
    {

        var duration = TimeSpan.FromSeconds(1);
        Vibration.Vibrate(duration);
        int notificationId = rnd.Next();
        var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                       .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                       .SetContentTitle("Button Clicked") // Set the title
                       .SetSmallIcon(Resource.Drawable.notification_bg) // This is the icon to display
                       .SetVibrate(new long[] { 1000, 1000, 1000, 1000, 1000 })
                       .SetContentText("New record registered!"); // the message to display.

        // Finally, publish the notification:
        var notificationManager = NotificationManagerCompat.From(this);
        notificationManager.Notify(notificationId, builder.Build());

        await CreateDinamicAnxityRecord();

    }
    public async Task<int> CreateDinamicAnxityRecord()
    {
        AnxityRecords anxObj = new AnxityRecords();
        anxObj.date = DateTime.Now.ToString();
        var location = await new LocationTracker().getCurrentLocation();
        anxObj.locationLong = location.Longitude.ToString();
        anxObj.locationLat = location.Latitude.ToString();
        var db = new AnxityDatabase();
        int recordId = await db.SaveAnxityRecordAsync(anxObj);
        return recordId;
    }
}
