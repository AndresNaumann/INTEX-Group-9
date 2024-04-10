﻿using System.Diagnostics;

namespace BrickwellStore.Data
{
    public class EFLegoRepository : ILegoRepository
    {
        private BrickwellContext _context;
        public EFLegoRepository(BrickwellContext temp)
        {
            _context = temp;
        }
        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Customer> Customers => _context.Customers;
        public IQueryable<Order> Orders => _context.Orders;

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(c => c.ProductId == id);
        }


        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<Product> GetRecommendations(int productId)
        {
            var recommendedProductIds = _context.ProductRecommendations.Where(pr => pr.ProductID == productId).Select(pr => pr.RecommendedProductID);

            var recommendedProducts = _context.Products.Where(p => recommendedProductIds.Contains(p.ProductId)).AsQueryable();

            return recommendedProducts;
        }

        public IEnumerable<Product> GetCustomerRecommendations(int customerId)
        {
            var recommendedProductIds = _context.CustomerRecommendations
                .Where(cr => cr.CustomerId == customerId)
                .Select(cr => cr.RecommendedProductId)
                .ToList();

            // Debugging: check the count and the actual IDs being retrieved
            Debug.WriteLine($"Total recommended product IDs: {recommendedProductIds.Count}");
            foreach (var id in recommendedProductIds)
            {
                Debug.WriteLine($"Recommended Product ID: {id}");
            }

            var recommendedProducts = _context.Products
                .Where(p => recommendedProductIds.Contains(p.ProductId))
                .ToList();

            // Debugging: check the count of the products returned
            Debug.WriteLine($"Total recommended products found: {recommendedProducts.Count}");

            return recommendedProducts;
        }

    }
}


