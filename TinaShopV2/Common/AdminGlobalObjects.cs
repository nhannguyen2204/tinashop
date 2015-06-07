using System.Web.Configuration;

namespace TinaShopV2.Common
{
    public class AdminGlobalObjects
    {
        public const int PageSize = 20;

        private static string contentRelativePath;
        public static string ContentRelativePath
        {
            get
            {
                if (string.IsNullOrEmpty(contentRelativePath))
                    contentRelativePath = WebConfigurationManager.AppSettings["admin_content_relative_path"] ?? "/Content/SmartAdminPackage/";

                return contentRelativePath;
            }
        }

        private static string hostUser;
        public static string HostUser
        {
            get
            {
                if (string.IsNullOrEmpty(hostUser))
                    hostUser = WebConfigurationManager.AppSettings["admin_host"] ?? string.Empty;

                return hostUser;
            }
        }

        private static string avatarDefaultPath;
        public static string AvatarDefaultPath
        {
            get
            {
                if (string.IsNullOrEmpty(avatarDefaultPath))
                {
                    avatarDefaultPath = WebConfigurationManager.AppSettings["admin_avatar_default"] ?? "/Content/SmartAdminPackage/img/avatars/male.png";
                }

                return avatarDefaultPath;
            }
        }

        private static string avatarMaleDefaultPath;
        public static string AvatarMaleDefaultPath
        {
            get
            {
                if (string.IsNullOrEmpty(avatarMaleDefaultPath))
                {
                    avatarMaleDefaultPath = WebConfigurationManager.AppSettings["admin_avatar_male_default"] ?? "/Content/SmartAdminPackage/img/avatars/male.png";
                }

                return avatarMaleDefaultPath;
            }
        }

        private static string avatarFemaleDefaultPath;
        public static string AvatarFemaleDefaultPath
        {
            get
            {
                if (string.IsNullOrEmpty(avatarFemaleDefaultPath))
                {
                    avatarFemaleDefaultPath = WebConfigurationManager.AppSettings["admin_avatar_female_default"] ?? "/Content/SmartAdminPackage/img/avatars/female.png";
                }

                return avatarFemaleDefaultPath;
            }
        }

        private static int mainMenuTypeId;
        public static int MainMenuTypeId
        {
            get
            {
                if (mainMenuTypeId == 0)
                {
                    string menuTypeId = WebConfigurationManager.AppSettings["admin_main_menu_type_id"] ?? string.Empty;
                    if (!string.IsNullOrEmpty(menuTypeId))
                        int.TryParse(menuTypeId, out mainMenuTypeId);
                }

                return mainMenuTypeId;
            }
        }
    }
}