using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum TakafolKind : byte
    {
        [Description("تحت تکفل")]
        UnderTakafol = 21,

        [Description("غیر تحت تکفل")]
        NonUnderTakafol = 22
    }
}
