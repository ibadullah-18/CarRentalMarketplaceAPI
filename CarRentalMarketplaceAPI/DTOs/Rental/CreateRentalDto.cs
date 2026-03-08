namespace CarRentalMarketplaceAPI.DTOs.Rental;

public class CreateRentalDto
{
    public Guid CarId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
