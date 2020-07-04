using Elk.Core;
using System;

namespace InsBrokers.Domain
{
    public class ContactUsSearchFilter : PagingParameter
    {
        public Guid UserId { get; set; }
        public string Subject { get; set; }
        public string MobileNumber { get; set; }
    }
}
