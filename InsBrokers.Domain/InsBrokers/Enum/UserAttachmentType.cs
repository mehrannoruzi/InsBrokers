using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum UserAttachmentType
    {
        [Description("تصویر کارت ملی")]
        NationalCard = 1,
        
        [Description("تصویر پشت کارت ملی")]
        NationalCardPage2 = 2,

        [Description("تصویر صفحه اول شناسنامه")]
        IdentityCardPage1 = 3,

        [Description("تصویر صفحه دوم شناسنامه")]
        IdentityCardPage2 = 4,

        [Description("عکس پرسنلی")]
        PersoneliPicture = 5,

        [Description("تصویر دفترچه بیمه")]
        InsuranceBooklet = 6,
    }
}