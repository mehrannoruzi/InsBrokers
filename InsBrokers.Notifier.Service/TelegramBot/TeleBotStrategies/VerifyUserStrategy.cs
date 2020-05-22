using Elk.Core;
using Elk.Http;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace InsBrokers.Notifier.Service
{
    public class VerifyUserStrategy : ITeleBotStrategy
    {
        public void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs)
        {
            var TeleBotUser = new
            {
                MobileNumber = long.Parse(eventArgs.Message.Contact.PhoneNumber),
                ChatId = eventArgs.Message.From.Id
            };

            var registerResult = HttpRequestTools.PostJson<IResponse<bool>>("http://InsBrokers.me/notifier/verifyTelebotuser", TeleBotUser);
            //send register result message to user
            //botClient.SendTextMessageAsync(
            //chatId: eventArgs.Message.From.Id,
            //text: ServiceMessage.WelcomeMessage,
            //replyMarkup: new ReplyKeyboardMarkup(
            //        keyboardRow: new List<KeyboardButton>
            //            {
            //                new KeyboardButton() { Text  = ServiceMessage.SendMobileNumber, RequestContact = true }
            //            },
            //        resizeKeyboard: true,
            //        oneTimeKeyboard: false
            //    )
            //);
        }
    }
}