using Elk.Core;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace InsBrokers.CrossCutting
{
    public static class HttpFileTools
    {
        public static string GetPath(string fileNameWithExtension, string root = "~",
            bool includeYearInPath = false, bool includeMonthInPath = false,
            bool includeDayInPath = false, string objectId = null,
            string urlPrefix = null, string fileNamePrefix = null)
        {
            #region Create Directory Address
            var persianDate = PersianDateTime.Now;
            var path = string.Join("/", root);
            if (includeYearInPath) path += "/" + persianDate.Year;
            if (includeMonthInPath) path += "/" + persianDate.Month;
            if (includeDayInPath) path += "/" + persianDate.Day;
            path += (objectId == null ? string.Empty : ("/" + objectId));
            var directoryAddress = Path.Combine(Directory.GetCurrentDirectory(), urlPrefix ?? "", "wwwroot", path.Replace("/", "\\"));
            #endregion

            #region Create File Name
            var trustedFileName = WebUtility.HtmlEncode(fileNameWithExtension);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(trustedFileName);
            var fileName = fileNamePrefix != null ? fileNamePrefix + "_" : string.Empty;
            fileName += fileNameWithoutExtension + "_" + persianDate.Ticks.ToString() + Path.GetExtension(trustedFileName);
            #endregion

            if (!FileOperation.CreateDirectory(directoryAddress)) return null;
            return "wwwroot/" + path + "/" + fileName;
        }

        public static string Save(IFormFile file, string fullPath)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                if (File.Exists(fullPath)) return fullPath.Contains("wwwroot/") ? fullPath.Remove(0, 8) : fullPath;
            }

            return null;
        }

        public static string Save(byte[] fileBytes, string fullPath)
        {
            if (fileBytes != null && fileBytes.Length > 0)
            {
                var file = new FormFile(null, 0, fileBytes.Length, "", "");

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                if (File.Exists(fullPath)) return fullPath;
            }

            return null;
        }
    }
}