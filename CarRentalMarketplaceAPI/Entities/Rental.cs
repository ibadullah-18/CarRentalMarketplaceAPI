namespace CarRentalMarketplaceAPI.Entities;

public enum RentalStatus
{
    Active,
    Completed,
    Cancelled
}
public class Rental
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public Guid RenterId { get; set; }
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
    public DateTimeOffset CreatedDate { get; set; } =DateTimeOffset.UtcNow;
}
