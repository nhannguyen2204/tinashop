using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models;
using TinaShopV2.Areas.Administration.Models.Address;
using TinaShopV2.Areas.Administration.Models.Brand;
using TinaShopV2.Areas.Administration.Models.Category;
using TinaShopV2.Areas.Administration.Models.Color;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Areas.Administration.Models.MediaType;
using TinaShopV2.Areas.Administration.Models.MenuType;
using TinaShopV2.Areas.Administration.Models.Product;
using TinaShopV2.Areas.Administration.Models.TinaAction;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Areas.Administration.Models.User;
using TinaShopV2.Models;
using TinaShopV2.Models.Entity;

namespace TinaShopV2.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Administration_Categories_Edit",
                "Administration/Category/Edit/{CatCode}",
                new
                {
                    area = AreaName,
                    controller = "Category",
                    action = "Edit",
                    CatCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Categories_Details",
                "Administration/Category/Details/{CatCode}",
                new
                {
                    area = AreaName,
                    controller = "Category",
                    action = "Details",
                    CatCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Categories_Delete",
                "Administration/Category/Delete/{CatCode}",
                new
                {
                    area = AreaName,
                    controller = "Category",
                    action = "Delete",
                    CatCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Brands_Delete",
                "Administration/Brands/Delete/{BrandCode}",
                new
                {
                    area = AreaName,
                    controller = "Brands",
                    action = "Delete",
                    BrandCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Brands_Details",
                "Administration/Brands/Details/{BrandCode}",
                new
                {
                    area = AreaName,
                    controller = "Brands",
                    action = "Details",
                    BrandCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Brands_Edit",
                "Administration/Brands/Edit/{BrandCode}",
                new
                {
                    area = AreaName,
                    controller = "Brands",
                    action = "Edit",
                    BrandCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Medias_Index",
                "Administration/Medias/Index/{Page}/{TypeId}/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Medias",
                    action = "Index",
                    ProductCode = UrlParameter.Optional,
                    Page = 1,
                    TypeId = 0,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Products_Details",
                "Administration/Products/Details/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Details",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Products_Delete",
                "Administration/Products/Delete/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Delete",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Products_Edit",
                "Administration/Products/Edit/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Edit",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Products_Create",
                "Administration/Products/Create/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Create",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Products_Find",
                "Administration/Products/Find/{ProductCode}",
                new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Find",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                name: "Administration_Products_Index",
                url: "Administration/Products/Index/{Page}/{IsPublish}/{IsDeleted}/{CanSale}/{BrandCode}/{ProductCode}",
                defaults: new
                {
                    area = AreaName,
                    controller = "Products",
                    action = "Index",
                    Page = 1,
                    IsPublish = 0,
                    IsDeleted = 0,
                    CanSale = 0,
                    PageTotal = 0,
                    Total = 0,
                    BrandCode = "all",
                    ProductCode = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
                //,constraints: new { IsPublish = new TinaShopV2.Common.Helpers.EnumConstraint("TinaShopV2.Areas.Administration.Models.Product.PublishStatus") }
            );

            context.MapRoute(
                "Administration_TinaMenus_Create",
                "Administration/TinaMenus/Create/{MenuTypeId}",
                new
                {
                    area = AreaName,
                    controller = "TinaMenus",
                    action = "Create",
                    MenuTypeId = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_TinaMenus_Index",
                "Administration/TinaMenus/{MenuTypeId}",
                new
                {
                    area = AreaName,
                    controller = "TinaMenus",
                    action = "Index",
                    MenuTypeId = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Colors_Delete",
                "Administration/Colors/Delete/{ColorKey}",
                new
                {
                    area = AreaName,
                    controller = "Colors",
                    action = "Delete",
                    ColorKey = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Colors_Details",
                "Administration/Colors/Details/{ColorKey}",
                new
                {
                    area = AreaName,
                    controller = "Colors",
                    action = "Details",
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Colors_Edit",
                "Administration/Colors/Edit/{ColorKey}",
                new
                {
                    area = AreaName,
                    controller = "Colors",
                    action = "Edit",
                    ColorKey = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Colors_Create",
                "Administration/Colors/Create/{ColorKey}",
                new
                {
                    area = AreaName,
                    controller = "Colors",
                    action = "Create",
                    ColorKey = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new
                {
                    area = AreaName,
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            context.MapRoute(
                "Administration_Localized",
                "{language}-{culture}/Administration/{controller}/{action}/{id}",
                new
                {
                    area = AreaName,
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }
            );

            AutoMapper.Mapper.CreateMap<IdentityRole, RoleViewModel>();
            AutoMapper.Mapper.CreateMap<RoleViewModel, IdentityRole>();

            AutoMapper.Mapper.CreateMap<ApplicationUser, UserViewModel>();
            AutoMapper.Mapper.CreateMap<UserViewModel, ApplicationUser>();

            AutoMapper.Mapper.CreateMap<ApplicationUser, CreateUserViewModel>();
            AutoMapper.Mapper.CreateMap<CreateUserViewModel, ApplicationUser>();

            AutoMapper.Mapper.CreateMap<ApplicationUser, EditUserViewModel>();
            AutoMapper.Mapper.CreateMap<EditUserViewModel, ApplicationUser>();

            AutoMapper.Mapper.CreateMap<TinaAction, TinaActionViewModel>();
            AutoMapper.Mapper.CreateMap<TinaActionViewModel, TinaAction>();

            AutoMapper.Mapper.CreateMap<MenuType, MenuTypeViewModel>();
            AutoMapper.Mapper.CreateMap<MenuTypeViewModel, MenuType>();

            AutoMapper.Mapper.CreateMap<TinaMenu, TinaMenuViewModel>();
            AutoMapper.Mapper.CreateMap<TinaMenuViewModel, TinaMenu>();

            AutoMapper.Mapper.CreateMap<Color, ColorViewModel>();
            AutoMapper.Mapper.CreateMap<ColorViewModel, Color>();

            AutoMapper.Mapper.CreateMap<Address, AddressViewModel>();
            AutoMapper.Mapper.CreateMap<AddressViewModel, Address>();

            AutoMapper.Mapper.CreateMap<Brand, BrandViewModel>();
            AutoMapper.Mapper.CreateMap<BrandViewModel, Brand>();

            AutoMapper.Mapper.CreateMap<MediaType, MediaTypeViewModel>();
            AutoMapper.Mapper.CreateMap<MediaTypeViewModel, MediaType>();

            AutoMapper.Mapper.CreateMap<Media, MediaViewModel>();
            AutoMapper.Mapper.CreateMap<MediaViewModel, Media>();

            AutoMapper.Mapper.CreateMap<Product, ProductViewModel>();
            AutoMapper.Mapper.CreateMap<ProductViewModel, Product>();

            AutoMapper.Mapper.CreateMap<Category, CategoryViewModel>();
            AutoMapper.Mapper.CreateMap<CategoryViewModel, Category>();
        }
    }
}