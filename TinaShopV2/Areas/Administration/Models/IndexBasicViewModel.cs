using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;

namespace TinaShopV2.Areas.Administration.Models
{
    public class IndexBasicViewModel
    {
        private int page = 1;
        [Range(1, int.MaxValue)]
        [Display(Name = "Page", ResourceType = typeof(Commons))]
        public int Page
        {
            get { return page; }
            set { page = value; }
        }
    }
}