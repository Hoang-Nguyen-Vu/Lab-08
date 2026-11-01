//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class InvalidDeviceReadingException : Exception
    {
        public string DeviceId { get; private set; }
        public int InvalidValue { get; private set; }

        public InvalidDeviceReadingException(string deviceId, int invalidValue)
            : base("Chỉ số không hợp lệ cho thiết bị '" + deviceId + "': " + invalidValue + " (yêu cầu từ 0 đến 100).")
        {
            DeviceId = deviceId;
            InvalidValue = invalidValue;
        }

        public InvalidDeviceReadingException(string deviceId, int invalidValue, string message)
            : base(message)
        {
            DeviceId = deviceId;
            InvalidValue = invalidValue;
        }
    }
}