namespace APITaller1.src.Dtos;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
}