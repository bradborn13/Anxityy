using Android.Renderscripts;
using Android.Text.Format;
using Java.Util;
using SQLite;

public class AnxityRecords
{
    [PrimaryKey, AutoIncrement]
    public int _id { get; set; }
    public string locationLat { get; set; }
    public string locationLong { get; set; }
    public string date { get; set; }
    public int rating { get; set; }
    public string note { get; set; }
    public string type { get; set; }
    public string locationName { get; set; }
    public AnxityRecords( string date , int rating, string note, string type, string locationName = null, string locationLat = null, string locationLong = null)
{
        this.locationLat = locationLat;
        this.locationLong = locationLong;
        this.locationName = locationName;
        this.date = date;
        this.rating = rating;
        this.note = note;
        this.type = type;
}
   
    public AnxityRecords()
    {
  
    }
}