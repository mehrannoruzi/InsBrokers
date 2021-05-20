using InsBrokers.Domain;
using Microsoft.AspNetCore.Http;

namespace InsBrokers.Portal
{
    public class AttachmentModel
    {
        public IFormFile File { get; set; }
        public AttachmentType Type { get; set; }
    }
}
