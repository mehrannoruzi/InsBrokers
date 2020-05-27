using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace InsBrokers.Domain
{
    public class PortalSignUpModel
    {
        [Display(Name = nameof(Strings.Gender), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Gender Gender { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Name { get; set; }

        [Display(Name = nameof(Strings.Family), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Family { get; set; }

        [Display(Name = nameof(Strings.FatherName), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string FatherName { get; set; }

        [Display(Name = nameof(Strings.NationalCode), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NationalCode { get; set; }

        [Display(Name = nameof(Strings.IdentityNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string IdentityNumber { get; set; }

        [Display(Name = nameof(Strings.Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Password { get; set; }

        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidMobileNumber), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string MobileNumber { get; set; }

        [Display(Name = nameof(Strings.BirthDay), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string BirthDay { get; set; }

        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [EmailAddress(ErrorMessageResourceName = nameof(ErrorMessage.WrongEmailFormat), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Email { get; set; }

        [Display(Name = nameof(Strings.BaseInsurance), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public BaseInsuranceType BaseInsurance { get; set; }

        [Display(Name = nameof(Strings.Province), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Province { get; set; }

        [Display(Name = nameof(Strings.City), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string City { get; set; }

        [Display(Name = nameof(Strings.Address), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string AddressDetails { get; set; }

        [Display(Name = nameof(Strings.BankName), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public BankName BankName { get; set; }

        [Display(Name = nameof(Strings.AccountNumber), ResourceType = typeof(Strings))]
        [MaxLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string AccountNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Display(Name = nameof(Strings.Shaba), ResourceType = typeof(Strings))]
        [MaxLength(26, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(26, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [RegularExpression(@"^(IR|ir)\d{24}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidShaba), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Shaba { get; set; }

        public int MemberRoleId { get; set; }

    }
}
