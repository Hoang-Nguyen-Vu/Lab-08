//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class ProjectorRemote : TeachingDevice, IBatteryOperated
    {
        public int BatteryLevel { get; private set; }

        public ProjectorRemote(string id, string name, int initialBattery = 100)
            : base(id, name)
        {
            if (initialBattery < 0 || initialBattery > 100)
            {
                throw new InvalidDeviceReadingException(id, initialBattery);
            }

            BatteryLevel = initialBattery;
        }

        public void UpdateBatteryLevel(int newLevel)
        {
            if (newLevel < 0 || newLevel > 100)
            {
                throw new InvalidDeviceReadingException(Id, newLevel);
            }

            int oldLevel = BatteryLevel;
            BatteryLevel = newLevel;

            if (newLevel == 0)
            {
                RaiseAlert("Pin: 0%! Không thể hoạt động", AlertSeverity.Critical);
            }
            else if (newLevel < 20)
            {
                RaiseAlert("Pin: " + newLevel + "% < 20%, cần sạc hoặc thay pin", AlertSeverity.Warning);
            }
        }
    }
}