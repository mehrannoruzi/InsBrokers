using Telegram.Bot;
using Telegram.Bot.Args;

namespace InsBrokers.Notifier.Service
{
    public interface ITeleBotStrategy
    {
        void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs);
    }
}