using InsBrokers.Domain;
using System.Collections.Generic;

namespace InsBrokers.Portal
{
    public class ManageAttachmentModel
    {
        public List<AttachmentType> Types { get; set; }
        public List<BaseAttachment> Attachments { get; set; }
    }
}
