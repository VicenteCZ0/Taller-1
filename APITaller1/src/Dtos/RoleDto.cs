using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.Dtos
{
    public class RoleDto
    {
        public int RoleID { get; set; }
        public required string RolName { get; set; }
    }
}