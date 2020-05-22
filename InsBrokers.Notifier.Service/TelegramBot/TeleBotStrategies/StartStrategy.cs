using Telegram.Bot;
using Telegram.Bot.Args;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using InsBrokers.Notifier.Service.Resource;

namespace InsBrokers.Notifier.Service
{
    public class StartStrategy : ITeleBotStrategy
    {
        public void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs)
        {
            botClient.SendTextMessageAsync(
            chatId: eventArgs.Message.From.Id,
            text: ServiceMessage.WelcomeMessage,
            replyMarkup: new ReplyKeyboardMarkup(
                    keyboardRow: new List<KeyboardButton>
                        {
                            new KeyboardButton() { Text  = ServiceMessage.SendMobileNumber, RequestContact = true }
                        },
                    resizeKeyboard: true, 
                    oneTimeKeyboard: false
                )
            );
        }
    }
}