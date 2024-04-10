using BrickwellStore.Infrastructure;
using System.Text.Json.Serialization;

namespace BrickwellStore.Data
{
    public class SessionCart : Cart
    {
        public static Cart GetCart (IServiceProvider services)
        {
            ISession? session = services.GetService<IHttpContextAccessor>()
                .HttpContext?.Session;

            SessionCart cart = session?.GetJson<SessionCart>("Cart") ??
                new SessionCart();

            cart.Session = session;

            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product p, int quantity, double price)
        {
            base.AddItem(p, quantity, price);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Product p)
        {
            base.RemoveLine(p);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}
