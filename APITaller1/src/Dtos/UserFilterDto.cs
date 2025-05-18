namespace APITaller1.src.Dtos;

public class UserFilterDto
{
    public bool? AccountStatus { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public DateTime? RegisteredAfter { get; set; }
    public DateTime? RegisteredBefore { get; set; }
    public int Page { get; set; } = 1;
}
