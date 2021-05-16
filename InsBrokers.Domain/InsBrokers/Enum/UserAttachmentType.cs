using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum UserAttachmentType
    {
        [Description("کارت ملی")]
        NationalCard = 1,

        [Description("شناسنامه")]
        IdentityCard = 2,

        [Description("عکس پرسنلی")]
        PersoneliPicture = 3,

        [Description("دفترچه بیمه")]
        InsuranceBooklet = 4,
    }
}