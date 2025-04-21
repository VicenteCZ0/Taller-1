using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;
using Microsoft.EntityFrameworkCore;  

namespace APITaller1.src.data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Product> Products { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<ShippingAddres> ShippingAddres { get; set; }

}
