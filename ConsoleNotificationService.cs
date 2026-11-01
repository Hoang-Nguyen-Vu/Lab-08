//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class ConsoleNotificationService : IDeviceAlertSubscriber
    {
        public void HandleAlert(object sender, DeviceAlertEventArgs e)
        {
            Console.WriteLine("[" + e.Severity + "] " + e.DeviceId + ": " + e.Message);
        }
    }
}