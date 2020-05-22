using InsBrokers.Domain;

namespace InsBrokers.Notifier.Service
{
    public class SendNotificationFactory
    {
        public static ISendStrategy GetStrategy(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Sms:
                    return new SendSmsStrategy();

                case NotificationType.TeleBot:
                    return new SendTeleBotStrategy();

                case NotificationType.Email:
                    return new SendEmailStrategy();

                default:
                    return new SendSmsStrategy();
            }
        }
    }
}