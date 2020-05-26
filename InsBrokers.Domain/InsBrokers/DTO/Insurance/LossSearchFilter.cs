using Elk.Core;
using System;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class LossSearchFilter : PagingParameter
    {
        public Guid? UserId { get; set; }

        [Display(Name = nameof(Strings.LossType), ResourceType = typeof(Strings))]
        public string LossType { get; set; }

        [Display(Name = nameof(Strings.LossDateShFrom), ResourceType = typeof(Strings))]
        public string LossDateShFrom { get; set; }

        [Display(Name = nameof(Strings.LossDateShTo), ResourceType = typeof(Strings))]
        public string LossDateShTo { get; set; }

        [Display(Name = nameof(Strings.NationalCode), ResourceType = typeof(Strings))]
        public string NationalCode { get; set; }
    }
}
