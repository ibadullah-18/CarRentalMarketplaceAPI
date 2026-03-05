namespace CarRentalMarketplaceAPI.DTOs.Cart;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}
