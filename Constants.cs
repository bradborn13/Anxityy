using System;
using System.IO;

public static class Constants
{
    public const string DatabaseFilename = "anxity.db";
    public static string dbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "anxity.db");
    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath
    {
        get
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(basePath, DatabaseFilename);
        }
    }
}