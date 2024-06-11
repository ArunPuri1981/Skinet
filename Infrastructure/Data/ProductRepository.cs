using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _storeContext;
        public ProductRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }



        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _storeContext.ProductBrand.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _storeContext.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecPrams productSpecPrams)
        {
            IQueryable<Product> query = _storeContext.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType);

            /*---Filter----*/
            if (productSpecPrams.BrandId.HasValue)
            {
                query = query.Where(p => p.ProductBrandId == productSpecPrams.BrandId.Value);
            }
            if (productSpecPrams.TypeId.HasValue)
            {
                query = query.Where(p => p.ProductTypeId == productSpecPrams.TypeId.Value);
            }
            /*-----*/

            //Sourting
            query = productSpecPrams.Sort switch 
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };

            //Searching
            if (!string.IsNullOrEmpty(productSpecPrams.Search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(productSpecPrams.Search));
            }

            //Pagination
            query = query.Skip(productSpecPrams.PageSize * (productSpecPrams.PageIndex - 1)).Take(productSpecPrams.PageSize);

            return await query.ToListAsync();
            
            // _storeContext.Products
            // .Include(p => p.ProductBrand)
            // .Include(p => p.ProductType)
            // .OrderBy(x=>x.Name)
            // .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _storeContext.ProductTypes.ToListAsync();
        }

        public async Task<int> CountAsync(ProductSpecPrams productSpecPrams)
        {
            IQueryable<Product> query = _storeContext.Products;
            if (productSpecPrams.BrandId.HasValue)
            {
                query = query.Where(p => p.ProductBrandId == productSpecPrams.BrandId.Value);
            }
            if (productSpecPrams.TypeId.HasValue)
            {
                query = query.Where(p => p.ProductTypeId == productSpecPrams.TypeId.Value);

            }

            if (!string.IsNullOrEmpty(productSpecPrams.Search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(productSpecPrams.Search));
            }

            return await query.CountAsync();
        }
    }
}