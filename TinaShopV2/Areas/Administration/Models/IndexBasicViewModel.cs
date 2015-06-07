using Microsoft.Owin;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models
{
    public class IndexBasicViewModel : BaseViewModel
    {
        public IndexBasicViewModel() : base() { }

        public IndexBasicViewModel(IOwinContext owinContext)
            : base(owinContext)
        {

        }

       /// <summary>
       /// Current Page
       /// </summary>
        private int page = 1;
        [Range(1, int.MaxValue)]
        [Display(Name = "Page", ResourceType = typeof(Commons))]
        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// Total Of Page
        /// </summary>
        private int pageTotal = 0;
        public int PageTotal
        {
            get { return pageTotal; }
            set { pageTotal = value; }
        }

        /// <summary>
        /// Total Of Items
        /// </summary>
        private int total = 0;
        public int Total
        {
            get { return total; }
            set { total = value; }
        }

    }
}