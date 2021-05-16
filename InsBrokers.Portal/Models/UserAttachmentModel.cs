using InsBrokers.Domain;
using Microsoft.AspNetCore.Http;

namespace InsBrokers.Portal
{
    public class UserAttachmentModel
    {
        public IFormFile File { get; set; }
        public UserAttachmentType Type { get; set; }
    }
}
