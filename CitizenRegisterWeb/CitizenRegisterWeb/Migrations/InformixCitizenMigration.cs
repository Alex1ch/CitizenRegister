using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.DB2.Core;

namespace CitizenRegisterWeb.Migrations
{
    /// <summary>
    /// Migration Realization of Citizen Table Migration using Informix DB
    /// </summary>
    public class InformixCitizenMigration : IMigration
    {
        public void Apply(DbConnection connection, bool dropTables)
        {
            var createTableCmd = new DB2Command("CREATE TABLE IF NOT EXISTS Citizen (" +
                                                "id SERIAL PRIMARY KEY, " +
                                                "surname CHAR(30) NOT NULL, " +
                                                "name CHAR(30) NOT NULL, " +
                                                "middlename CHAR(40) NOT NULL," +
                                                "birthdate DATETIME YEAR TO DAY NOT NULL" +
                                                ");", connection as DB2Connection);

            var createSurnameIndex = new DB2Command("CREATE INDEX IF NOT EXISTS SurnameIndex ON Citizen (surname);",
                connection as DB2Connection);
            var createSurnameNameIndex = new DB2Command("CREATE INDEX IF NOT EXISTS SurnameNameIndex ON Citizen (surname, name);",
                connection as DB2Connection);
            var createFullNameIndex = new DB2Command("CREATE INDEX IF NOT EXISTS NameIndex ON Citizen (surname, name, middlename);",
                connection as DB2Connection);
            var createBirthDate = new DB2Command("CREATE INDEX IF NOT EXISTS BirthDateIndex ON Citizen (birthdate);",
                connection as DB2Connection);

            if (dropTables)
            {
                var dropTablesCmd = new DB2Command("DROP TABLE IF EXISTS Citizen;", connection as DB2Connection);

                dropTablesCmd.ExecuteNonQuery();
            }

            createTableCmd.ExecuteNonQuery();
            createSurnameIndex.ExecuteNonQuery();
            createSurnameNameIndex.ExecuteNonQuery();
            createFullNameIndex.ExecuteNonQuery();
            createBirthDate.ExecuteNonQuery();
        }
    }
}
