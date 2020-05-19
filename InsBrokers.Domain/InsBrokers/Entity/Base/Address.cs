using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(Address), Schema = "Base")]
    public class Address : IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [ForeignKey(nameof(UserId))]
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public User User { get; set; }
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid UserId { get; set; }

        [Display(Name = nameof(Strings.AddressType), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public AddressType AddressType { get; set; }

        [Display(Name = nameof(Strings.IsDefault), ResourceType = typeof(Strings))]
        public bool IsDefault { get; set; }

        [Display(Name = nameof(Strings.Latitude), ResourceType = typeof(Strings))]
        public double Latitude { get; set; }

        [Display(Name = nameof(Strings.Longitude), ResourceType = typeof(Strings))]
        public double Longitude { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Display(Name = nameof(Strings.Address), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string AddressDetails { get; set; }
    }
}
