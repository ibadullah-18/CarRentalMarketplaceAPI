namespace CarRentalMarketplaceAPI.Entities;

public class Favorite
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CarId { get; set; }

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
}
