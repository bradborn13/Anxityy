using Android.Renderscripts;
using SQLite;

public class AnxityRecords
{
    [PrimaryKey, AutoIncrement]
    public int _id { get; set; }
    public string location { get; set; }
    public string date { get; set; }
    public int rating { get; set; }
    public string comment { get; set; }
    public string type { get; set; } 

 public AnxityRecords(string location, string date, int rating, string comment, string type)
{
        this.location = location;
        this.date = date;
        this.rating = rating;
        this.comment = comment;
        this.type = type;
}
    public AnxityRecords()
    {
  
    }
}