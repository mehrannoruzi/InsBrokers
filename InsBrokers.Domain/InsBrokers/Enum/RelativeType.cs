using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum RelativeType : byte
    {
        [Description("پدر")]
        Father = 105,

        [Description("مادر")]
        Mother = 106,

        [Description("همسر")]
        Partner = 107,

        [Description("پسر")]
        Boy = 108,

        [Description("دختر")]
        Girl = 109,

        //[Description("پدربزرگ")]
        //GrandPa = 110,

        //[Description("مادربزرگ")]
        //GrandMa = 111,
    }
}