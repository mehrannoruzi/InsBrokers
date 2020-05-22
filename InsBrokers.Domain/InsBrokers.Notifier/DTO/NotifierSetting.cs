using Elk.Core;

namespace InsBrokers.Domain
{
    public class NotifierSetting
    {
        public NotifierSetting(string notifierUrl, string notificationProcessPattern,
            string host, string username, string password)
        {
            NotifierUrl = notifierUrl;
            NotificationProcessPattern = notificationProcessPattern;

            EmailService = new EmailService(host, username, password);
        }

        public string NotifierUrl { get; set; }
        public string NotificationProcessPattern { get; set; }


        public EmailService EmailService { get; set; }
    }
}
