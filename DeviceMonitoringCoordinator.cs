//Ngô Trường Phúc 202418964

using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class DeviceMonitoringCoordinator : IDisposable
    {
        private readonly List<TeachingDevice> devices;
        private readonly List<IDeviceAlertSubscriber> alertSubscribers;
        private readonly List<IDeviceStatusSubscriber> statusSubscribers;

        private bool isSubscribed = false;
        private bool isDisposed = false;

        public DeviceMonitoringCoordinator(
            IEnumerable<TeachingDevice> devices,
            IEnumerable<IDeviceAlertSubscriber> alertSubscribers,
            IEnumerable<IDeviceStatusSubscriber> statusSubscribers)
        {
            this.devices = devices != null ? new List<TeachingDevice>(devices) : new List<TeachingDevice>();
            this.alertSubscribers = alertSubscribers != null ? new List<IDeviceAlertSubscriber>(alertSubscribers) : new List<IDeviceAlertSubscriber>();
            this.statusSubscribers = statusSubscribers != null ? new List<IDeviceStatusSubscriber>(statusSubscribers) : new List<IDeviceStatusSubscriber>();
        }

        public DeviceMonitoringCoordinator(
            IEnumerable<TeachingDevice> devices,
            ConsoleNotificationService notificationService,
            DeviceAuditLogger auditLogger,
            MaintenanceService maintenanceService)
        {
            this.devices = devices != null ? new List<TeachingDevice>(devices) : new List<TeachingDevice>();

            this.alertSubscribers = new List<IDeviceAlertSubscriber>();
            if (notificationService != null)
            {
                this.alertSubscribers.Add(notificationService);
            }
            if (maintenanceService != null)
            {
                this.alertSubscribers.Add(maintenanceService);
            }

            this.statusSubscribers = new List<IDeviceStatusSubscriber>();
            if (auditLogger != null)
            {
                this.statusSubscribers.Add(auditLogger);
            }
        }

        public void Subscribe()
        {
            if (isSubscribed)
            {
                return;
            }

            foreach (var device in devices)
            {
                foreach (var statusSub in statusSubscribers)
                {
                    device.StatusChanged += statusSub.HandleStatusChanged;
                }
                foreach (var alertSub in alertSubscribers)
                {
                    device.AlertRaised += alertSub.HandleAlert;
                }
            }

            isSubscribed = true;
        }

        public void Unsubscribe()
        {
            if (!isSubscribed)
            {
                return;
            }

            foreach (var device in devices)
            {
                foreach (var statusSub in statusSubscribers)
                {
                    device.StatusChanged -= statusSub.HandleStatusChanged;
                }
                foreach (var alertSub in alertSubscribers)
                {
                    device.AlertRaised -= alertSub.HandleAlert;
                }
            }

            isSubscribed = false;
        }

        public void AddDevice(TeachingDevice device)
        {
            if (device == null || devices.Contains(device))
            {
                return;
            }

            devices.Add(device);

            if (isSubscribed)
            {
                foreach (var statusSub in statusSubscribers)
                {
                    device.StatusChanged += statusSub.HandleStatusChanged;
                }
                foreach (var alertSub in alertSubscribers)
                {
                    device.AlertRaised += alertSub.HandleAlert;
                }
            }
        }

        public void RemoveDevice(TeachingDevice device)
        {
            if (device == null || !devices.Contains(device))
            {
                return;
            }

            if (isSubscribed)
            {
                foreach (var statusSub in statusSubscribers)
                {
                    device.StatusChanged -= statusSub.HandleStatusChanged;
                }
                foreach (var alertSub in alertSubscribers)
                {
                    device.AlertRaised -= alertSub.HandleAlert;
                }
            }

            devices.Remove(device);
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                Unsubscribe();
                isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}