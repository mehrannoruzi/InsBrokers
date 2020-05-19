using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(ActionInRole), Schema = "Auth")]
    public class ActionInRole : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionInRoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        [Display(Name = nameof(Strings.Role), ResourceType = typeof(Strings))]
        public int RoleId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public Action Action { get; set; }
        [Display(Name = nameof(Strings.Action), ResourceType = typeof(Strings))]
        public int ActionId { get; set; }

        [Display(Name = nameof(Strings.IsDefault), ResourceType = typeof(Strings))]
        public bool IsDefault { get; set; }
    }
}