//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public abstract class TeachingDevice
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DeviceStatus Status { get; private set; }

        public event EventHandler<DeviceStatusChangedEventArgs> StatusChanged;
        public event EventHandler<DeviceAlertEventArgs> AlertRaised;

        protected TeachingDevice(string id, string name)
        {
            if (string.IsNullOrEmpty(id) || id.Trim().Length == 0)
            {
                throw new ArgumentException("Mã thiết bị không được để trống.");
            }
            if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
            {
                throw new ArgumentException("Tên thiết bị không được để trống.");
            }

            Id = id;
            Name = name;
            Status = DeviceStatus.Available;
        }

        public void ChangeStatus(DeviceStatus newStatus)
        {
            if (newStatus == Status)
            {
                return;
            }

            DeviceStatus oldStatus = Status;
            Status = newStatus;

            OnStatusChanged(new DeviceStatusChangedEventArgs(
                Id,
                oldStatus,
                newStatus,
                DateTime.Now));
        }

        protected void RaiseAlert(string message, AlertSeverity severity)
        {
            OnAlertRaised(new DeviceAlertEventArgs(
                Id,
                message,
                severity,
                DateTime.Now));
        }

        protected virtual void OnStatusChanged(DeviceStatusChangedEventArgs e)
        {
            EventHandler<DeviceStatusChangedEventArgs> handler = StatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnAlertRaised(DeviceAlertEventArgs e)
        {
            EventHandler<DeviceAlertEventArgs> handler = AlertRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override string ToString()
        {
            return "[" + Id + "] " + Name + " (" + Status + ")";
        }
    }
}