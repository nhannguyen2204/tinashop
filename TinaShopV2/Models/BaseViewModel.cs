using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace TinaShopV2.Models
{
    public class BaseViewModel
    {
        private string createdUserId;
        [Display(Name = "CreatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string CreatedUserId
        {
            get { return createdUserId; }
            set
            {
                createdUserId = value;
                if (!string.IsNullOrEmpty(createdUserId))
                    createdUser = ApplicationUserManager.Instance.FindById(CreatedUserId);
            }
        }

        private ApplicationUser createdUser;
        [Display(Name = "CreatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        public ApplicationUser CreatedUser
        {
            get
            {
                if (createdUser == null && !string.IsNullOrEmpty(CreatedUserId))
                    createdUser = ApplicationUserManager.Instance.FindById(CreatedUserId);

                return createdUser;
            }
        }

        private string updatedUserId;
        [Display(Name = "UpdatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string UpdatedUserId
        {
            get { return updatedUserId; }
            set
            {
                updatedUserId = value;
                if (!string.IsNullOrEmpty(updatedUserId))
                    updatedUser = ApplicationUserManager.Instance.FindById(CreatedUserId);
            }
        }

        private ApplicationUser updatedUser;
        [Display(Name = "UpdatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        public ApplicationUser UpdatedUser
        {
            get
            {
                if (updatedUser == null && !string.IsNullOrEmpty(CreatedUserId))
                    updatedUser = ApplicationUserManager.Instance.FindById(CreatedUserId);

                return updatedUser;
            }
        }

        [Display(Name = "CreatedDatetime", ResourceType = typeof(App_GlobalResources.Commons))]
        public System.DateTime CreatedDatetime { get; set; }

        [Display(Name = "UpdatedDatetime", ResourceType = typeof(App_GlobalResources.Commons))]
        public System.DateTime UpdatedDatetime { get; set; }

        public void SetInteractionUser(string userId, bool isCreation = false)
        {
            if (isCreation)
            {
                CreatedUserId = userId;
                CreatedDatetime = DateTime.Now;
            }
            UpdatedUserId = userId;
            UpdatedDatetime = DateTime.Now;
        }
    }
}