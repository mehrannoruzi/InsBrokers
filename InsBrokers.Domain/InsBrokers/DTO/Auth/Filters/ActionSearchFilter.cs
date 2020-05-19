using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class ActionSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameF { get; set; }

        [Display(Name = nameof(Strings.ControllerName), ResourceType = typeof(Strings))]
        [MaxLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ControllerNameF { get; set; }

        [Display(Name = nameof(Strings.Action), ResourceType = typeof(Strings))]
        [MaxLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ActionNameF { get; set; }
    }
}