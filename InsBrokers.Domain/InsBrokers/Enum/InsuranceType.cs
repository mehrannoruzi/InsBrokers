using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum InsuranceType : byte
    {
        [Description("اصلی")]
        Main = 31,

        [Description("فرعی")]
        Secondary = 32,
    }
}
