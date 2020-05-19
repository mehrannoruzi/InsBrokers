using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(VerificationCode), Schema = "Auth")]
    public class VerificationCode : IInsertDateProperties, IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VerificationCodeId { get; set; }

        [Display(Name = nameof(Strings.IsUsed), ResourceType = typeof(Strings))]
        public bool IsUsed { get; set; }

        [Display(Name = nameof(Strings.UsedDate), ResourceType = typeof(Strings))]
        public DateTime? UsedDateMi { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid UserId { get; set; }

        [Column(TypeName = "varchar(6)")]
        [Display(Name = nameof(Strings.Code), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(6, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(6, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Code { get; set; }
    }
}