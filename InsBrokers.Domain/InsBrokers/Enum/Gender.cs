using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum Gender : byte
    {
        [Description("مرد")]
        Men = 26,

        [Description("زن")]
        Women = 27
    }
}
