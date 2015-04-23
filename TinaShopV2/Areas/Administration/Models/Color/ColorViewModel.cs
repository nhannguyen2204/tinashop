﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Color
{
    public class ColorViewModel : BaseViewModel
    {
        public string ColorKey { get; set; }

        public string ColorCode { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Name", ResourceType = typeof(Commons))]
        public string Name { get; set; }
    }
}