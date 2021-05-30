using System.ComponentModel;

namespace InsBrokers.Domain
{
    public enum BaseInsuranceType : byte
	{
        [Description("تامین اجتماعی")]
        TaminEjtemaei = 1,

        [Description("خدمات درمانی")]
        KhadamatDarmani = 2,

        [Description("نیروهای مسلح")]
        NiruhayeMosalah = 3,

        [Description("بیمه بانک")]
        Bank = 4,

        [Description("بیمه سلامت")]
        BimehSalat = 6,

        [Description("فاقد بیمه پایه")]
        BeduneBimehPayeh = 5,
    }
}
