using System;
using Elk.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace InsBrokers.Notifier.Service
{
    public class TelegramBot
    {
        public static TelegramBotClient _client;

        
        private static void TeleBot_ReceiveMessage(object sender, MessageEventArgs eventArgs)
        {
            try
            {
                switch (eventArgs.Message.Type)
                {
                    case MessageType.Text:
                        TeleBotFactory.GetInstance(eventArgs.Message.Text).ProcessRequest(_client, sender, eventArgs as MessageEventArgs);
                        break;
                    case MessageType.Contact:
                        TeleBotFactory.GetInstance(eventArgs.Message.Contact.PhoneNumber).ProcessRequest(_client, sender, eventArgs as MessageEventArgs);
                        break;
                }
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
            }
        }

        public static void Initialize(string teleBotToken)
        {
            try
            {
                _client = new TelegramBotClient(teleBotToken);
                _client.OnMessage += TeleBot_ReceiveMessage;
                _client.StartReceiving();
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
            }
        }
    }
}