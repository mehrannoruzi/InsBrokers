using InsBrokers.Portal.Resource;
using System.ComponentModel.DataAnnotations;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal
{
    public class SignInModel
    {
        [Display(Name = nameof(DomainString.MobileNumber), ResourceType = typeof(DomainString))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        //[RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(DomainError.InvalidMobileNumber), ErrorMessageResourceType = typeof(DomainError))]
        public string MobileNumber { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = nameof(DomainString.NationalCode), ResourceType = typeof(DomainString))]
        [MinLength(5, ErrorMessageResourceName = nameof(Strings.MinLength), ErrorMessageResourceType = typeof(Strings))]
        [MaxLength(15, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 5, ErrorMessageResourceName = nameof(Strings.Min5MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string Password { get; set; }

        [Display(Name = nameof(Strings.RememberMe), ResourceType = typeof(Strings))]
        public bool RememberMe { get; set; }
    }
}