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
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
