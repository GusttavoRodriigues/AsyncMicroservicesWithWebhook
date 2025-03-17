using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using Shared.DTOs;
using Shared.Models;
using System.Text;

namespace OrderApi.Repository
{
    public class OrderRepo : IOrder
    {
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderRepo(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ServiceResponse> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderSummary = await GetOrderSummaryAsync();
            string content = BuildOrderEmailBody(
                orderSummary.Id,
                orderSummary.ProductName,
                orderSummary.ProductPrice,
                orderSummary.Quantity,
                orderSummary.TotalAmount,
                orderSummary.Date
                );

            await _publishEndpoint.Publish(new EmailDTO("Informações do Pedido",content));
            await ClearOrderTable();
            return new ServiceResponse(true, "Order palced successfully");

        }

        public async Task<List<Order>> GetAllOrderAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task<OrderSummary> GetOrderSummaryAsync()
        {
            var order = await _context.Orders.FirstOrDefaultAsync();
            var products = await _context.Products.ToListAsync();

            var productInfo = products.Find(x => x.Id == order!.ProductId);
            return new OrderSummary(
                order!.Id,
                productInfo!.Id,
                productInfo.Name,
                productInfo.Price,
                order.Quantity, 
                order.Quantity * productInfo.Price,
                order.Date
                );
        }
    
    
        private string BuildOrderEmailBody(int orderId, string productName, decimal price, int orderQuantity, decimal totalAmount, DateTime date)
        {
            var sb=new StringBuilder();
            sb.AppendLine("<h1><strong>Informações do pedido</strong></h1>");
            sb.AppendLine($"<p><strong>ID do pedido: </strong> {orderId} </p>");

            sb.AppendLine("<h2>Item do pedido: </h2>");
                sb.AppendLine("<ul>");
                sb.AppendLine($"<li>Nome: {productName} </li>");
                sb.AppendLine($"<li>Preço: {price} </li>");
                sb.AppendLine($"<li>Quantidade: {orderQuantity} </li>");
                sb.AppendLine($"<li>valor total: {totalAmount} </li>");
                sb.AppendLine($"<li>Data do pedido: {date} </li>");

            sb.AppendLine("</ul>");

            sb.AppendLine("<p>Obrigado pelo seu pedido!</p>");

            return sb.ToString();
        }

        private async Task ClearOrderTable()
        {
            _context.Orders.Remove(await _context.Orders.FirstOrDefaultAsync());
            await _context.SaveChangesAsync();
        }
    }
}
