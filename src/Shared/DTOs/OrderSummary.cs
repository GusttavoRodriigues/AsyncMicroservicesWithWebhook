

namespace Shared.DTOs
{
    public record OrderSummary
        (int Id, int ProductId, string ProductName, decimal ProductPrice, int Quantity, decimal TotalAmount, DateTime Date
        );

}