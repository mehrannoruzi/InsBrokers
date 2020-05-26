using Elk.Core;
using InsBrokers.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class RelativeSearchFilter : PagingParameter
    {
        public Guid? UserId { get; set; }

        [Display(Name = nameof(Strings.NationalCode), ResourceType = typeof(Strings))]
        public string NationalCode { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        public string Name { get; set; }
    }
}
