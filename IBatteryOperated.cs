//Ngô Trường Phúc 202418964

namespace DeviceManagement
{
    public interface IBatteryOperated
    {
        int BatteryLevel { get; }
        void UpdateBatteryLevel(int newLevel);
    }
}