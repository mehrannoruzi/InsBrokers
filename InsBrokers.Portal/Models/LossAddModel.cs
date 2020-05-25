using InsBrokers.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InsBrokers.Portal
{
    public class LossAddModel : Loss
    {
        public IList<IFormFile> Files { set; get; }
    }
}
