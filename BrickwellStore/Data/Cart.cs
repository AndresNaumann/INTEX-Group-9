using Microsoft.CodeAnalysis;
using static BrickwellStore.Data.Cart;

//namespace BrickwellStore.Data
//{
//    public class Cart
//    {

//        public List<CartLine> Lines { get; set; } = new List<CartLine>();

//        private int _lastCartLineId = 0;

//        private int GenerateCartLineId()
//        {
//            return ++_lastCartLineId;
//        }
//        public void AddItem(Product p, int quantity, double price)
//        //public void AddItem(CartLine cartLine)

//        {
//            int cartLineId = GenerateCartLineId();

//            CartLine? line = Lines
//                .Where(x => x.Product.ProductId == p.ProductId)
//                .FirstOrDefault();


//            //has this item already been added to our cart
//            if (line == null)
//            {
//                Lines.Add(new CartLine
//                {
//                    CartLineId = cartLineId,
//                    Product = p,
//                    Quantity = quantity,
//                    Price = price
//                });
//            }
//            else
//            {
//                line.Quantity += quantity;
//                line.Price += price;
//            }
//        }

//        public void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.ProductId == p.ProductId);

//        public void Clear() => Lines.Clear();

//        public decimal CalculateTotal() => Lines.Sum(x => (decimal)x.Price);

//        public class CartLine
//        {
//            public int CartLineId { get; set; }
//            public Product Product { get; set; } = new();
//            public int Quantity { get; set; }
//            public double Price { get; set; }

//        }
//    }
//}





namespace BrickwellStore.Data
{

    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        private int _lastCartLineId = 1;
        private int GenerateCartLineId()
        {
            return _lastCartLineId + 1;
        }
        public void AddItem(Product p, int quantity, double price)
        {
            int cartLineId = GenerateCartLineId();

            CartLine? line = Lines
                .Where(x => x.Product.ProductId == p.ProductId)
                .FirstOrDefault();

            // Has this item already been added to our cart?
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    //CartLineId = cartLineId,
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
            Console.WriteLine(cartLineId);
        }

        public void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.ProductId == p.ProductId);

        public void Clear() => Lines.Clear();

        public decimal CalculateTotal() => Lines.Sum(x => (decimal)x.Price);
        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; } = new();
            public int Quantity { get; set; }
            public double Price { get; set; }
        }
    }
}
