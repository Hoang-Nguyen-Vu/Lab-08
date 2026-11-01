//Ngô Trường Phúc 202418964

namespace DeviceManagement
{
    public interface IDeviceStatusSubscriber
    {
        void HandleStatusChanged(object sender, DeviceStatusChangedEventArgs e);
    }
}