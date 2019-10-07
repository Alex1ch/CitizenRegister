using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CitizenRegisterWeb.Migrations;
using Microsoft.Extensions.Configuration;

namespace CitizenRegisterWeb.Support
{
    /// <summary>
    /// Abstract class for DB Connection/Migration/Configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataBaseConfiguration
    {
        protected IConfiguration dbConfiguration;

        public IConfiguration DbConfiguration => dbConfiguration;

        public virtual object GetConnection()
        {
            throw new NotImplementedException();
        }
        
        public virtual void Migrate(IMigration migration, bool dropTables = false)
        {
            using (var connection = GetConnection() as DbConnection)
            {
                connection.Open();
                migration.Apply(connection, dropTables);
                connection.Close();
            }
        }

        public virtual DbCommand NewCommand(string cmd, DbConnection connection)
        {
            throw new NotImplementedException();
        }

        public virtual DbCommand NewCommand(string cmd)
        {
            throw new NotImplementedException();
        }

        public DbParameter CreateDateParameter(DbCommand cmd, DateTime date, string name)
        {
            DbParameter param = cmd.CreateParameter();
            param.DbType = DbType.DateTime;
            param.Direction = ParameterDirection.Input;
            param.Value = date;
            param.ParameterName = name;
            return param;
        }
        
        public DbParameter CreateStringParameter(DbCommand cmd, string input, string name)
        {
            DbParameter param = cmd.CreateParameter();
            param.DbType = DbType.String;
            param.Direction = ParameterDirection.Input;
            param.Value = input;
            param.ParameterName = name;
            return param;
        }

        public virtual bool OffsetAfterSelect { get => false; }

        public virtual string OffsetKeyWord(int offset)
        {
            throw new NotImplementedException();
        }

        public virtual string LimitKeyWord(int limit)
        {
            throw new NotImplementedException();
        }
    }
}
