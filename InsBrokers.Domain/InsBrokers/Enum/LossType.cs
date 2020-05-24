using System.ComponentModel;

namespace InsBrokers.Domain.InsBrokers.Enum
{
    public enum LossType : byte
    {
        [Description("انواع ویزیت")]
        Visit = 1,

        [Description("انواع دارو")]
        Drug = 2,

        [Description("انواع آزمایش")]
        Test = 3,

        [Description("انواع سونوگرافی")]
        Sonography = 4,

        [Description("عینک")]
        Glass = 5,

        [Description("شنوایی سنجی")]
        Audiologists = 6,

        [Description("لیزر درمانی")]
        LaserTherapy = 7,

        [Description("بستری در بیمارستان")]
        Hospitalization = 8,

        [Description("انواع جراحی")]
        Surgery = 9,

        [Description("زایمان")]
        ChildBirth = 10,

        [Description("دندانپزشکی")]
        Dentistry = 11,

        [Description("انواع هزینه های چشم پزشکی")]
        OphthalmologyCost = 12,

        [Description("هزینه آمبولانس")]
        Ambulance = 13,

        [Description("موارد دیگر")]
        Other = 15
    }
}
