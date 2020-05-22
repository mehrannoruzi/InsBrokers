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

        [Description("بانک")]
        Bank = 4,

        [Description("فاقد بیمه پایه")]
        BeduneBimehPayeh = 5,
    }
}
