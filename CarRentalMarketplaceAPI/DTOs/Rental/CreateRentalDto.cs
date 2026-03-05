namespace CarRentalMarketplaceAPI.DTOs.Rental;

public class CreateRentalDto
{
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
