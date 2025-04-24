using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DBFusion.Factories
{
    public enum DatabaseType
    {
        SQL_SERVER, // Microsoft SQL Server
        MYSQL, // MySQL
        POSTGRESQL, // PostgreSQL
        SQLITE, // SQLite
        MONGODB, // MongoDB (NoSQL)
        ACCESS // Access
    }
}
