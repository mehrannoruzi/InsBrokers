using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum UserAttachmentType
    {
        [Description("تصویر کارت ملی")]
        NationalCard = 1,

        [Description("تصویر شناسنامه")]
        IdentityCard = 2,

        [Description("عکس پرسنلی")]
        PersoneliPicture = 3,

        [Description("تصویر دفترچه بیمه")]
        InsuranceBooklet = 4,
    }
}