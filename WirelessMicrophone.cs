//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class WirelessMicrophone : TeachingDevice, IBatteryOperated
    {
        public int BatteryLevel { get; private set; }
        public int SignalQuality { get; private set; }

        public WirelessMicrophone(string id, string name, int initialBattery = 100, int initialSignalQuality = 100)
            : base(id, name)
        {
            if (initialBattery < 0 || initialBattery > 100)
            {
                throw new InvalidDeviceReadingException(id, initialBattery);
            }
            if (initialSignalQuality < 0 || initialSignalQuality > 100)
            {
                throw new InvalidDeviceReadingException(id, initialSignalQuality);
            }

            BatteryLevel = initialBattery;
            SignalQuality = initialSignalQuality;
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
                RaiseAlert("Pin micro: 0%", AlertSeverity.Critical);
            }
            else if (newLevel < 30)
            {
                RaiseAlert("Pin micro: " + newLevel + "% < 30%", AlertSeverity.Warning);
            }
        }

        public void UpdateSignalQuality(int newQuality)
        {
            if (newQuality < 0 || newQuality > 100)
            {
                throw new InvalidDeviceReadingException(Id, newQuality);
            }

            int oldQuality = SignalQuality;
            SignalQuality = newQuality;

            if (newQuality == 0)
            {
                RaiseAlert("Tín hiệu: 0%", AlertSeverity.Critical);
            }
            else if (newQuality < 40)
            {
                RaiseAlert("Tín hiệu: " + newQuality + "% < 40%", AlertSeverity.Warning);
            }
        }
    }
}