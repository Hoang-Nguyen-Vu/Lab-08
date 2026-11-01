//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class DeviceStatusChangedEventArgs : EventArgs
    {
        public string DeviceId { get; private set; }
        public DeviceStatus OldStatus { get; private set; }
        public DeviceStatus NewStatus { get; private set; }
        public DateTime ChangedAt { get; private set; }

        public DeviceStatusChangedEventArgs(
            string deviceId,
            DeviceStatus oldStatus,
            DeviceStatus newStatus,
            DateTime changedAt)
        {
            DeviceId = deviceId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedAt = changedAt;
        }
    }
}