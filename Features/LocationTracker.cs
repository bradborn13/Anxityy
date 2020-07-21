// What do I have to do here?
using Android.Gms.Tasks;
using System.Threading.Tasks;
using Xamarin.Essentials;

public class LocationTracker
{
    public LocationTracker()
    {

    }
     public async Task<Location> getCurrentLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        return location;
    }
     public async Task<Location> getLaskKnownLocation()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        return location;
    }


}
