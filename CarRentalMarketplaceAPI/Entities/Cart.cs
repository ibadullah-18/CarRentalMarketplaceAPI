namespace CarRentalMarketplaceAPI.Entities;

public class Cart
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
}
