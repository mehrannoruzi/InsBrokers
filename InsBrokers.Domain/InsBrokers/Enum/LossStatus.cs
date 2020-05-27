using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum LossStatus: byte
    {
        [Description("ثبت شده")]
        Added = 0,

        [Description("رد شده")]
        Denied = 1,

        [Description("تایید شده")]
        Agreed = 2,

        [Description("پرداخت شده")]
        Payed = 3
    }
}
