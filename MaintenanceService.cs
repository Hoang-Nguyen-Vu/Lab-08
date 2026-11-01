//Ngô Trường Phúc 202418964

using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class MaintenanceService : IDeviceAlertSubscriber
    {
        private readonly List<string> requests = new List<string>();

        public IReadOnlyList<string> Requests
        {
            get { return requests.AsReadOnly(); }
        }

        public void HandleAlert(object sender, DeviceAlertEventArgs e)
        {
            HandleCriticalAlert(sender, e);
        }

        public void HandleCriticalAlert(object sender, DeviceAlertEventArgs e)
        {
            if (e.Severity != AlertSeverity.Critical)
            {
                return;
            }

            requests.Add("Maintenance request for " + e.DeviceId + ": " + e.Message);
        }

        public void Clear()
        {
            requests.Clear();
        }
    }
}