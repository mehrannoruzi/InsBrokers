﻿using System;
using Elk.Core;
using InsBrokers.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsBrokers.Domain
{
    [Table(nameof(RelativeAttachment), Schema = "Base")]
    public class RelativeAttachment : BaseAttachment, IInsertDateProperties, ISoftDeleteProperty, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RelativeAttachmentId { get; set; }

        [ForeignKey(nameof(RelativeId))]
        [Display(Name = nameof(Strings.Relatives), ResourceType = typeof(Strings))]
        public Relative Relative { get; set; }
        [Display(Name = nameof(Strings.Relatives), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int RelativeId { get; set; }
        public override int GetId() => RelativeAttachmentId;
        //[Display(Name = nameof(Strings.FileType), ResourceType = typeof(Strings))]
        //public FileType FileType { get; set; }

        //[Display(Name = nameof(Strings.UserAttachmentType), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public AttachmentType UserAttachmentType { get; set; }

        //[Display(Name = nameof(Strings.Size), ResourceType = typeof(Strings))]
        //public long Size { get; set; }

        //[Display(Name = nameof(Strings.IsDeleted), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public bool IsDeleted { get; set; }

        //[Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        //public DateTime InsertDateMi { get; set; }

        //[Column(TypeName = "char(10)")]
        //[Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        //[MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public string InsertDateSh { get; set; }

        //[Column(TypeName = "varchar(5)")]
        //[Display(Name = nameof(Strings.Extention), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        //[MaxLength(5, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public string Extention { get; set; }

        //[Column(TypeName = "varchar(50)")]
        //[Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        //[MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public string Name { get; set; }

        //[Column(TypeName = "varchar(250)")]
        //[Display(Name = nameof(Strings.Url), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        //[MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        //public string Url { get; set; }
    }
}