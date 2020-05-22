using PayamakProvider;
using System.Threading.Tasks;
using InsBrokers.InfraStructure;

namespace InsBrokers.Notifier.Service
{
    public class LinePayamakProvider
    {
        private static readonly string _password = GlobalVariables.SmsProviders.LinePayamak.Password;
        private static readonly string _username = GlobalVariables.SmsProviders.LinePayamak.Username;
        private static readonly string _senderId = GlobalVariables.SmsProviders.LinePayamak.SenderId;

        public static async Task<string> SendSimpleSmsAsync(string to, string content)
        {
            var sendResult = await new SendSoapClient().SendSimpleSMSAsync(_username, _password, to, _senderId, content, false);

            return sendResult.Body.SendSimpleSMSResult;
        }

        public static async Task<string> SendSmsAsync(string to, string content)
        {
            var reciver = new ArrayOfString();
            var status = new ArrayOfbyte();
            var smsId = new ArrayOfLong();
            reciver.Add(to);

            var sendResult = await new SendSoapClient().SendSmsAsync(_username, _password, reciver, _senderId, content, false, null, smsId, status);

            return $"{sendResult.Body.recId[0]}:{sendResult.Body.SendSmsResult}:{sendResult.Body.status[0]}";
        }
    }
}