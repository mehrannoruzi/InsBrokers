using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum EventType : byte
    {
        [Description("ثبت نام")]
        Subscription = 1,

        [Description("ثبت هزینه")]
        LossRegister = 2,

        [Description("پرداخت هزینه")]
        LossPay = 3
    }
}