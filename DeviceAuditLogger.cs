//Ngô Trường Phúc 202418964

using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class DeviceAuditLogger : IDeviceStatusSubscriber
    {
        private readonly List<string> entries = new List<string>();

        public IReadOnlyList<string> Entries
        {
            get { return entries.AsReadOnly(); }
        }

        public void HandleStatusChanged(object sender, DeviceStatusChangedEventArgs e)
        {
            entries.Add(e.ChangedAt.ToString("yyyy-MM-dd HH:mm:ss") + ": " + e.DeviceId + ", " + e.OldStatus + " -> " + e.NewStatus);
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}