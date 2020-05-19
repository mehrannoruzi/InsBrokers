using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class UserSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        public string MobileNumberF { get; set; }

        [Display(Name = nameof(Strings.FullName), ResourceType = typeof(Strings))]
        [MaxLength(60, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(60, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string FullNameF { get; set; }

        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string EmailF { get; set; }
    }
}