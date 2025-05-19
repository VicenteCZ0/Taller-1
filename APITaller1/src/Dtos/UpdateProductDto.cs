namespace APITaller1.src.Dtos;

public class UpdateProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public int StatusID { get; set; }
    public List<IFormFile>? Images { get; set; }

    public List<IFormFile>? NewImages { get; set; }  
    public List<int>? RemoveImageIds { get; set; }   

}
