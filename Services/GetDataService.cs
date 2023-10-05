using System;

namespace MonitoringSensor.Services
{
    class GetDataService
    {
        public Action Method;

        private string stringData;
        public string StringData
        {
            get { return stringData; }
            set
            {
                stringData = value;
                if (Method != null)
                {
                    Method.Invoke();
                }
            }
        }
    }
}
