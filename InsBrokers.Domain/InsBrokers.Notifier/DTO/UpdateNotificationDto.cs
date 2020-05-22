using System;

namespace InsBrokers.Domain
{
    public class UpdateNotificationDto
    {
        public int NotificationId { get; set; }
        public NotificationStatus Status { get; set; }
        public string SendStatus { get; set; }
        public DateTime SendDateMi { get; set; }
        public bool IsLock { get; set; }
    }
}
