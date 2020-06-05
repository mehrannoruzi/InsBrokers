using Elk.Core;
using InsBrokers.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class AddressSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public Guid? UserId { get; set; }

        [Display(Name = nameof(Strings.Address), ResourceType = typeof(Strings))]
        public string Details { get; set; }
    }
}
