using InsBrokers.Domain;
using System.Collections.Generic;

namespace InsBrokers.Notifier.Service
{
    public class TeleBotFactory
    {
        private static readonly List<TeleBotPatternDto> _patterns = new List<TeleBotPatternDto>
            {
                new TeleBotPatternDto(@"^\/start$", TeleBotRequestType.Start),
                new TeleBotPatternDto(@"^\+989|989[0-9]{9}$", TeleBotRequestType.VerifyUser)
            };


        public static ITeleBotStrategy GetInstance(string input)
        {
            foreach (var item in _patterns)
            {
                if (item.Pattern.IsMatch(input))
                    switch (item.TeleBotRequestType)
                    {
                        case TeleBotRequestType.Start:
                            return new StartStrategy();

                        case TeleBotRequestType.VerifyUser:
                            return new VerifyUserStrategy();
                    }
            }
            return new JuncStrategy();
        }
    }
}