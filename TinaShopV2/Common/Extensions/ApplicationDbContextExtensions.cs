using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
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
using TinaShopV2.Areas.Administration.Models.Category;
using TinaShopV2.Areas.Administration.Models.Color;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Areas.Administration.Models.MediaType;
using TinaShopV2.Areas.Administration.Models.MenuType;
using TinaShopV2.Areas.Administration.Models.Product;
using TinaShopV2.Areas.Administration.Models.TinaAction;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Models;
using TinaShopV2.Models.Entity;
using TinaShopV2.Areas.Administration.Models.Slider;
using TinaShopV2.Areas.Administration.Models.Catalog;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Hosting;

namespace TinaShopV2.Common.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        #region Product FO

        public static ProductFOViewModel GetProductFOViewModelById(this IOwinContext owinContext, string productCode)
        {
            if (owinContext == null || string.IsNullOrEmpty(productCode))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var product = dbContext.Products.Find(productCode.ToUpper());
            if (product == null || product.IsPublish != true)
                throw new HttpException(404, "ContentNotFound");

            ProductFOViewModel model = new ProductFOViewModel(owinContext);
            AutoMapper.Mapper.Map(product, model);

            return model;
        }

        public static ProductFilterIndexViewModel GetProductFOViewModels(this IOwinContext owinContext, ref ProductFilterIndexViewModel model)
        {
            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<ProductFOViewModel> productsFO = new List<ProductFOViewModel>();
            var products = dbContext.Products.Where(m => m.IsPublish == true && m.IsDeleted == false).AsEnumerable();

            var fromPrice = model.FromPrice;
            var toPrice = model.ToPrice;
            products = products.Where(m => m.Price >= (fromPrice * 1000) && m.Price <= (toPrice * 1000));

            if (!string.IsNullOrEmpty(model.BrandCode) && model.BrandCode.ToLower() != GlobalObjects.DefaultAllBrandCode)
            {
                string brandCode = model.BrandCode;
                products = products.Where(m => m.BrandCode == brandCode);
            }

            if (!string.IsNullOrEmpty(model.CatCode) && model.CatCode.ToLower() != GlobalObjects.DefaultAllCatCode)
            {
                List<CategoryViewModel> catChilds = new List<CategoryViewModel>();
                AdminHelpers.GenerateCategories(ref catChilds, owinContext, model.CatCode);
                var childsCode = catChilds.Select(m => m.CatCode).ToList();
                childsCode.Add(model.CatCode);
                products = products.Where(m => childsCode.Contains(m.CatCode));
            }

            if (!string.IsNullOrEmpty(model.ColorKeys) && model.ColorKeys.ToLower() != GlobalObjects.DefaultAllColors)
            {
                var colorKeys = model.ColorKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var selectedProducts = dbContext.ProductDetails.Where(m => colorKeys.Contains(m.ColorKey)).Select(n => n.ProductCode).ToList();
                products = products.Where(m => selectedProducts.Contains(m.ProductCode));
            }

            if (model.IsOrderDatetimeDesc == 0)
            {
                if (model.IsOrderPriceDesc == 0)
                    products = products.OrderByDescending(m => m.UpdatedDatetime.Date).ThenByDescending(m => m.Price);
                else
                    products = products.OrderByDescending(m => m.UpdatedDatetime.Date).ThenBy(m => m.Price);
            }
            else
            {
                if (model.IsOrderPriceDesc == 0)
                    products = products.OrderBy(m => m.UpdatedDatetime.Date).ThenByDescending(m => m.Price);
                else
                    products = products.OrderBy(m => m.UpdatedDatetime.Date).ThenBy(m => m.Price);
            }

            model.SetOwinContext(owinContext);

            model.PageTotal = products.Count() / AdminGlobalObjects.PageSize + (products.Count() % AdminGlobalObjects.PageSize > 0 ? 1 : 0);
            model.Total = products.Count();

            if (model.Page != 0)
                products = products.Skip((model.Page - 1) * AdminGlobalObjects.PageSize).Take(AdminGlobalObjects.PageSize);

            if (model.Page > 1 && products.Count() == 0)
                throw new HttpException(404, "ContentNotFound");

            AutoMapper.Mapper.Map(products, productsFO);
            model.Products = productsFO;

            foreach (var item in model.Products)
            {
                item.SetOwinContext(owinContext);
            }

            return model;
        }

        #endregion

        #region Product

        public static IEnumerable<ProductViewModel> GetAllProductViewModels(this IOwinContext owinContext)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            var products = dbContext.Products.AsEnumerable();
            AutoMapper.Mapper.Map(products, model);
            return model;
        }

        public static void GetProductsByIndexViewModel(this IOwinContext owinContext, ref ProductIndexViewModel indexViewModel)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            // Init List
            List<ProductViewModel> models = new List<ProductViewModel>();
            var products = dbContext.Products.AsEnumerable();

            // Filter By Brand Code
            string brandCode = indexViewModel.BrandCode;
            if (!string.IsNullOrEmpty(brandCode) && brandCode.ToLower() != "all")
                products = products.Where(m => m.BrandCode == brandCode);

            // Filter By Category Code
            string catCode = indexViewModel.CatCode;
            if (!string.IsNullOrEmpty(catCode) && catCode.ToLower() != "all")
            {
                List<CategoryViewModel> catChilds = new List<CategoryViewModel>();
                AdminHelpers.GenerateCategories(ref catChilds, owinContext, catCode);
                var childsCode = catChilds.Select(m => m.CatCode).ToList();
                childsCode.Add(catCode);
                products = products.Where(m => childsCode.Contains(m.CatCode));
            }

            // Filter By Product Code
            string productCode = indexViewModel.ProductCode;
            if (!string.IsNullOrEmpty(productCode))
                products = products.Where(m => m.ProductCode.ToLower().Contains(productCode.ToLower()));

            // Filter By IsPublish
            if ((PublishStatus)indexViewModel.IsPublish != PublishStatus.Null)
            {
                bool isPublish = (PublishStatus)indexViewModel.IsPublish == PublishStatus.Published;
                products = products.Where(m => m.IsPublish == isPublish);
            }

            // Filter By IsDeleted
            if ((DeleteStatus)indexViewModel.IsDeleted != DeleteStatus.Null)
            {
                bool isDeleted = (DeleteStatus)indexViewModel.IsDeleted == DeleteStatus.Deleted;
                products = products.Where(m => m.IsDeleted == isDeleted);
            }

            // Filter By CanSale
            if ((SaleState)indexViewModel.CanSale != SaleState.Null)
            {
                bool canSale = (SaleState)indexViewModel.CanSale == SaleState.CanSale;
                products = products.Where(m => m.CanSale == canSale);
            }

            // Sort By UpdatedDatetime
            products = products.OrderByDescending(m => m.UpdatedDatetime);

            indexViewModel.PageTotal = products.Count() / AdminGlobalObjects.PageSize + (products.Count() % AdminGlobalObjects.PageSize > 0 ? 1 : 0);
            indexViewModel.Total = products.Count();

            if (indexViewModel.Page != 0)
                products = products.Skip((indexViewModel.Page - 1) * AdminGlobalObjects.PageSize).Take(AdminGlobalObjects.PageSize);

            if (indexViewModel.Page > 1 && products.Count() == 0)
                throw new HttpException(404, "ContentNotFound");

            indexViewModel.Products = AutoMapper.Mapper.Map(products, models);

            foreach (var item in indexViewModel.Products)
            {
                item.SetOwinContext(owinContext);
            }
        }

        public static IEnumerable<ProductViewModel> FindProductViewModelById(this IOwinContext owinContext, string productCode)
        {
            if (owinContext == null || string.IsNullOrEmpty(productCode))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var products = dbContext.Products.Where(m => !string.IsNullOrEmpty(m.ProductCode) && m.ProductCode.Contains(productCode)).OrderBy(m => m.ProductCode);

            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            AutoMapper.Mapper.Map(products, model);

            return model;
        }

        public static ProductViewModel GetProductViewModelById(this IOwinContext owinContext, string productCode)
        {
            if (owinContext == null || string.IsNullOrEmpty(productCode))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var product = dbContext.Products.Find(productCode.ToUpper());
            if (product == null)
                throw new HttpException(404, "ContentNotFound");

            ProductViewModel model = new ProductViewModel(owinContext);
            AutoMapper.Mapper.Map(product, model);

            return model;
        }

        public static void CreateProductByViewModel(this IOwinContext owinContext, ProductViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var existingProduct = dbContext.Products.Find(model.ProductCode);
                if (existingProduct != null)
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.ProductCode));

                model.ProductCode = model.ProductCode.ToUpper();

                var product = new Product();
                AutoMapper.Mapper.Map(model, product);

                dbContext.Products.Add(product);
                dbContext.Entry<Product>(product).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditProductByViewModel(this IOwinContext owinContext, ProductViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (string.IsNullOrEmpty(model.ProductCode))
                    throw new HttpException(404, "ContentNotFound");
                else
                {
                    var product = dbContext.Products.Find(model.ProductCode.ToUpper());
                    if (product == null)
                        throw new HttpException(404, "ContentNotFound");

                    model.CreatedDatetime = product.CreatedDatetime;
                    model.CreatedUserId = product.CreatedUserId;

                    AutoMapper.Mapper.Map(model, product);

                    dbContext.Entry<Product>(product).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteProductById(this IOwinContext owinContext, string productCode)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var product = dbContext.Products.Find(productCode);
            if (product == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                dbContext.Products.Remove(product);
                dbContext.Entry<Product>(product).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Media

        public static IEnumerable<MediaViewModel> GetAllMediaViewModels(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<MediaViewModel> models = new List<MediaViewModel>();
            var medias = dbContext.Medias.AsEnumerable();
            AutoMapper.Mapper.Map(medias, models);
            return models;
        }

        public static IEnumerable<MediaViewModel> FindMediaViewModelByName(this IOwinContext owinContext, string mediaName, int? typeId = null)
        {
            if (owinContext == null || string.IsNullOrEmpty(mediaName))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var medias = dbContext.Medias.Where(m => !string.IsNullOrEmpty(m.Name) && m.Name.Contains(mediaName));

            if (typeId != null)
                medias = medias.Where(m => m.TypeId == typeId.Value);

            medias = medias.OrderBy(m => m.Name);

            IEnumerable<MediaViewModel> model = new List<MediaViewModel>();
            AutoMapper.Mapper.Map(medias, model);

            return model;
        }

        public static void GetMediasByIndexViewModel(this IOwinContext owinContext, ref MediaIndexViewModel indexViewModel)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<MediaViewModel> models = new List<MediaViewModel>();
            var medias = dbContext.Medias.AsEnumerable();

            int? typeId = indexViewModel.TypeId;
            if (indexViewModel.TypeId != null && indexViewModel.TypeId != 0)
                medias = medias.Where(m => m.TypeId == typeId);

            string productCode = indexViewModel.ProductCode;
            if (!string.IsNullOrEmpty(indexViewModel.ProductCode))
                medias = medias.Where(m => m.ProductCode == productCode);

            medias = medias.OrderByDescending(m => m.UpdatedDatetime);

            indexViewModel.PageTotal = medias.Count() / AdminGlobalObjects.PageSize + (medias.Count() % AdminGlobalObjects.PageSize > 0 ? 1 : 0);

            if (indexViewModel.Page != 0)
                medias = medias.Skip((indexViewModel.Page - 1) * AdminGlobalObjects.PageSize).Take(AdminGlobalObjects.PageSize);

            if (indexViewModel.Page > 1 && medias.Count() == 0)
                throw new HttpException(404, "ContentNotFound");

            indexViewModel.Medias = AutoMapper.Mapper.Map(medias, models);

            foreach (var item in indexViewModel.Medias)
            {
                item.SetOwinContext(owinContext);
            }
        }

        public static MediaViewModel GetMediaViewModelById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var media = dbContext.Medias.Find(id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            MediaViewModel model = new MediaViewModel(owinContext);
            AutoMapper.Mapper.Map(media, model);

            return model;
        }

        public static void CreateMediaByViewModel(this IOwinContext owinContext, MediaViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Medias.Any(m => m.Name.ToLower() == model.Name))
                {
#warning Add message error
                    throw new Exception("Trùng tên");
                }

                model.FilePath = Helpers.SaveFile(model.FileUploader, GlobalObjects.MediaImageFolderPath);
                if (model.ThumbUploader != null && model.ThumbUploader.ContentLength > 0)
                    model.ThumbPath = Helpers.SaveFile(model.ThumbUploader, GlobalObjects.MediaImageFolderPath);
                else if (!string.IsNullOrEmpty(model.FilePath))
                {
                    var imagePath = HostingEnvironment.MapPath(Path.Combine(GlobalObjects.MediaImageFolderPath, model.FilePath));
                    var image = Image.FromFile(imagePath);

                    var newImage = Helpers.ScaleImage(image, 356, 390);

                    var folderPath = HostingEnvironment.MapPath(GlobalObjects.MediaImageFolderPath);
                    var newName = string.Format("{0}.png", Guid.NewGuid().ToString());
                    newImage.Save(Path.Combine(folderPath, newName), ImageFormat.Png);
                    model.ThumbPath = newName;
                }

                if (model.TypeId != GlobalObjects.MediaType_ProductImage_Id)
                    model.ProductCode = null;

                model.SetOwinContext(owinContext);

                var media = new TinaShopV2.Models.Entity.Media();
                AutoMapper.Mapper.Map(model, media);

                dbContext.Medias.Add(media);
                dbContext.Entry<Media>(media).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMediaByViewModel(this IOwinContext owinContext, MediaViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var media = dbContext.Medias.Find(model.Id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                if (dbContext.Medias.Any(m => m.Name.ToLower() == model.Name && m.Id != model.Id))
                {
#warning Add message error
                    throw new Exception("Trùng tên");
                }

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
                    model.ProductCode = null;

                AutoMapper.Mapper.Map(model, media);
                dbContext.Entry<Media>(media).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMediaById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var media = dbContext.Medias.Find(id);
            if (media == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                // remove images
                Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, media.FilePath);
                Helpers.DeleteFile(GlobalObjects.MediaImageFolderPath, media.ThumbPath);

                dbContext.Medias.Remove(media);
                dbContext.Entry<Media>(media).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region MediaType

        public static IEnumerable<MediaTypeViewModel> GetAllMediaTypeViewModels(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<MediaTypeViewModel> models = new List<MediaTypeViewModel>();
            var mediaTypes = dbContext.MediaTypes.AsEnumerable();
            AutoMapper.Mapper.Map(mediaTypes, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static MediaTypeViewModel GetMediaTypeViewModelById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var mediaType = dbContext.MediaTypes.Find(id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            MediaTypeViewModel model = new MediaTypeViewModel(owinContext);
            AutoMapper.Mapper.Map(mediaType, model);

            return model;
        }

        public static void CreateMediaTypeByViewModel(this IOwinContext owinContext, MediaTypeViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            if (dbContext.MediaTypes.Any(m => m.Name == model.Name))
                throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

            try
            {
                MediaType newMediaType = new MediaType();
                AutoMapper.Mapper.Map(model, newMediaType);

                dbContext.MediaTypes.Add(newMediaType);
                dbContext.Entry<MediaType>(newMediaType).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMediaTypeByViewModel(this IOwinContext owinContext, MediaTypeViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var mediaType = dbContext.MediaTypes.Find(model.Id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            if (dbContext.MediaTypes.Any(m => m.Name == model.Name && m.Id != model.Id))
                throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

            try
            {
                model.CreatedDatetime = mediaType.CreatedDatetime;
                model.CreatedUserId = mediaType.CreatedUserId;

                AutoMapper.Mapper.Map(model, mediaType);
                dbContext.Entry<MediaType>(mediaType).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMediaTypeByViewModel(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var mediaType = dbContext.MediaTypes.Find(id);
            if (mediaType == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                dbContext.MediaTypes.Remove(mediaType);
                dbContext.Entry<MediaType>(mediaType).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Brand

        public static IEnumerable<BrandViewModel> GetAllBrandViewModels(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<BrandViewModel> models = new List<BrandViewModel>();
            var brands = dbContext.Brands.AsEnumerable();
            AutoMapper.Mapper.Map(brands, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static BrandViewModel GetBrandViewModelById(this IOwinContext owinContext, string brandCode)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            BrandViewModel model = new BrandViewModel(owinContext);
            var brand = dbContext.Brands.Find(brandCode);
            if (brand == null)
                throw new HttpException(404, "ContentNotFound");
            AutoMapper.Mapper.Map(brand, model);
            return model;
        }

        public static void CreateBrandByViewModel(this IOwinContext owinContext, BrandViewModel model)
        {
            if (owinContext == null && model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Brands.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Brand newBrand = new Brand();
                AutoMapper.Mapper.Map(model, newBrand);
                dbContext.Brands.Add(newBrand);
                dbContext.Entry<Brand>(newBrand).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditBrandByViewModel(this IOwinContext owinContext, BrandViewModel model)
        {
            if (owinContext == null && model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var brand = dbContext.Brands.Find(model.BrandCode);
                if (brand == null)
                    throw new HttpException(404, "ContentNotFound");

                if (dbContext.Brands.Any(m => m.Name == model.Name && m.BrandCode != model.BrandCode))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = brand.CreatedDatetime;
                model.CreatedUserId = brand.CreatedUserId;

                AutoMapper.Mapper.Map(model, brand);
                dbContext.Entry<Brand>(brand).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteBrandById(this IOwinContext owinContext, string brandCode)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var brand = dbContext.Brands.Find(brandCode);
                if (brand == null)
                    throw new HttpException(404, "ContentNotFound");

                dbContext.Brands.Remove(brand);
                dbContext.Entry<Brand>(brand).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Category

        public static IEnumerable<CategoryViewModel> GetCatViewModelByParent(this IOwinContext owinContext, string parentCode = null, bool? isPublished = null)
        {
            IEnumerable<Category> result = null;

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            if (string.IsNullOrEmpty(parentCode))
                result = dbContext.Categories.Where(m => m.CatParentCode == null || m.CatParentCode == string.Empty).OrderBy(m => m.OrderNumber);
            else
                result = dbContext.Categories.Where(m => m.CatParentCode == parentCode).OrderBy(m => m.OrderNumber);

            if (isPublished != null)
                result = result.Where(m => m.IsPublish == isPublished);

            IEnumerable<CategoryViewModel> model = new List<CategoryViewModel>();
            AutoMapper.Mapper.Map(result, model);
            return model ?? new List<CategoryViewModel>();
        }

        public static IEnumerable<string> GetParentCodesByCategoryViewModel(this IOwinContext owinContext, CategoryViewModel model)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<string> parentCodes = new List<string>();

            Category parent = !string.IsNullOrEmpty(model.CatParentCode) ? dbContext.Categories.Find(model.CatParentCode) : null;
            while (parent != null)
            {
                parentCodes.Add(parent.CatCode);

                if (!string.IsNullOrEmpty(parent.CatParentCode))
                    parent = dbContext.Categories.Find(parent.CatParentCode);
                else
                    break;
            }

            return parentCodes;
        }

        public static CategoryViewModel GetCategoryViewModelByCatCode(this IOwinContext owinContext, string catCode)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            CategoryViewModel model = null;

            Category cat = dbContext.Categories.Find(catCode);
            if (cat != null)
            {
                model = new CategoryViewModel(owinContext);
                AutoMapper.Mapper.Map(cat, model);
            }

            return model;
        }

        public static void CreateCategoryByViewModel(this IOwinContext owinContext, CategoryViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Categories.Any(m => m.CatCode.ToLower().Trim() == model.CatCode.ToLower().Trim()))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, model.GetDisplayName(m => m.CatCode)));

                if (dbContext.Categories.Any(m => m.Name.ToLower().Trim() == model.Name.ToLower().Trim()))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, model.GetDisplayName(m => m.Name)));

                if (!string.IsNullOrEmpty(model.CatParentCode) && !dbContext.Categories.Any(m => m.CatCode == model.CatParentCode))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.CatParentCode)));

                var categories = owinContext.GetParentCodesByCategoryViewModel(model);
                if (categories.Count() > 0 && categories.Contains(model.CatCode))
                    throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.CatParentCode)));

                Category newCat = new Category();
                AutoMapper.Mapper.Map(model, newCat);

                dbContext.Categories.Add(newCat);
                dbContext.Entry<Category>(newCat).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditCategoryByViewModel(this IOwinContext owinContext, CategoryViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            Category category = dbContext.Categories.Find(model.CatCode);
            if (category == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                if (dbContext.Categories.Any(m => m.CatCode.ToLower().Trim() == model.CatCode.ToLower().Trim() && m.CatCode != model.CatCode))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, model.GetDisplayName(m => m.CatCode)));

                if (dbContext.Categories.Any(m => m.Name.ToLower().Trim() == model.Name.ToLower().Trim() && m.CatCode != model.CatCode))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, model.GetDisplayName(m => m.Name)));

                if (!string.IsNullOrEmpty(model.CatParentCode) && !dbContext.Categories.Any(m => m.CatCode == model.CatParentCode))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.CatParentCode)));

                var categories = owinContext.GetParentCodesByCategoryViewModel(model);
                if (categories.Count() > 0 && categories.Contains(model.CatCode))
                    throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.CatParentCode)));

                model.CreatedDatetime = category.CreatedDatetime;
                model.CreatedUserId = category.CreatedUserId;
                AutoMapper.Mapper.Map(model, category);

                dbContext.Entry<Category>(category).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteCategoryByCatCode(this IOwinContext owinContext, string catCode)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var cat = dbContext.Categories.Find(catCode);
                if (cat == null)
                    throw new HttpException(404, "ContentNotFound");

                dbContext.Categories.Remove(cat);
                dbContext.Entry<Category>(cat).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Address

        public static IEnumerable<AddressViewModel> GetAllAddressViewModels(this IOwinContext owinContext)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<AddressViewModel> models = new List<AddressViewModel>();
            var addresses = dbContext.Addresses.AsEnumerable();
            AutoMapper.Mapper.Map(addresses, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static AddressViewModel GetAddressViewModelById(this IOwinContext owinContext, int id)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            var address = dbContext.Addresses.Find(id);
            if (address == null)
                throw new HttpException(404, "ContentNotFound");

            AddressViewModel model = new AddressViewModel(owinContext);
            AutoMapper.Mapper.Map(address, model);

            return model;
        }

        public static void CreateAddressByViewModel(this IOwinContext owinContext, AddressViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Addresses.Any(m => m.StoreAddress == model.StoreAddress))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Address address = new Address();
                AutoMapper.Mapper.Map(model, address);
                dbContext.Addresses.Add(address);
                dbContext.Entry<Address>(address).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditAddressByViewModel(this IOwinContext owinContext, AddressViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var address = dbContext.Addresses.Find(model.Id);
                if (address == null)
                    throw new HttpException(404, "ContentNotFound");

                if (dbContext.Addresses.Any(m => m.StoreAddress == model.StoreAddress && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = address.CreatedDatetime;
                model.CreatedUserId = address.CreatedUserId;

                AutoMapper.Mapper.Map(model, address);
                dbContext.Entry<Address>(address).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteAddressById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var address = dbContext.Addresses.Find(id);
                if (address == null)
                    throw new HttpException(404, "ContentNotFound");

                dbContext.Addresses.Remove(address);
                dbContext.Entry<Address>(address).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Color

        public static IEnumerable<ColorViewModel> GetAllColorViewModels(this IOwinContext owinContext)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<ColorViewModel> models = new List<ColorViewModel>();
            var colors = dbContext.Colors.AsEnumerable();
            AutoMapper.Mapper.Map(colors, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static ColorViewModel GetColorByKey(this IOwinContext owinContext, string colorKey)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var color = dbContext.Colors.Find(colorKey);
                if (color == null)
                    throw new HttpException(404, "ContentNotFound");

                ColorViewModel model = new ColorViewModel(owinContext);
                AutoMapper.Mapper.Map(color, model);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateColorByViewModel(this IOwinContext owinContext, ColorViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Colors.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                TinaShopV2.Models.Entity.Color newColor = new TinaShopV2.Models.Entity.Color();
                AutoMapper.Mapper.Map(model, newColor);
                dbContext.Colors.Add(newColor);
                dbContext.Entry<TinaShopV2.Models.Entity.Color>(newColor).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditColorByViewModel(this IOwinContext owinContext, ColorViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var color = dbContext.Colors.Find(model.ColorKey);
                if (color == null)
                    throw new HttpException(404, "ContentNotFound");

                if (dbContext.Colors.Any(m => m.Name == model.Name && m.ColorKey != model.ColorKey))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = color.CreatedDatetime;
                model.CreatedUserId = color.CreatedUserId;

                AutoMapper.Mapper.Map(model, color);


                dbContext.Entry<TinaShopV2.Models.Entity.Color>(color).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteColorByKey(this IOwinContext owinContext, string colorKey)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var color = dbContext.Colors.Find(colorKey);
                if (color == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.Colors.Remove(color);
                dbContext.Entry<TinaShopV2.Models.Entity.Color>(color).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Slider

        public static IEnumerable<SliderViewModel> GetAllSliderViewModels(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<SliderViewModel> models = new List<SliderViewModel>();
            var sliders = dbContext.Sliders.AsEnumerable();
            AutoMapper.Mapper.Map(sliders, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static SliderViewModel GetSliderById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var slider = dbContext.Sliders.Find(id);
                if (slider == null)
                    throw new HttpException(404, "ContentNotFound");

                SliderViewModel model = new SliderViewModel(owinContext);
                AutoMapper.Mapper.Map(slider, model);

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateSliderByViewModel(this IOwinContext owinContext, SliderViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Sliders.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                Slider newSlider = new Slider();
                AutoMapper.Mapper.Map(model, newSlider);

                dbContext.Sliders.Add(newSlider);
                dbContext.Entry<Slider>(newSlider).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditSliderByViewModel(this IOwinContext owinContext, SliderViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var slider = dbContext.Sliders.Find(model.Id);
                if (slider == null)
                    throw new HttpException(404, "ContentNotFound");

                if (dbContext.Sliders.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = slider.CreatedDatetime;
                model.CreatedUserId = slider.CreatedUserId;

                AutoMapper.Mapper.Map(model, slider);

                dbContext.Entry<Slider>(slider).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteSliderById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var slider = dbContext.Sliders.Find(id);
                if (slider == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.Sliders.Remove(slider);
                dbContext.Entry<Slider>(slider).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Catalog

        public static IEnumerable<CatalogViewModel> GetAllCatalogViewModels(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            List<CatalogViewModel> models = new List<CatalogViewModel>();
            var catalogs = dbContext.Catalogs.AsEnumerable();
            AutoMapper.Mapper.Map(catalogs, models);

            foreach (var item in models)
            {
                item.SetOwinContext(owinContext);
            }

            return models;
        }

        public static CatalogViewModel GetCatalogById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            //var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var catalog = dbContext.Catalogs.Find(id);
                if (catalog == null)
                    throw new HttpException(404, "ContentNotFound");

                CatalogViewModel model = new CatalogViewModel(owinContext);
                AutoMapper.Mapper.Map(catalog, model);

                foreach (var item in model.Products)
                {
                    model.ProductIds += "," + item.ProductCode + ",";
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateCatalogByViewModel(this IOwinContext owinContext, CatalogViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Catalogs.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                string[] productIds = model.ProductIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (productIds.Count() > 0)
                    model.Products = dbContext.Products.Where(m => productIds.Contains(m.ProductCode)).ToList();

                Catalog newCatalog = new Catalog();
                AutoMapper.Mapper.Map(model, newCatalog);

                dbContext.Catalogs.Add(newCatalog);
                dbContext.Entry<Catalog>(newCatalog).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditCatalogByViewModel(this IOwinContext owinContext, CatalogViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var catalog = dbContext.Catalogs.Find(model.Id);
                if (catalog == null)
                    throw new HttpException(404, "ContentNotFound");

                if (dbContext.Catalogs.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedDatetime = catalog.CreatedDatetime;
                model.CreatedUserId = catalog.CreatedUserId;
                //model.SetOwinContext(owinContext);

                if (!string.IsNullOrEmpty(model.ProductIds))
                {
                    string[] productIds = model.ProductIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (productIds.Count() > 0)
                        model.Products = dbContext.Products.Where(m => productIds.Contains(m.ProductCode)).ToList();
                }
                else
                    model.Products = new List<Product>();

                AutoMapper.Mapper.Map(model, catalog);

                dbContext.Entry<Catalog>(catalog).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteCatalogById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var catalog = dbContext.Catalogs.Find(id);
                if (catalog == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.Catalogs.Remove(catalog);
                dbContext.Entry<Catalog>(catalog).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Roles

        public static IEnumerable<RoleViewModel> GetAllRoleViewModel(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            return dbContext.Roles.Select(m => new RoleViewModel() { Id = m.Id, Name = m.Name });
        }

        public static void CreateRoleByViewModel(this IOwinContext owinContext, RoleViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.Roles.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                IdentityRole newRole = new IdentityRole(model.Name);
                dbContext.Roles.Add(newRole);
                dbContext.Entry<IdentityRole>(newRole).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditRoleByViewModel(this IOwinContext owinContext, RoleViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var role = dbContext.Roles.Find(model.Id);
                if (role == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (dbContext.Roles.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                role.Name = model.Name;

                dbContext.Entry<IdentityRole>(role).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteRoleByViewModel(this IOwinContext owinContext, string id)
        {
            if (owinContext == null || string.IsNullOrEmpty(id))
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var role = dbContext.Roles.Find(id);
                if (role == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (dbContext.TinaAuthorizes.Any(m => m.RoleId == id))
                    throw new Exception(App_GlobalResources.Errors.NotDelete_HaveChild);

                dbContext.Roles.Remove(role);
                dbContext.Entry<IdentityRole>(role).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TinaActions

        public static IEnumerable<TinaActionViewModel> GetAllTinaActionViewModel(this IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            IEnumerable<TinaActionViewModel> result = new List<TinaActionViewModel>();
            AutoMapper.Mapper.Map(dbContext.TinaActions.OrderByDescending(m => m.UpdatedDatetime), result);

            foreach (var item in result)
            {
                item.SetOwinContext(owinContext);
            }

            return result;
        }

        public static void CreateTinaActionByViewModel(this IOwinContext owinContext, TinaActionViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var existingItem = dbContext.TinaActions.FirstOrDefault(m => m.Name == model.Name);
                if (existingItem != null)
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                if (dbContext.TinaActions.Any(m => m.Controller.ToLower() == model.Controller.ToLower() && m.Action.ToLower() == model.Action.ToLower() && (m.Area ?? string.Empty).ToLower() == model.Area.ToLower()))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, "Action"));

                TinaAction newAction = new TinaAction();
                AutoMapper.Mapper.Map(model, newAction);
                newAction = dbContext.TinaActions.Add(newAction);
                dbContext.Entry<TinaAction>(newAction).State = EntityState.Added;
                dbContext.SaveChanges();

                if (model.RoleIds != null && model.RoleIds.Count() > 0)
                {
                    foreach (var item in model.RoleIds)
                    {
                        TinaAuthorize newAuthorize = new TinaAuthorize() { ActionId = newAction.Id, RoleId = item };
                        dbContext.TinaAuthorizes.Add(newAuthorize);
                        dbContext.Entry<TinaAuthorize>(newAuthorize).State = EntityState.Added;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditTinaActionByViewModel(this IOwinContext owinContext, TinaActionViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                TinaAction tinaAction = dbContext.TinaActions.Find(model.Id);
                if (tinaAction == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (dbContext.TinaActions.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                if (dbContext.TinaActions.Any(m => m.Controller.ToLower() == model.Controller.ToLower() && m.Action.ToLower() == model.Action.ToLower() && (m.Area ?? string.Empty).ToLower() == model.Area.ToLower() && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, "Action"));

                model.CreatedUserId = tinaAction.CreatedUserId;
                model.CreatedDatetime = tinaAction.CreatedDatetime;

                AutoMapper.Mapper.Map(model, tinaAction);

                dbContext.Entry<TinaAction>(tinaAction).State = EntityState.Modified;
                dbContext.SaveChanges();

                var authorizes = dbContext.TinaAuthorizes.Where(m => m.ActionId == model.Id).ToList();
                foreach (var item in authorizes)
                {
                    dbContext.TinaAuthorizes.Remove(item);
                    dbContext.Entry<TinaAuthorize>(item).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }

                if (model.RoleIds != null && model.RoleIds.Count() > 0)
                {
                    foreach (var item in model.RoleIds)
                    {
                        TinaAuthorize newAuthorize = new TinaAuthorize() { ActionId = model.Id, RoleId = item };
                        dbContext.TinaAuthorizes.Add(newAuthorize);
                        dbContext.Entry<TinaAuthorize>(newAuthorize).State = EntityState.Added;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeleteTinaActionByViewModel(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                TinaAction tinaAction = dbContext.TinaActions.Find(id);

                if (tinaAction == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.TinaActions.Remove(tinaAction);
                dbContext.Entry<TinaAction>(tinaAction).State = EntityState.Deleted;
                dbContext.SaveChanges();
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

        public static IEnumerable<MenuTypeViewModel> GetAllMenuTypeViewModel(this IOwinContext owinContext)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            IEnumerable<MenuTypeViewModel> result = new List<MenuTypeViewModel>();
            AutoMapper.Mapper.Map(dbContext.MenuTypes, result);

            foreach (var item in result)
            {
                item.SetOwinContext(owinContext);
            }

            return result;
        }

        public static void CreateMenuTypeByViewModel(this IOwinContext owinContext, MenuTypeViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (dbContext.MenuTypes.Any(m => m.Name == model.Name))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                MenuType newMenuType = new MenuType();
                AutoMapper.Mapper.Map(model, newMenuType);
                dbContext.MenuTypes.Add(newMenuType);
                dbContext.Entry<MenuType>(newMenuType).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditMenuTypeByViewModel(this IOwinContext owinContext, MenuTypeViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                MenuType menuType = dbContext.MenuTypes.Find(model.Id);
                if (menuType == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                // Check 'Name' existing
                if (dbContext.MenuTypes.Any(m => m.Name == model.Name && m.Id != model.Id))
                    throw new Exception(string.Format(App_GlobalResources.Errors.FieldExisting, App_GlobalResources.Commons.Name));

                model.CreatedUserId = menuType.CreatedUserId;
                model.CreatedDatetime = menuType.CreatedDatetime;

                AutoMapper.Mapper.Map(model, menuType);
                dbContext.Entry<MenuType>(menuType).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMenuTypeByViewModel(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                MenuType menuType = dbContext.MenuTypes.Find(id);

                if (menuType == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.MenuTypes.Remove(menuType);
                dbContext.Entry<MenuType>(menuType).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TinaMenu

        public static IEnumerable<TinaMenuViewModel> GetTinaMenuViewModelByTypeAndParent(this IOwinContext owinContext, int menuTypeId, int? parentId, bool? isHidden = null)
        {
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            IEnumerable<TinaMenu> result;
            if (isHidden == null)
                result = dbContext.TinaMenus.Where(m => m.MenuTypeId == menuTypeId && m.ParentId == parentId).OrderBy(m => m.OrderNumber);
            else
                result = dbContext.TinaMenus.Where(m => m.MenuTypeId == menuTypeId && m.ParentId == parentId && m.IsHidden == isHidden.Value).OrderBy(m => m.OrderNumber);

            IEnumerable<TinaMenuViewModel> model = new List<TinaMenuViewModel>();
            AutoMapper.Mapper.Map(result, model);

            foreach (var item in model)
            {
                item.SetOwinContext(owinContext);
            }

            return model ?? new List<TinaMenuViewModel>();
        }

        public static void CreateTinaMenuByViewModel(this IOwinContext owinContext, TinaMenuViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (!dbContext.MenuTypes.Any(m => m.Id == model.MenuTypeId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.MenuTypeId)));

                if (model.ActionId != null && !dbContext.TinaActions.Any(m => m.Id == model.ActionId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ActionId)));

                if (model.ParentId != null && !dbContext.TinaActions.Any(m => m.Id == model.ParentId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ParentId)));

                if (model.ParentId != null)
                {
                    bool parentIsOk = true;
                    var parent = dbContext.TinaMenus.Find(model.ParentId.Value);
                    while (parent != null && (parentIsOk = !parent.IsHidden))
                    {
                        if (parent.ParentId == null)
                            break;
                        else
                            parent = dbContext.TinaMenus.Find(parent.ParentId.Value);
                    }

                    if (!parentIsOk)
                        throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.ParentId)));
                }

                TinaMenu newMenu = new TinaMenu();
                AutoMapper.Mapper.Map(model, newMenu);
                dbContext.TinaMenus.Add(newMenu);
                dbContext.Entry<TinaMenu>(newMenu).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EditTinaMenuByViewModel(this IOwinContext owinContext, TinaMenuViewModel model)
        {
            if (owinContext == null || model == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                if (!dbContext.MenuTypes.Any(m => m.Id == model.MenuTypeId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.MenuTypeId)));

                if (model.ActionId != null && !dbContext.TinaActions.Any(m => m.Id == model.ActionId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ActionId)));

                if (model.ParentId != null && !dbContext.TinaActions.Any(m => m.Id == model.ParentId))
                    throw new Exception(string.Format(Errors.DataNotExisting_FormatName, model.GetDisplayName(m => m.ParentId)));

                var tinaMenu = dbContext.TinaMenus.Find(model.Id);
                if (tinaMenu == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                if (tinaMenu.MenuTypeId != model.MenuTypeId)
                    throw new Exception(App_GlobalResources.Errors.NotChangeMenuType);

                if (model.ParentId != null)
                {
                    bool parentIsOk = true;
                    var parent = dbContext.TinaMenus.Find(model.ParentId.Value);
                    while (parent != null && (parentIsOk = !parent.IsHidden))
                    {
                        if (parent.ParentId == null)
                            break;
                        else
                            parent = dbContext.TinaMenus.Find(parent.ParentId.Value);
                    }

                    if (!parentIsOk)
                        throw new Exception(string.Format(Errors.NotSelectThisValueFormat, model.GetDisplayName(m => m.ParentId)));
                }

                model.CreatedDatetime = tinaMenu.CreatedDatetime;
                model.CreatedUserId = tinaMenu.CreatedUserId;

                AutoMapper.Mapper.Map(model, tinaMenu);
                dbContext.Entry<TinaMenu>(tinaMenu).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteTinaMenuById(this IOwinContext owinContext, int id)
        {
            if (owinContext == null)
                throw new Exception(App_GlobalResources.Errors.DataNotNull);

            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = owinContext.Get<ApplicationDbContext>();

            try
            {
                var model = dbContext.TinaMenus.Find(id);
                if (model == null)
                    throw new Exception(App_GlobalResources.Errors.DataNotExisting);

                dbContext.TinaMenus.Remove(model);
                dbContext.Entry<TinaMenu>(model).State = EntityState.Deleted;
                dbContext.SaveChanges();
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