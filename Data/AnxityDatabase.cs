
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Java.Util;

using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


    public class AnxityDatabase
{
        readonly SQLiteAsyncConnection _database;
       string dbPath = Path.Combine(
       Environment.GetFolderPath(Environment.SpecialFolder.Personal),
       "Anxity.db");
    //public static AnxityDatabase Database
    //{
    //    get
    //    {
    //        if (database == null)
    //        {
    //            database = new AnxityDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "anxity.db"));
    //        }
    //        return database;
    //    }
    //}
    public AnxityDatabase()
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<AnxityRecords>().Wait();
        }
    public  List<AnxityRecords>GetCurrentWeekRecords()
   {
        DateTime givenDate = DateTime.Today;
        //string givenDated = DateTime.Now.ToString("dddd, MMMM dd,  yyyy");
        DateTime startOfWeek = givenDate.AddDays(-1 * Convert.ToInt32(givenDate.DayOfWeek));
        DateTime endOfWeek = startOfWeek.AddDays(7);
        var weeksRecords = _database.Table<AnxityRecords>().ToListAsync().Result.Where(ob => DateTime.Compare(startOfWeek, DateTime.Parse(ob.date)) < 0 || DateTime.Compare(startOfWeek, DateTime.Parse(ob.date)) == 0 && DateTime.Compare(DateTime.Parse(ob.date), endOfWeek) > 0).ToList();
;
     
        return weeksRecords;
  }
    
    public Task<List<AnxityRecords>> GetAnxityRecordsAsync()
        {
            return _database.Table<AnxityRecords>().ToListAsync();
        }

    public Task<AnxityRecords> GetAnxityRecordAsync(int id)
        {
            return _database.Table<AnxityRecords>()
                            .Where(i => i._id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAnxityRecordAsync(AnxityRecords record)
        {
            if (record._id != 0)
            {
                return _database.UpdateAsync(record);
            }
            else
            {
                return _database.InsertAsync(record);
            }
        }

        public Task<int> DeleteAnxityRecordAsync(AnxityRecords record)
        {
            return _database.DeleteAsync(record);
        }
    }


