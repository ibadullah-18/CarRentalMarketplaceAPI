namespace CarRentalMarketplaceAPI.DTOs.Favorite;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
