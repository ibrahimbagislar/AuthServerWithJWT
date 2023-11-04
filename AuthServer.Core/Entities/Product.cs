
namespace AuthServer.Core.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; } = null!;
    }
}
