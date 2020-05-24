using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum LossStatus: byte
    {
        [Description("ثبت شده")]
        Added = 0,

        [Description("پرداهت شده")]
        Payed = 1
    }
}
