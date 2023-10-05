using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSensor.Services
{
    interface ICsvService
    {

        bool CsvCreate(string line);
        void CsvAdd(string timer, string data);
        bool CsvClose();
    }
}
