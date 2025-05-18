using System.ComponentModel.DataAnnotations;

namespace APITaller1.src.Dtos
{
    public class DeleteUserDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "La razón de eliminación es requerida")]
        [MinLength(10, ErrorMessage = "La razón debe tener al menos 10 caracteres")]
        [MaxLength(500, ErrorMessage = "La razón no puede exceder 500 caracteres")]
        public required string Reason { get; set; }
    }
}