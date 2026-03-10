namespace CarRentalMarketplaceAPI.DTOs.Favorite;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal PricePerDay { get; set; }
    public string Color { get; set; }
    public string Location { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
