namespace InsBrokers.Notifier.Service
{
    public class CreateNotificationFactory
    {
        public static ICreateStrategy GetStrategy(string NotifyStrategy)
        {
            switch (NotifyStrategy.ToLower())
            {
                case "sms":
                    return new CreateSmsStrategy();
                    
                case "telebot":
                    return new CreateTeleBotStrategy();
                    
                case "email":
                    return new CreateEmailStrategy();
                    

                default:
                    return new CreateSmsStrategy();
            }
        }
    }
}