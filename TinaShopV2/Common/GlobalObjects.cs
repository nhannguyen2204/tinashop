using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web.Configuration;
using TinaShopV2.Models;

namespace TinaShopV2.Common
{
    public static class GlobalObjects
    {
        public const string MainDomainProtocol = "http://tina2012.vn";
        public const string MainDomain = "tina2012.vn";
        public const string SuccesMessageKey = "success_messages";
        public const string ErrorMessageKey = "error_messages";
        public const string ErrorMessFormat = "[ERROR] message : {0}";
        public const string TitleFormat = "{0} - {1}";
        public const string SiteTitle = "Tina Shop";
        public const string HrefBlankPath = "jsvascript:void(0);";

        private static string mediaImageFolderPath;
        public static string MediaImageFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(mediaImageFolderPath))
                    mediaImageFolderPath = WebConfigurationManager.AppSettings["image_folder_path"] ?? "/MediaFiles/Images";

                return mediaImageFolderPath;
            }
        }

        private static string mediaVideoFolderPath;
        public static string MediaVideoFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(mediaVideoFolderPath))
                    mediaVideoFolderPath = WebConfigurationManager.AppSettings["video_folder_path"] ?? "/MediaFiles/Videos";

                return mediaVideoFolderPath;
            }
        }

        private static string mediaOtherFolderPath;
        public static string MediaOtherFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(mediaOtherFolderPath))
                    mediaOtherFolderPath = WebConfigurationManager.AppSettings["other_media_folder_path"] ?? "/MediaFiles/Others";

                return mediaOtherFolderPath;
            }
        }

        private static int mediaType_ProductImage_Id;
        public static int MediaType_ProductImage_Id
        {
            get
            {
                if (mediaType_ProductImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_productimage_id"] ?? "1";
                    if (!int.TryParse(id, out mediaType_ProductImage_Id))
                        mediaType_ProductImage_Id = 1;
                }

                return mediaType_ProductImage_Id;
            }
        }

        private static int mediaType_BrandImage_Id;
        public static int MediaType_BrandImage_Id
        {
            get
            {
                if (mediaType_BrandImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_brandimage_id"] ?? "5";
                    if (!int.TryParse(id, out mediaType_BrandImage_Id))
                        mediaType_BrandImage_Id = 5;
                }

                return mediaType_BrandImage_Id;
            }
        }

        private static int mediaType_CatalogImage_Id;
        public static int MediaType_CatalogImage_Id
        {
            get
            {
                if (mediaType_CatalogImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_catalogimage_id"] ?? "6";
                    if (!int.TryParse(id, out mediaType_CatalogImage_Id))
                        mediaType_CatalogImage_Id = 6;
                }

                return mediaType_CatalogImage_Id;
            }
        }

        private static int mediaType_CategoryImage_Id;
        public static int MediaType_CategoryImage_Id
        {
            get
            {
                if (mediaType_CategoryImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_categoryimage_id"] ?? "7";
                    if (!int.TryParse(id, out mediaType_CategoryImage_Id))
                        mediaType_CategoryImage_Id = 7;
                }

                return mediaType_CategoryImage_Id;
            }
        }

        private static int mediaType_SliderImage_Id;
        public static int MediaType_SliderImage_Id
        {
            get
            {
                if (mediaType_SliderImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_sliderimage_id"] ?? "2";
                    if (!int.TryParse(id, out mediaType_SliderImage_Id))
                        mediaType_SliderImage_Id = 2;
                }

                return mediaType_SliderImage_Id;
            }
        }

        private static int mediaType_StaticObjects_Id;
        public static int MediaType_StaticObjects_Id
        {
            get
            {
                if (mediaType_StaticObjects_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["mediatype_staticobjects_id"] ?? "4";
                    if (!int.TryParse(id, out mediaType_StaticObjects_Id))
                        mediaType_StaticObjects_Id = 4;
                }

                return mediaType_StaticObjects_Id;
            }
        }

        private static int media_NoImage_Id;
        public static int Media_NoImage_Id
        {
            get
            {
                if (media_NoImage_Id == 0)
                {
                    string id = WebConfigurationManager.AppSettings["media_noimage_id"] ?? "2";
                    if (!int.TryParse(id, out media_NoImage_Id))
                        media_NoImage_Id = 2;
                }

                return media_NoImage_Id;
            }
        }

        private static bool? isComingSoonMode;
        public static bool IsComingSoonMode
        {
            get
            {
                if (isComingSoonMode == null)
                {
                    string valueStr = WebConfigurationManager.AppSettings["IsComingSoonMode"] ?? "false";
                    bool boolValue = false;
                    if (!bool.TryParse(valueStr, out boolValue))
                        isComingSoonMode = false;

                    isComingSoonMode = boolValue;
                }

                return isComingSoonMode.Value;
            }
        }


        private static TinaShopV2.Models.Entity.Media media_NoImage;
        public static TinaShopV2.Models.Entity.Media Get_Media_NoImage(IOwinContext owinContext)
        {
            if (media_NoImage == null)
            {
                var dbContext = owinContext.Get<ApplicationDbContext>();
                media_NoImage = dbContext.Medias.Find(Media_NoImage_Id);
            }
            return media_NoImage;
        }

        private static string media_NoImage_Path = string.Empty;
        public static string Media_NoImage_Path
        {
            get
            {
                //if (string.IsNullOrEmpty(media_NoImage_Path) && Media_NoImage != null)
                //    media_NoImage_Path = Media_NoImage.FilePath;

                return media_NoImage_Path;
            }
        }

        private static string media_NoImageThumb_Path = string.Empty;
        public static string Media_NoImageThumb_Path
        {
            get
            {
                //if (string.IsNullOrEmpty(media_NoImageThumb_Path) && Media_NoImage != null)
                //    media_NoImageThumb_Path = Media_NoImage.ThumbPath;

                return media_NoImageThumb_Path;
            }
        }
    }
}