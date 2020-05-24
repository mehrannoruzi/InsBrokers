using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class BankAccountSearchFilter : PagingParameter
    {
        public Guid? UserId { get; set; }

        [Display(Name = nameof(Strings.BankName), ResourceType = typeof(Strings))]
        public BankName? Name { get; set; }
    }
}
