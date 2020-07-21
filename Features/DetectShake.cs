// What do I have to do here?
using System;
using Xamarin.Essentials;

public class DetectShake
{
    SensorSpeed speed = SensorSpeed.Game;
    public DetectShake()
    {
        Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;   
    }
    void Accelerometer_ShakeDetected(Object sender, EventArgs e )
    {

    }
    public void ToggleAccelerometer()
    {
        try
        {
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
         
            }
        
            else
                Accelerometer.Start(speed); 
         

        }
        catch(FeatureNotSupportedException fnEx)
        {
            //Feature not supported on device
        }
        catch(Exception e)
        {
            //Other exception has occured
        }
    }
}