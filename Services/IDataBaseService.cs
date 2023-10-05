using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSensor.Services
{
    public interface IDataBaseService
    {
        bool DatabaseOpen(string _userName, string _pw, string _server, string _database, string _tableName);
        void DatabaseAdd(string timer, string data);
        bool DatabaseClose();
    }
}
