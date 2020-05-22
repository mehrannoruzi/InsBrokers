using Telegram.Bot;
using Telegram.Bot.Args;
using InsBrokers.Notifier.Service.Resource;

namespace InsBrokers.Notifier.Service
{
    public class JuncStrategy : ITeleBotStrategy
    {
        public void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs)
        {
            botClient.SendTextMessageAsync(eventArgs.Message.From.Id, ServiceMessage.JunkMessage);
        }
    }
}