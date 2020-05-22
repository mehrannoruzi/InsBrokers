using Microsoft.Extensions.Configuration;
using InsBrokers.Domain;
using System;

namespace InsBrokers.Dashboard
{
    //public class SingleUploaderModel
    //{
    //    public SingleUploaderModel(IConfiguration configure, AttachedFile attch)
    //    {
    //        if (attch != null)
    //        {
    //            AttachedFileId = attch.AttachedFileId;
    //            FileId = attch.FileId;
    //            AttachFileType = attch.Type;
    //            Url = $"{configure["CustomSettings:FileManager:Url"]}/File/DownloadFile?fileId={attch.FileId}&fileType={attch.FileType}";
    //        }
    //    }

    //    public bool HasAttch => !string.IsNullOrWhiteSpace(Url);

    //    public string AttchName { get; set; }

    //    public string UploaderName { get; set; }

    //    public int AttachedFileId { get; set; }

    //    public Guid FileId { get; set; }

    //    public string Url { get; set; }

    //    public string FileName { get; set; }

    //    public AttachFileType AttachFileType { get; set; }

    //    public string Accept { get; set; } = "image/*";
    //}
}
