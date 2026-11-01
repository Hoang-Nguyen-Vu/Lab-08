//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class FilteredAlertSubscriber : IDeviceAlertSubscriber
    {
        private readonly AlertFilter filter;
        private readonly IDeviceAlertSubscriber innerSubscriber;
        private readonly Action<DeviceAlertEventArgs> action;

        public FilteredAlertSubscriber(
            AlertFilter filter,
            Action<DeviceAlertEventArgs> action)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.filter = filter;
            this.action = action;
            this.innerSubscriber = null;
        }

        public FilteredAlertSubscriber(
            AlertFilter filter,
            IDeviceAlertSubscriber innerSubscriber)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (innerSubscriber == null)
            {
                throw new ArgumentNullException("innerSubscriber");
            }

            this.filter = filter;
            this.innerSubscriber = innerSubscriber;
            this.action = null;
        }

        public void HandleAlert(object sender, DeviceAlertEventArgs e)
        {
            if (filter(e))
            {
                if (action != null)
                {
                    action(e);
                }
                if (innerSubscriber != null)
                {
                    innerSubscriber.HandleAlert(sender, e);
                }
            }
        }
    }
}