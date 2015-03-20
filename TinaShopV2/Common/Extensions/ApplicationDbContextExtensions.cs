using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models;
using TinaShopV2.Areas.Administration.Models.Address;
using TinaShopV2.Areas.Administration.Models.Brand;
using TinaShopV2.Areas.Administration.Models.Color;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Areas.Administration.Models.MediaType;
using TinaShopV2.Areas.Administration.Models.MenuType;
using TinaShopV2.Areas.Administration.Models.Product;
using TinaShopV2.Areas.Administration.Models.TinaAction;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Models;
using TinaShopV2.Models.Entity;

namespace TinaShopV2.Common.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        #region Product

        public static IEnumerable<ProductViewModel> GetAllProductViewModels(this ApplicationDbContext context)
        {
            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            var products = context.Products.AsEnumerable();
            AutoMapper.Mapper.Map(products, model);
            return model;
        }

        public static IEnumerable<ProductViewModel> FindProductViewModelById(this ApplicationDbContext context, string productCode)
        {
            if (context == null || string.IsNullOrEmpty(productCode))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var products = context.Products.Where(m => !string.IsNullOrEmpty(m.ProductCode) && m.ProductCode.Contains(productCode)).OrderBy(m => m.ProductCode);

            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            AutoMapper.Mapper.Map(products, model);

            return model;
        }

        public static ProductViewModel GetProductViewModelById(this ApplicationDbContext context, string productCode)
        {
            if (context == null || string.IsNullOrEmpty(productCode))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var product = context.Products.Find(productCode.ToUpper());
            if (product == null)
                throw new HttpException(404, "ContentNotFound");

            ProductViewModel model = new ProductViewModel();
            AutoMapper.Mapper.Map(product, model);

            return model;
        }

        public static void CreateProductByViewModel(this ApplicationDbContext context, ProductViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var existingProduct = context.Products.Find(model.ProductCode);
                if (existingProduct != null)
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.ProductCode));

                model.ProductCode = model.ProductCode.ToUpper();

                var product = new Product();
                AutoMapper.Mapper.Map(model, product);

                context.Products.Add(product);
                context.Entry<Product>(product).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditProductByViewModel(this ApplicationDbContext context, ProductViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (string.IsNullOrEmpty(model.ProductCode))
                    throw new HttpException(404, "ContentNotFound");
                else
                {
                    var product = context.Products.Find(model.ProductCode.ToUpper());
                    if (product == null)
                        throw new HttpException(404, "ContentNotFound");

                    model.CreatedDatetime = product.CreatedDatetime;
                    model.CreatedUserId = product.CreatedUserId;

                    AutoMapper.Mapper.Map(model, product);

                    context.Entry<Product>(product).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteProductById(this ApplicationDbContext context, string productCode)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var product = context.Products.Find(productCode);
            if (product == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                context.Products.Remove(product);
                context.Entry<Product>(product).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Media

        public static IEnumerable<MediaViewModel> GetAllMediaViewModels(this ApplicationDbContext context)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            List<MediaViewModel> models = new List<MediaViewModel>();
            var medias = context.Medias.AsEnumerable();
            AutoMapper.Mapper.Map(medias, models);
            return models;
        }

        public static IEnumerable<MediaViewModel> GetMediaViewModels(this ApplicationDbContext context,out int pageToal, int page = 0, int? typeId = null, string productCode = null)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            List<MediaViewModel> models = new List<MediaViewModel>();
            var medias = context.Medias.AsEnumerable();
            if (typeId != null && typeId != 0)
                medias = medias.Where(m => m.TypeId == typeId);

            if (!string.IsNullOrEmpty(productCode))
                medias = medias.Where(m => m.ProductCode == productCode);

            medias = medias.OrderByDescending(m => m.UpdatedDatetime);

            pageToal = medias.Count() / AdminGlobalObjects.PageSize + (medias.Count() % AdminGlobalObjects.PageSize > 0 ? 1 : 0);

            if (page != 0)
                medias = medias.Skip((page - 1) * AdminGlobalObjects.PageSize).Take(AdminGlobalObjects.PageSize);

            if (page > 1 && medias.Count() == 0)
                throw new HttpException(404, "ContentNotFound");

            AutoMapper.Mapper.Map(medias, models);
            return models;
        }

        public static MediaViewModel GetMediaViewModelById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var media = context.Medias.Find(id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            MediaViewModel model = new MediaViewModel();
            AutoMapper.Mapper.Map(media, model);

            return model;
        }

        public static void CreateMediaByViewModel(this ApplicationDbContext context, MediaViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                model.FilePath = Helpers.SaveFile(model.FileUploader, GlobalObjects.MediaImageFolderPath);
                model.ThumbPath = Helpers.SaveFile(model.ThumbUploader, GlobalObjects.MediaImageFolderPath);

                if (model.TypeId != GlobalObjects.MediaType_ProductImage_Id)
                    model.ProductCode = string.Empty;

                var media = new Media();
                AutoMapper.Mapper.Map(model, media);

                context.Medias.Add(media);
                context.Entry<Media>(media).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMediaByViewModel(this ApplicationDbContext context, MediaViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var media = context.Medias.Find(model.Id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                model.CreatedDatetime = media.CreatedDatetime;
                model.CreatedUserId = media.CreatedUserId;

                if (model.FileUploader != null && model.FileUploader.ContentLength > 0)
                {
                    // Delete old file
                    Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, model.FilePath);

                    // save images.
                    model.FilePath = Helpers.SaveFile(model.FileUploader, GlobalObjects.MediaImageFolderPath);
                }

                if (model.ThumbUploader != null && model.ThumbUploader.ContentLength > 0)
                {
                    // Delete old file
                    Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, model.ThumbPath);

                    // save images.
                    model.ThumbPath = Helpers.SaveFile(model.ThumbUploader, GlobalObjects.MediaImageFolderPath);
                }

                if (model.TypeId != GlobalObjects.MediaType_ProductImage_Id)
                    model.ProductCode = string.Empty;

                AutoMapper.Mapper.Map(model, media);
                context.Entry<Media>(media).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMediaById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var media = context.Medias.Find(id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                // remove images
                Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, media.FilePath);
                Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, media.ThumbPath);

                context.Medias.Remove(media);
                context.Entry<Media>(media).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region MediaType

        public static IEnumerable<MediaTypeViewModel> GetAllMediaTypeViewModels(this ApplicationDbContext context)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            List<MediaTypeViewModel> models = new List<MediaTypeViewModel>();
            var mediaTypes = context.MediaTypes.AsEnumerable();
            AutoMapper.Mapper.Map(mediaTypes, models);
            return models;
        }

        public static MediaTypeViewModel GetMediaTypeViewModelById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var mediaType = context.MediaTypes.Find(id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            MediaTypeViewModel model = new MediaTypeViewModel();
            AutoMapper.Mapper.Map(mediaType, model);

            return model;
        }

        public static void CreateMediaTypeByViewModel(this ApplicationDbContext context, MediaTypeViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            if (context.MediaTypes.Any(m => m.Name == model.Name))
                throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

            try
            {
                MediaType newMediaType = new MediaType();
                AutoMapper.Mapper.Map(model, newMediaType);

                context.MediaTypes.Add(newMediaType);
                context.Entry<MediaType>(newMediaType).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMediaTypeByViewModel(this ApplicationDbContext context, MediaTypeViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var mediaType = context.MediaTypes.Find(model.Id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            if (context.MediaTypes.Any(m => m.Name == model.Name && m.Id != model.Id))
                throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

            try
            {
                model.CreatedDatetime = mediaType.CreatedDatetime;
                model.CreatedUserId = mediaType.CreatedUserId;

                AutoMapper.Mapper.Map(model, mediaType);
                context.Entry<MediaType>(mediaType).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMediaTypeByViewModel(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var mediaType = context.MediaTypes.Find(id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                context.MediaTypes.Remove(mediaType);
                context.Entry<MediaType>(mediaType).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Brand

        public static IEnumerable<BrandViewModel> GetAllBrandViewModels(this ApplicationDbContext context)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            List<BrandViewModel> models = new List<BrandViewModel>();
            var brands = context.Brands.AsEnumerable();
            AutoMapper.Mapper.Map(brands, models);
            return models;
        }

        public static BrandViewModel GetBrandViewModelById(this ApplicationDbContext context, string brandCode)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            BrandViewModel model = new BrandViewModel();
            var brand = context.Brands.Find(brandCode);
            if (brand == null)
                throw new HttpException(404, "ContentNotFound");
            AutoMapper.Mapper.Map(brand, model);
            return model;
        }

        public static void CreateBrandByViewModel(this ApplicationDbContext context, BrandViewModel model)
        {
            if (context == null && model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (context.Brands.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Brand newBrand = new Brand();
                AutoMapper.Mapper.Map(model, newBrand);
                context.Brands.Add(newBrand);
                context.Entry<Brand>(newBrand).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditBrandByViewModel(this ApplicationDbContext context, BrandViewModel model)
        {
            if (context == null && model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var brand = context.Brands.Find(model.BrandCode);
                if (brand == null)
                    throw new HttpException(404, "ContentNotFound");

                if (context.Brands.Any(m => m.Name == model.Name && m.BrandCode != model.BrandCode))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = brand.CreatedDatetime;
                model.CreatedUserId = brand.CreatedUserId;

                AutoMapper.Mapper.Map(model, brand);
                context.Entry<Brand>(brand).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteBrandById(this ApplicationDbContext context, string brandCode)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var brand = context.Brands.Find(brandCode);
                if (brand == null)
                    throw new HttpException(404, "ContentNotFound");

                context.Brands.Remove(brand);
                context.Entry<Brand>(brand).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Address

        public static IEnumerable<AddressViewModel> GetAllAddressViewModels(this ApplicationDbContext context)
        {
            List<AddressViewModel> models = new List<AddressViewModel>();
            var addresses = context.Addresses.AsEnumerable();
            AutoMapper.Mapper.Map(addresses, models);
            return models;
        }

        public static AddressViewModel GetAddressViewModelById(this ApplicationDbContext context, int id)
        {
            var address = context.Addresses.Find(id);
            if (address == null)
                throw new HttpException(404, "ContentNotFound");

            AddressViewModel model = new AddressViewModel();
            AutoMapper.Mapper.Map(address, model);

            return model;
        }

        public static void CreateAddressByViewModel(this ApplicationDbContext context, AddressViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (context.Addresses.Any(m => m.StoreAddress == model.StoreAddress))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Address address = new Address();
                AutoMapper.Mapper.Map(model, address);
                context.Addresses.Add(address);
                context.Entry<Address>(address).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditAddressByViewModel(this ApplicationDbContext context, AddressViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var address = context.Addresses.Find(model.Id);
                if (address == null)
                    throw new HttpException(404, "ContentNotFound");

                if (context.Addresses.Any(m => m.StoreAddress == model.StoreAddress && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = address.CreatedDatetime;
                model.CreatedUserId = address.CreatedUserId;

                AutoMapper.Mapper.Map(model, address);
                context.Entry<Address>(address).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteAddressById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var address = context.Addresses.Find(id);
                if (address == null)
                    throw new HttpException(404, "ContentNotFound");

                context.Addresses.Remove(address);
                context.Entry<Address>(address).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Color

        public static IEnumerable<ColorViewModel> GetAllColorViewModels(this ApplicationDbContext context)
        {
            List<ColorViewModel> models = new List<ColorViewModel>();
            var colors = context.Colors.AsEnumerable();
            AutoMapper.Mapper.Map(colors, models);
            return models;
        }

        public static ColorViewModel GetColorById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var color = context.Colors.Find(id);
                if (color == null)
                    throw new HttpException(404, "ContentNotFound");

                ColorViewModel model = new ColorViewModel();
                AutoMapper.Mapper.Map(color, model);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateColorByViewModel(this ApplicationDbContext context, ColorViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (context.Colors.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Color newColor = new Color();
                AutoMapper.Mapper.Map(model, newColor);
                context.Colors.Add(newColor);
                context.Entry<Color>(newColor).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditColorByViewModel(this ApplicationDbContext context, ColorViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var color = context.Colors.Find(model.Id);
                if (color == null)
                    throw new HttpException(404, "ContentNotFound");

                if (context.Colors.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = color.CreatedDatetime;
                model.CreatedUserId = color.CreatedUserId;

                AutoMapper.Mapper.Map(model, color);
                context.Entry<Color>(color).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteColorById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var color = context.Colors.Find(id);
                if (color == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                context.Colors.Remove(color);
                context.Entry<Color>(color).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Roles

        public static IEnumerable<RoleViewModel> GetAllRoleViewModel(this ApplicationDbContext context)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            return context.Roles.Select(m => new RoleViewModel() { Id = m.Id, Name = m.Name });
        }

        public static void CreateRoleByViewModel(this ApplicationDbContext context, RoleViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (context.Roles.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                IdentityRole newRole = new IdentityRole(model.Name);
                context.Roles.Add(newRole);
                context.Entry<IdentityRole>(newRole).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditRoleByViewModel(this ApplicationDbContext context, RoleViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var role = context.Roles.Find(model.Id);
                if (role == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (context.Roles.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                context.Entry<IdentityRole>(role).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteRoleByViewModel(this ApplicationDbContext context, string id)
        {
            if (context == null || string.IsNullOrEmpty(id))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var role = context.Roles.Find(id);
                if (role == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (context.TinaAuthorizes.Any(m => m.RoleId == id))
                    throw new Exception(App_GlobalResources.Errors.NotDelete_HaveChild);

                context.Roles.Remove(role);
                context.Entry<IdentityRole>(role).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TinaActions

        public static IEnumerable<TinaActionViewModel> GetAllTinaActionViewModel(this ApplicationDbContext context)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            IEnumerable<TinaActionViewModel> result = new List<TinaActionViewModel>();
            AutoMapper.Mapper.Map(context.TinaActions, result);
            return result;
        }

        public static void CreateTinaActionByViewModel(this ApplicationDbContext context, TinaActionViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var existingItem = context.TinaActions.FirstOrDefault(m => m.Name == model.Name);
                if (existingItem != null)
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                if (context.TinaActions.Any(m => m.Controller.ToLower() == model.Controller.ToLower() && m.Action.ToLower() == model.Action.ToLower() && (m.Area ?? string.Empty).ToLower() == model.Area.ToLower()))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, "Action"));

                TinaAction newAction = new TinaAction();
                AutoMapper.Mapper.Map(model, newAction);
                newAction = context.TinaActions.Add(newAction);
                context.Entry<TinaAction>(newAction).State = EntityState.Added;
                context.SaveChanges();

                if (model.RoleIds != null && model.RoleIds.Count() > 0)
                {
                    foreach (var item in model.RoleIds)
                    {
                        TinaAuthorize newAuthorize = new TinaAuthorize() { ActionId = newAction.Id, RoleId = item };
                        context.TinaAuthorizes.Add(newAuthorize);
                        context.Entry<TinaAuthorize>(newAuthorize).State = EntityState.Added;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditTinaActionByViewModel(this ApplicationDbContext context, TinaActionViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                TinaAction tinaAction = context.TinaActions.Find(model.Id);
                if (tinaAction == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (context.TinaActions.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                if (context.TinaActions.Any(m => m.Controller.ToLower() == model.Controller.ToLower() && m.Action.ToLower() == model.Action.ToLower() && (m.Area ?? string.Empty).ToLower() == model.Area.ToLower() && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, "Action"));

                model.CreatedUserId = tinaAction.CreatedUserId;
                model.CreatedDatetime = tinaAction.CreatedDatetime;

                AutoMapper.Mapper.Map(model, tinaAction);

                context.Entry<TinaAction>(tinaAction).State = EntityState.Modified;
                context.SaveChanges();

                var authorizes = context.TinaAuthorizes.Where(m => m.ActionId == model.Id).ToList();
                foreach (var item in authorizes)
                {
                    context.TinaAuthorizes.Remove(item);
                    context.Entry<TinaAuthorize>(item).State = EntityState.Deleted;
                    context.SaveChanges();
                }

                if (model.RoleIds != null && model.RoleIds.Count() > 0)
                {
                    foreach (var item in model.RoleIds)
                    {
                        TinaAuthorize newAuthorize = new TinaAuthorize() { ActionId = model.Id, RoleId = item };
                        context.TinaAuthorizes.Add(newAuthorize);
                        context.Entry<TinaAuthorize>(newAuthorize).State = EntityState.Added;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeleteTinaActionByViewModel(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                TinaAction tinaAction = context.TinaActions.Find(id);

                if (tinaAction == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                context.TinaActions.Remove(tinaAction);
                context.Entry<TinaAction>(tinaAction).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TinaAuthorize

        #endregion

        #region MenuType

        public static IEnumerable<MenuTypeViewModel> GetAllMenuTypeViewModel(this ApplicationDbContext context)
        {
            IEnumerable<MenuTypeViewModel> result = new List<MenuTypeViewModel>();
            AutoMapper.Mapper.Map(context.MenuTypes, result);
            return result;
        }

        public static void CreateMenuTypeByViewModel(this ApplicationDbContext context, MenuTypeViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (context.MenuTypes.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                MenuType newMenuType = new MenuType();
                AutoMapper.Mapper.Map(model, newMenuType);
                context.MenuTypes.Add(newMenuType);
                context.Entry<MenuType>(newMenuType).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMenuTypeByViewModel(this ApplicationDbContext context, MenuTypeViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                MenuType menuType = context.MenuTypes.Find(model.Id);
                if (menuType == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                // Check 'Name' existing
                if (context.MenuTypes.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedUserId = menuType.CreatedUserId;
                model.CreatedDatetime = menuType.CreatedDatetime;

                AutoMapper.Mapper.Map(model, menuType);
                context.Entry<MenuType>(menuType).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMenuTypeByViewModel(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                MenuType menuType = context.MenuTypes.Find(id);

                if (menuType == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                context.MenuTypes.Remove(menuType);
                context.Entry<MenuType>(menuType).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TinaMenu

        public static IEnumerable<TinaMenuViewModel> GetTinaMenuViewModelByTypeAndParent(this ApplicationDbContext context, int menuTypeId, int? parentId, bool? isHidden = null)
        {
            IEnumerable<TinaMenu> result;
            if (isHidden == null)
                result = context.TinaMenus.Where(m => m.MenuTypeId == menuTypeId && m.ParentId == parentId).OrderBy(m => m.OrderNumber);
            else
                result = context.TinaMenus.Where(m => m.MenuTypeId == menuTypeId && m.ParentId == parentId && m.IsHidden == isHidden.Value).OrderBy(m => m.OrderNumber);

            IEnumerable<TinaMenuViewModel> model = new List<TinaMenuViewModel>();
            AutoMapper.Mapper.Map(result, model);
            return model ?? new List<TinaMenuViewModel>();
        }

        public static void CreateTinaMenuByViewModel(this ApplicationDbContext context, TinaMenuViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (!context.MenuTypes.Any(m => m.Id == model.MenuTypeId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.MenuTypeId)));

                if (model.ActionId != null && !context.TinaActions.Any(m => m.Id == model.ActionId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ActionId)));

                if (model.ParentId != null && !context.TinaActions.Any(m => m.Id == model.ParentId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ParentId)));

                if (model.ParentId != null)
                {
                    bool parentIsOk = true;
                    var parent = context.TinaMenus.Find(model.ParentId.Value);
                    while (parent != null && (parentIsOk = !parent.IsHidden))
                    {
                        if (parent.ParentId == null)
                            break;
                        else
                            parent = context.TinaMenus.Find(parent.ParentId.Value);
                    }

                    if (!parentIsOk)
                        throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.ParentId)));
                }

                TinaMenu newMenu = new TinaMenu();
                AutoMapper.Mapper.Map(model, newMenu);
                context.TinaMenus.Add(newMenu);
                context.Entry<TinaMenu>(newMenu).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditTinaMenuByViewModel(this ApplicationDbContext context, TinaMenuViewModel model)
        {
            if (context == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                if (!context.MenuTypes.Any(m => m.Id == model.MenuTypeId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.MenuTypeId)));

                if (model.ActionId != null && !context.TinaActions.Any(m => m.Id == model.ActionId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ActionId)));

                if (model.ParentId != null && !context.TinaActions.Any(m => m.Id == model.ParentId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ParentId)));

                var tinaMenu = context.TinaMenus.Find(model.Id);
                if (tinaMenu == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (tinaMenu.MenuTypeId != model.MenuTypeId)
                    throw new Exception(App_GlobalResources.Errors.NotChangeMenuType);

                if (model.ParentId != null)
                {
                    bool parentIsOk = true;
                    var parent = context.TinaMenus.Find(model.ParentId.Value);
                    while (parent != null && (parentIsOk = !parent.IsHidden))
                    {
                        if (parent.ParentId == null)
                            break;
                        else
                            parent = context.TinaMenus.Find(parent.ParentId.Value);
                    }

                    if (!parentIsOk)
                        throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.ParentId)));
                }

                model.CreatedDatetime = tinaMenu.CreatedDatetime;
                model.CreatedUserId = tinaMenu.CreatedUserId;

                AutoMapper.Mapper.Map(model, tinaMenu);
                context.Entry<TinaMenu>(tinaMenu).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteTinaMenuById(this ApplicationDbContext context, int id)
        {
            if (context == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            try
            {
                var model = context.TinaMenus.Find(id);
                if (model == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                context.TinaMenus.Remove(model);
                context.Entry<TinaMenu>(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Custom Extensions

        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {

            Type type = typeof(TModel);

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);

            // First look into attributes on a type and it's parents
            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                }
            }
            return (attr != null) ? attr.Name : String.Empty;
        }

        #endregion
    }
}