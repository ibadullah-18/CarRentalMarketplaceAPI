namespace CarRentalMarketplaceAPI.DTOs.Favorite;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
