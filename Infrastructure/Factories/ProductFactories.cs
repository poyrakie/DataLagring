﻿using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class ProductFactories(CategoryRepository categoryRepository, ProductRepository productRepository, ManufacturerRepository manufacturerRepository, OrderRepository orderRepository, OrderRowRepository orderRowRepository)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly ProductRepository _productRepository = productRepository;
    private readonly ManufacturerRepository _manufacturerRepository = manufacturerRepository;
    private readonly OrderRepository _orderRepository = orderRepository;
    private readonly OrderRowRepository _orderRowRepository = orderRowRepository;

    public Category GetOrCreateCategoryEntity(string categoryName)
    {
        try
        {
            Category categoryEntity = new Category
            {
                Name = categoryName
            };
            if (_categoryRepository.Exists(x => x.Name == categoryEntity.Name))
            {
                categoryEntity = _categoryRepository.GetOne(x => x.Name == categoryEntity.Name);
            }
            else
            {
                categoryEntity = _categoryRepository.Create(categoryEntity);
            }
            return categoryEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public Manufacturer GetOrCreateManufacturerEntity(string manufacturerName)
    {
        try
        {
            Manufacturer manufacturerEntity = new Manufacturer
            {
                Name = manufacturerName
            };
            if (_manufacturerRepository.Exists(x => x.Name == manufacturerEntity.Name))
            {
                manufacturerEntity = _manufacturerRepository.GetOne(x => x.Name == manufacturerEntity.Name);
            }
            else
            {
                manufacturerEntity = _manufacturerRepository.Create(manufacturerEntity);
            }
            return manufacturerEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public Product CreateProductEntity(string title, string description, decimal price, int categoryId, int manufacturerId)
    {
        try
        {
            if(_categoryRepository.Exists(x => x.Id ==  categoryId) && _manufacturerRepository.Exists(x => x.Id == manufacturerId))
            {
                Product productEntity = new Product
                {
                    Title = title,
                    Description = description,
                    Price = price,
                    CategoryId = categoryId,
                    ManufacturerId = manufacturerId
                };
                productEntity = _productRepository.Create(productEntity);

                return productEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public Order CreateOrderEntity(int userId)
    {
        try
        {
            Order orderEntity = new Order
            {
                UserId = userId
            };
            orderEntity = _orderRepository.Create(orderEntity);

            return orderEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public OrderRow CreateOrderRowEntity(int orderId, int productId)
    {
        try
        {
            if (_orderRepository.Exists(x => x.Id == orderId) && _productRepository.Exists(x => x.Id == productId))
            {
                OrderRow orderRowEntity = new OrderRow
                {
                    OrderId = orderId,
                    ProductId = productId
                };
                orderRowEntity = _orderRowRepository.Create(orderRowEntity);

                return orderRowEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public ProductDto CompileFullProduct(Product entity)
    {
        try
        {
            Category category = _categoryRepository.GetOne(x => x.Id == entity.ManufacturerId);
            Manufacturer manufacturer = _manufacturerRepository.GetOne(x => x.Id == entity.ManufacturerId);

            ProductDto productDto = new ProductDto
            {
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                CategoryName = category.Name,
                ManufacturerName = manufacturer.Name,
                Id = entity.Id
            };
            return productDto;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
    public IEnumerable<Order> GetAllOrders(int userId) 
    {
        try
        {
            if (_orderRepository.Exists(x => x.UserId == userId))
            {
                List<Order> updatedOrderList = [];
                List<Order> orderList = _orderRepository.GetAll().ToList();
                foreach (var order in orderList)
                {
                    if (userId == order.UserId)
                    {
                        updatedOrderList.Add(order);
                    }
                }
                return updatedOrderList;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        List<Order> emptyList = [];
        return emptyList;
    }
    public IEnumerable<OrderRow> GetAllOrderRows(int orderId) 
    {
        try
        {
            if(_orderRepository.Exists(x => x.Id == orderId))
            {
                List<OrderRow> updatedOrderRows = [];
                List<OrderRow> orderRows = _orderRowRepository.GetAll().ToList();
                foreach (var row in orderRows)
                {
                    if (orderId == row.OrderId)
                    {
                        updatedOrderRows.Add(row);
                    }
                }
                return updatedOrderRows;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }
}
