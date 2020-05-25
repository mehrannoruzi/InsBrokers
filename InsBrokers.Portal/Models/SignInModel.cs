using DomainString = InsBrokers.Domain.Resource.Strings;
using DomainError = InsBrokers.Domain.Resource.ErrorMessage;
using InsBrokers.Portal.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Portal
{

    public class SignInModel
    {

        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(DomainError.InvalidMobileNumber), ErrorMessageResourceType = typeof(DomainError))]
        [Display(Name = nameof(DomainString.MobileNumber), ResourceType = typeof(DomainString))]
        [Required(ErrorMessageResourceName = nameof(Resource.Strings.Required), ErrorMessageResourceType = typeof(Resource.Strings), AllowEmptyStrings = false)]
        public string MobileNumber { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(15, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [MinLength(5, ErrorMessageResourceName = nameof(Strings.MinLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(15, MinimumLength = 5, ErrorMessageResourceName = nameof(Strings.Min5MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = nameof(Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Display(Name = nameof(Resource.Strings.RememberMe), ResourceType = typeof(Resource.Strings))]
        public bool RememberMe { get; set; }
    }
}