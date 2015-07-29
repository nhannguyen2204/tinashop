using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.ComponentModel.DataAnnotations;

namespace TinaShopV2.Models
{
    public class BaseOwinContext
    {
        protected ApplicationUserManager _userManagerService;
        protected ApplicationDbContext _dbContextService;
        protected IOwinContext _owinContext;


        public BaseOwinContext()
        {

        }

        public BaseOwinContext(IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _userManagerService = owinContext.GetUserManager<ApplicationUserManager>();
            _dbContextService = owinContext.Get<ApplicationDbContext>();
        }

        public void SetOwinContext(IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _userManagerService = owinContext.GetUserManager<ApplicationUserManager>();
            _dbContextService = owinContext.Get<ApplicationDbContext>();
        }
    }

    public class BaseViewModel : BaseOwinContext
    {
        public BaseViewModel() : base()
        {

        }

        public BaseViewModel(IOwinContext owinContext) : base(owinContext)
        {
            
        }

        private string createdUserId;
        [Display(Name = "CreatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string CreatedUserId
        {
            get { return createdUserId; }
            set
            {
                createdUserId = value;
                //if (!string.IsNullOrEmpty(createdUserId))
                //    createdUser = _userManagerService.FindById(CreatedUserId);
            }
        }

        private ApplicationUser createdUser;
        [Display(Name = "CreatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        public ApplicationUser CreatedUser
        {
            get
            {
                //if (createdUser == null && !string.IsNullOrEmpty(CreatedUserId))
                createdUser = _userManagerService.FindById(CreatedUserId);

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
                //if (!string.IsNullOrEmpty(updatedUserId))
                //    updatedUser = _userManagerService.FindById(CreatedUserId);
            }
        }

        private ApplicationUser updatedUser;
        [Display(Name = "UpdatedUser", ResourceType = typeof(App_GlobalResources.Commons))]
        public ApplicationUser UpdatedUser
        {
            get
            {
                //if (updatedUser == null && !string.IsNullOrEmpty(CreatedUserId))
                updatedUser = _userManagerService.FindById(CreatedUserId);

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