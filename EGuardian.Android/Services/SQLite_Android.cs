using System.IO;
using EGuardian.Droid.Services;
using EGuardian.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace EGuardian.Droid.Services
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "EGuardianSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}