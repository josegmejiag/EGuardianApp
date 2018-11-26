using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;
using EGuardian.iOS.Services;
using EGuardian.Interfaces;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace EGuardian.iOS.Services
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EGuardianSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}