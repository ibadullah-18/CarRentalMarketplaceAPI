namespace CarRentalMarketplaceAPI.Entities;

public class CarImage
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }
}
