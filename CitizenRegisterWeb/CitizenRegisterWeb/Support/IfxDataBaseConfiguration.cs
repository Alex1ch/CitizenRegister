using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CitizenRegisterWeb.Migrations;
using IBM.Data.DB2.Core;
using Microsoft.Extensions.Configuration;

namespace CitizenRegisterWeb.Support
{
    /// <summary>
    /// DataBaseConfiguration for Informix DataBase
    /// </summary>
    public class IfxDataBaseConfiguration : DataBaseConfiguration
    {
        public IfxDataBaseConfiguration(IConfiguration config)
        {
            dbConfiguration = config.GetSection("IfxDbSettings");
        }

        public override object GetConnection()
        {
            string connectionString = $"Server={ dbConfiguration["DbServer"] };" +
                                      $"Database={ dbConfiguration["DbDatabase"] };" +
                                      $"UID={ dbConfiguration["DbUser"] };" +
                                      $"PWD={ dbConfiguration["DbPassword"] };";

            var connection = new DB2Connection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        public override DbCommand NewCommand(string cmd, DbConnection connection)
        {
            return new DB2Command(cmd,connection as DB2Connection);
        }

        public override bool OffsetAfterSelect { get => true; }

        public override string OffsetKeyWord(int offset)
        {
            return $"SKIP {offset}";
        }

        public override string LimitKeyWord(int limit)
        {
            return $"LIMIT {limit}";
        }
    }
}
