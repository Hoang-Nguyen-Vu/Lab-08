//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class DeviceAlertEventArgs : EventArgs
    {
        public string DeviceId { get; private set; }
        public string Message { get; private set; }
        public AlertSeverity Severity { get; private set; }
        public DateTime OccurredAt { get; private set; }

        public DeviceAlertEventArgs(
            string deviceId,
            string message,
            AlertSeverity severity,
            DateTime occurredAt)
        {
            DeviceId = deviceId;
            Message = message;
            Severity = severity;
            OccurredAt = occurredAt;
        }
    }
}