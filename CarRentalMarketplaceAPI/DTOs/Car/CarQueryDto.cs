namespace CarRentalMarketplaceAPI.DTOs.Car;

public class CarQueryDto
{
    public string? Search { get; set; }
    public string? Brand { get; set; }
    public string? Location { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
}