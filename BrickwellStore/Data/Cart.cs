using Microsoft.CodeAnalysis;
using static BrickwellStore.Data.Cart;

namespace BrickwellStore.Data
{
    public class Cart
    {

        public List<CartLine> Lines { get; set; } = new List<CartLine>();


        public virtual void AddItem(Product p, int quantity, double price)
        //public void AddItem(CartLine cartLine)

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

        public virtual void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.ProductId == p.ProductId);

        public virtual void Clear() => Lines.Clear();

        public decimal CalculateTotal() => Lines.Sum(x => (decimal)x.Price);

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; } = new();
            public int Quantity { get; set; }
            public double Price { get; set; }

        }

        // New Method Here

        public List<LineItem> CreateLineItems()
        {
            List<LineItem> lineItems = new List<LineItem>(); 

            foreach (var line in Lines)
            {
                LineItem item = new LineItem
                {
                    ProductId = line.Product.ProductId,
                    Quantity = line.Quantity,
                    Rating = 5,
                };

                lineItems.Add(item);
            }

            return lineItems;
        }
    }
}






