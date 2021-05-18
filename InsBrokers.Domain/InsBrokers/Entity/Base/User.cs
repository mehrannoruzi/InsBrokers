using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(User), Schema = "Base")]
    public class User : IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public long MobileNumber { get; set; }

        [Display(Name = nameof(Strings.Gender), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Gender Gender { get; set; }

        [Display(Name = nameof(Strings.BaseInsurance), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public BaseInsuranceType BaseInsurance { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public bool IsActive { get; set; }

        [Display(Name = nameof(Strings.HasAccidentsInsurance), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public bool HasAccidentsInsurance { get; set; }

        [Display(Name = nameof(Strings.IsRecoveredPassword), ResourceType = typeof(Strings))]
        public bool MustChangePassword { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Display(Name = nameof(Strings.LastLoginDate), ResourceType = typeof(Strings))]
        public DateTime? LastLoginDateMi { get; set; }

        [Display(Name = nameof(Strings.BirthDay), ResourceType = typeof(Strings))]
        public DateTime BirthDayMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.LastLoginDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string LastLoginDateSh { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.BirthDay), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string BirthDay { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.NationalCode), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NationalCode { get; set; }

        [Column(TypeName = "varchar(10)")]
        [Display(Name = nameof(Strings.IdentityNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string IdentityNumber { get; set; }

        [Column(TypeName = "varchar(20)")]
        [Display(Name = nameof(Strings.InsuranceNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsuranceNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Strings.Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Password { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Display(Name = nameof(Strings.NewPassword), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NewPassword { get; set; }

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

        [Column(TypeName = "varchar(50)")]
        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [EmailAddress(ErrorMessageResourceName = nameof(ErrorMessage.WrongEmailFormat), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = nameof(Strings.Plan), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsurancePlan { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = nameof(Strings.Organization), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Organization { get; set; }



        [NotMapped]
        [Display(Name = nameof(Strings.FullName), ResourceType = typeof(Strings))]
        public string Fullname => $"{Name} {Family}";


        [Display(Name = nameof(Strings.Relatives), ResourceType = typeof(Strings))]
        public List<Relative> Relatives { get; set; }

        [Display(Name = nameof(Strings.Losses), ResourceType = typeof(Strings))]
        public List<Loss> losses { get; set; }

        [Display(Name = nameof(Strings.Addresses), ResourceType = typeof(Strings))]
        public List<Address> Addresses { get; set; }
        
        [Display(Name = nameof(Strings.BankAccounts), ResourceType = typeof(Strings))]
        public List<BankAccount> BankAccounts { get; set; }

        [Display(Name = nameof(Strings.UserAttachments), ResourceType = typeof(Strings))]
        public List<UserAttachment> UserAttachments { get; set; }
    }
}