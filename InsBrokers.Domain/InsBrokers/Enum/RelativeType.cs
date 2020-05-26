using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum RelativeType : byte
    {
        [Description("بیمه شده اصلی")]
        MainPerson = 1,

        [Description("همسر")]
        Partner = 12,

        [Description("پسر")]
        Boy = 7,

        [Description("دختر")]
        Girl = 8,

        [Description("پدر")]
        Father = 5,

        [Description("مادر")]
        Mother = 6,

        [Description("پدربزرگ/مادربزرگ")]
        Grandpa = 11,
    }
}
