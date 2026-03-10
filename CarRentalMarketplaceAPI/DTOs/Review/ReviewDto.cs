namespace CarRentalMarketplaceAPI.DTOs.Review;

public class ReviewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CarId { get; set; }
    public string UserFullName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
}
