using System.Collections.Generic;

namespace APITaller1.src.models;

public class Role
{
    public int RoleID { get; set; }
    public required string RolName { get; set; }
    
    // Relación con usuarios
    public ICollection<User> Users { get; set; } = new List<User>();
}