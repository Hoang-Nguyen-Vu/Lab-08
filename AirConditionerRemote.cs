//Ngô Trường Phúc 202418964

using System;

namespace DeviceManagement
{
    public class AirConditionerRemote : TeachingDevice
    {
        public string AssignedRoom { get; private set; }
        public bool IsResponsive { get; private set; }

        public AirConditionerRemote(string id, string name, string assignedRoom, bool initialResponsive = true)
            : base(id, name)
        {
            if (string.IsNullOrEmpty(assignedRoom) || assignedRoom.Trim().Length == 0)
            {
                throw new ArgumentException("Phòng chứa thiết bị không được để trống");
            }

            AssignedRoom = assignedRoom;
            IsResponsive = initialResponsive;
        }

        public void UpdateResponseState(bool isResponsive)
        {
            if (!IsResponsive && !isResponsive)
            {
                return;
            }

            bool oldState = IsResponsive;
            IsResponsive = isResponsive;

            if (oldState && !isResponsive)
            {
                RaiseAlert("Điều khiển điều hòa tại phòng " + AssignedRoom + " không phản hồi tín hiệu", AlertSeverity.Critical);
            }
            else if (!oldState && isResponsive)
            {
                RaiseAlert("Điều khiển điều hòa tại phòng " + AssignedRoom + " phản hồi tín hiệu", AlertSeverity.Information);
            }
        }

    }
}