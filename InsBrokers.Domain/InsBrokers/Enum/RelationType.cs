using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum RelationType : byte
    {
        [Description("بیمه شده اصلی")]
        MainPerson = 1,

        [Description("همسر")]
        Wife = 2,

        [Description("فرزند")]
        Child = 3,

        [Description("پدر/مادر")]
        Parent = 4,

        [Description("پدربزرگ/مادربزرگ")]
        Grandpa = 5
    }
}
