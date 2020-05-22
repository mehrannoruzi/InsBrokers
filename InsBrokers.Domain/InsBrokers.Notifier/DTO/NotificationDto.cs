using System;

namespace InsBrokers.Domain
{
    public class NotificationDto
    {
        public EventType Type { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }
        
        public string FullName { get; set; }
        
        public string Email { get; set; }

        public long MobileNumber { get; set; }
        
        public string TelegramChatId { get; set; }
    }
}
