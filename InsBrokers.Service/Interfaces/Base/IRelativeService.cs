using System;
using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InsBrokers.Service
{
    public interface IRelativeService
    {
        Task<IResponse<Relative>> AddAsync(Relative model);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<Relative>> FindAsync(int roleId);
        Task<IResponse<Relative>> FindWithAttachmentsAsync(int id);
        PagingListDetails<Relative> Get(RelativeSearchFilter filter);
        Task<IResponse<Relative>> UpdateAsync(Relative model);
        Task<IResponse<object>> AddAttachments(IFormFile file, UserAttachmentType type);
        Task<IResponse<bool>> DeleteAttachment(int attachmentId);
        IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10);
        string Export(RelativeSearchFilter filter);
    }
}