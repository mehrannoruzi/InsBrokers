using Elk.Core;
using InsBrokers.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(Role), Schema = "Auth")]
    public class Role : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Display(Name = nameof(Strings.Enabled), ResourceType = typeof(Strings))]
        public bool Enabled { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = nameof(Strings.NameFa), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string RoleNameFa { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Display(Name = nameof(Strings.NameEn), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(30, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string RoleNameEn { get; set; }

        public ICollection<ActionInRole> ActionInRoles { get; set; }
        public ICollection<UserInRole> UserInRoles { get; set; }
    }
}