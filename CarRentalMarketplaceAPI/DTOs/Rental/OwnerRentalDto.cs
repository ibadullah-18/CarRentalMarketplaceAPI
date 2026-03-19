namespace CarRentalMarketplaceAPI.DTOs.Rental;

public class OwnerRentalDto
{
    public Guid RentalId { get; set; }
    public Guid CarId { get; set; }

    public string CarName { get; set; } = string.Empty;
    public string CarBodyType { get; set; } = string.Empty;

    public Guid RenterId { get; set; }
    public string RenterFullName { get; set; } = string.Empty;
    public string RenterEmail { get; set; } = string.Empty;

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public string PickupLocation { get; set; } = string.Empty;
    public string ReturnLocation { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}