namespace CarRentalMarketplaceAPI.DTOs.Cart;

public class AddCartItemDto
{
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
