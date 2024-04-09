﻿using Microsoft.CodeAnalysis;

namespace BrickwellStore.Data
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Product p, int quantity, double price)
        {
            CartLine? line = Lines
                .Where(x => x.Product.ProductId == p.ProductId)
                .FirstOrDefault();

            //has this item already been added to our cart
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = p,
                    Quantity = quantity,
                    Price = price
                });
            }
            else
            {
                line.Quantity += quantity;
                line.Price += price;
            }
        }

        public void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.ProductId == p.ProductId);

        public void Clear() => Lines.Clear();

        public decimal CalculateTotal() => Lines.Sum(x => (decimal)x.Price * x.Quantity);

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; } = new();
            public int Quantity { get; set; }
            public double Price { get; set; }

        }
    }
}