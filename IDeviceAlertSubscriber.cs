//Ngô Trường Phúc 202418964

namespace DeviceManagement
{
    public interface IDeviceAlertSubscriber
    {
        void HandleAlert(object sender, DeviceAlertEventArgs e);
    }
}