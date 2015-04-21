using System;
using System.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using System.Collections;
using TinaShopV2.App_GlobalResources;

namespace TinaShopV2.Common
{
    public static class Helpers
    {
        #region HttpPostedFileBase

        public static bool DeleteFile(string folderPath, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // check folder existing
                string fullFolderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(fullFolderPath))
                    return false;

                // check file name existing
                if (string.IsNullOrEmpty(fileName))
                    fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileName);
                string fullFilePath = Path.Combine(fullFolderPath, fileName);

                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                    return true;
                }
                return false;
            }
            return false;
        }

        public static string SaveFile(HttpPostedFileBase file, string folderPath, string fileName = null)
        {
            if (file != null && file.ContentLength > 0)
            {
                // check folder existing
                string fullFolderPath = HostingEnvironment.MapPath(folderPath);
                if (!Directory.Exists(fullFolderPath))
                    return string.Empty;

                // check file name existing
                if (string.IsNullOrEmpty(fileName))
                    fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                string fullFilePath = Path.Combine(fullFolderPath, fileName);
                int copyIndex = 1;
                while (File.Exists(fullFilePath))
                {
                    copyIndex++;
                    fileName = string.Format("{0} ({1})", fileName, copyIndex);
                }

                // save file
                file.SaveAs(fullFilePath);
                return fileName;
            }
            return string.Empty;
        }

        #endregion

        #region ResourceManager

        static System.Resources.ResourceManager rmCommons;
        public static string GetResxNameByValue_Commons(string key)
        {
            if (rmCommons == null)
                rmCommons = new System.Resources.ResourceManager(typeof(Commons));

            return rmCommons.GetString(key);
        }

        static System.Resources.ResourceManager rmErrors;
        public static string GetResxNameByValue_Errors(string key)
        {
            if (rmErrors == null)
                rmErrors = new System.Resources.ResourceManager(typeof(Errors));

            return rmErrors.GetString(key);
        }

        #endregion

    }
}