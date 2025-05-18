namespace APITaller1.src.Dtos;

public class UpdateAddressDto
{
    public string Street { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string Commune { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
}