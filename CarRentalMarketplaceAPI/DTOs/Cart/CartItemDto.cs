namespace CarRentalMarketplaceAPI.DTOs.Cart;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public decimal PricePerDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}
