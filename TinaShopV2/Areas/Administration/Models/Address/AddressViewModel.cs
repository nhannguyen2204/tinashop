using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Address
{
    public class AddressViewModel : BaseViewModel
    {
        public AddressViewModel() : base() { }

        public AddressViewModel(IOwinContext owinContext)
            : base(owinContext)
        {

        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Address", ResourceType = typeof(Commons))]
        public string StoreAddress { get; set; }
    }
}