using System.Collections.Generic;

namespace InsBrokers.Domain
{
    public class InsuranceInformation
    {
        public int PaymentPart1 { get; set; }
        public int PaymentPart2 { get; set; }
        public int PaymentPart3 { get; set; }
        public string PaymentPart2Date { get; set; }
        public string PaymentPart3Date { get; set; }

        public List<InsuranceDetail> Details { get; set; }

    }
    public class InsuranceDetail
    {
        public string Type { get; set; }
        public string Plan { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int TotalPrice { get; set; }
    }
}
