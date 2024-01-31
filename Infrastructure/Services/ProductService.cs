using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ProductRepositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductService(ProductFactories productFactories, ProductRepository productRepository)
{
    private readonly ProductFactories _productFactories = productFactories;
    private readonly ProductRepository _productRepository = productRepository;

    public bool CreateProduct(ProductDto product)
    {
        try
        {
            Manufacturer manufacturer = _productFactories.GetOrCreateManufacturerEntity(product.ManufacturerName);
            Category category = _productFactories.GetOrCreateCategoryEntity(product.CategoryName);

            Product productEntity = _productFactories.CreateProductEntity(product.Title, product.Description, product.Price, manufacturer.Id, category.Id);
            if (productEntity != null)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
    public IEnumerable<ProductDto> GetAllProducts() 
    {
        var productList = _productRepository.GetAll();
        var productDtoList = new List<ProductDto>();

        foreach (var item in productList)
        {
            var product = _productFactories.CompileFullProduct(item);
            productDtoList.Add(product);
        }

        return productDtoList;
    }
}
