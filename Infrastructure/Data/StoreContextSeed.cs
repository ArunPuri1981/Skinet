using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsnyc(StoreContext storeContext)
        {
            if (!storeContext.ProductBrand.Any())
            {
                var brandData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                storeContext.ProductBrand.AddRange(brand);
            }
            if (!storeContext.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                storeContext.ProductTypes.AddRange(types);
            }
            if (!storeContext.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                storeContext.Products.AddRange(products);
            }

            if (!storeContext.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                storeContext.DeliveryMethods.AddRange(methods);
            }

            if (storeContext.ChangeTracker.HasChanges()) await storeContext.SaveChangesAsync();

        }
    }
}