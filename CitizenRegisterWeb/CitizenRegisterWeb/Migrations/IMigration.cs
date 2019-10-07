using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CitizenRegisterWeb.Migrations
{
    /// <summary>
    /// Migration interface that used in DataBaseConfiguration 
    /// </summary>
    public interface IMigration
    {
        void Apply(DbConnection connection, bool dropTables);
    }
}
