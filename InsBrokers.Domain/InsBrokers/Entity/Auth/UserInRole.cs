using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(UserInRole), Schema = "Auth")]
    public class UserInRole : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserInRoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        [Display(Name = nameof(Strings.Role), ResourceType = typeof(Strings))]
        public int RoleId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //[Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        //public User User { get; set; }
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid UserId { get; set; }
    }
}
