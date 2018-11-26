using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGuardian.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}