using MassTransit;
using OrderApi.Data;
using Shared.Models;

namespace OrderApi.Consumer
{
    public class ProductConsumer : IConsumer<Product>
    {
        private readonly OrderDbContext _db;
        public ProductConsumer(OrderDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<Product> context)
        {
            _db.Products.Add(context.Message);
            await _db.SaveChangesAsync();
        }
    }
}
